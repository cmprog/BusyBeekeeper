import type { GameConfig } from '../../config/GameConfig';
import { distanceBetween } from '../Position';
import type { StationId } from '../StationId';
import type { Task } from './Task';
import type { ActiveTask, WorkDayState } from './WorkDayState';

export type EnqueueResult =
  | { readonly ok: true; readonly task: Task }
  | { readonly ok: false; readonly reason: 'queueFull' | 'dayOver' };

/**
 * One work day in a scene: the day clock plus the beekeeper's task queue (ROADMAP §3.1, §3.4).
 *
 * Time only moves through `tick(dt)`. One large `dt` gives the same result as many small ones,
 * so banked time and offline catch-up can be fed in as a single big step.
 */
export class WorkDay {
  readonly #config: GameConfig;
  #state: WorkDayState;

  constructor(config: GameConfig, state: WorkDayState = WorkDay.initialState(config)) {
    this.#config = config;
    this.#state = state;
  }

  static initialState(config: GameConfig): WorkDayState {
    return {
      status: 'running',
      elapsedSeconds: 0,
      beekeeperPosition: config.backyard.beekeeperStart,
      activeTask: null,
      queuedTasks: [],
      nextTaskId: 1,
    };
  }

  /** Read-only snapshot. Safe to save or render; changes go through the methods below. */
  get state(): WorkDayState {
    return this.#state;
  }

  get remainingSeconds(): number {
    return Math.max(0, this.#config.time.workDaySeconds - this.#state.elapsedSeconds);
  }

  /** Adds a task to the end of the queue, starting it right away if the beekeeper is idle. */
  enqueue(stationId: StationId): EnqueueResult {
    const state = this.#state;
    if (state.status !== 'running') {
      return { ok: false, reason: 'dayOver' };
    }
    const taskCount = state.queuedTasks.length + (state.activeTask ? 1 : 0);
    if (taskCount >= this.#config.queue.maxTasks) {
      return { ok: false, reason: 'queueFull' };
    }

    const task: Task = { kind: 'placeholder', id: state.nextTaskId, stationId };
    this.#state = {
      ...state,
      queuedTasks: [...state.queuedTasks, task],
      nextTaskId: state.nextTaskId + 1,
    };
    this.#startNextTask();
    return { ok: true, task };
  }

  /**
   * Cancels a queued or active task. Cancelling the active task stops the beekeeper where they
   * are and loses its progress; the next queued task then starts. Returns false if not found.
   */
  cancel(taskId: number): boolean {
    const state = this.#state;
    if (state.activeTask?.task.id === taskId) {
      this.#state = { ...state, activeTask: null };
      this.#startNextTask();
      this.#endIfWoundDown();
      return true;
    }

    const queuedTasks = state.queuedTasks.filter((task) => task.id !== taskId);
    if (queuedTasks.length === state.queuedTasks.length) {
      return false;
    }
    this.#state = { ...state, queuedTasks };
    return true;
  }

  /** Same rules as running out of time: the active task finishes, the rest are dropped. */
  endDayEarly(): void {
    if (this.#state.status === 'running') {
      this.#closeDay();
    }
  }

  tick(dtSeconds: number): void {
    if (!Number.isFinite(dtSeconds) || dtSeconds < 0) {
      throw new RangeError(`tick needs a finite, non-negative dt (got ${String(dtSeconds)})`);
    }

    let remaining = dtSeconds;
    while (remaining > 0 && this.#state.status !== 'ended') {
      const running = this.#state.status === 'running';
      // While running, never step past the end of the day: the day has to close at that exact
      // moment so the queue is dropped before another task can start.
      const step = running ? Math.min(remaining, this.remainingSeconds) : remaining;

      const activeTask = this.#state.activeTask;
      const used = activeTask ? this.#advanceActiveTask(activeTask, step) : step;

      remaining -= used;
      if (running) {
        this.#state = { ...this.#state, elapsedSeconds: this.#state.elapsedSeconds + used };
        if (this.remainingSeconds <= 0) {
          this.#closeDay();
        }
      }
      this.#startNextTask();
      this.#endIfWoundDown();
    }
  }

  /**
   * Advances the active task by up to `limitSeconds`, stopping early if it completes.
   * Returns the time actually used.
   */
  #advanceActiveTask(activeTask: ActiveTask, limitSeconds: number): number {
    let used = 0;
    let { phase, workRemainingSeconds } = activeTask;
    let position = this.#state.beekeeperPosition;

    if (phase === 'walking') {
      const target = this.#config.backyard.stations[activeTask.task.stationId];
      const arriveSeconds =
        distanceBetween(position, target) / this.#config.beekeeper.walkSpeedUnitsPerSecond;
      if (limitSeconds < arriveSeconds) {
        const fraction = limitSeconds / arriveSeconds;
        position = {
          x: position.x + (target.x - position.x) * fraction,
          y: position.y + (target.y - position.y) * fraction,
        };
        this.#state = { ...this.#state, beekeeperPosition: position };
        return limitSeconds;
      }
      position = target;
      used = arriveSeconds;
      phase = 'working';
    }

    const available = limitSeconds - used;
    if (available < workRemainingSeconds) {
      workRemainingSeconds -= available;
      this.#state = {
        ...this.#state,
        beekeeperPosition: position,
        activeTask: { ...activeTask, phase, workRemainingSeconds },
      };
      return limitSeconds;
    }

    // Completed. Production effects get applied here in Phase 1 step 3.
    this.#state = { ...this.#state, beekeeperPosition: position, activeTask: null };
    return used + workRemainingSeconds;
  }

  #startNextTask(): void {
    const state = this.#state;
    const [next, ...rest] = state.queuedTasks;
    if (state.status !== 'running' || state.activeTask || !next) {
      return;
    }
    this.#state = {
      ...state,
      activeTask: { task: next, phase: 'walking', workRemainingSeconds: this.#workSeconds(next) },
      queuedTasks: rest,
    };
  }

  #closeDay(): void {
    this.#state = { ...this.#state, status: 'windingDown', queuedTasks: [] };
    this.#endIfWoundDown();
  }

  #endIfWoundDown(): void {
    if (this.#state.status === 'windingDown' && !this.#state.activeTask) {
      this.#state = { ...this.#state, status: 'ended' };
    }
  }

  #workSeconds(task: Task): number {
    switch (task.kind) {
      // eslint-disable-next-line @typescript-eslint/no-unnecessary-condition -- Task has one variant until step 3; the switch keeps the exhaustiveness check in place.
      case 'placeholder':
        return this.#config.tasks.placeholderWorkSeconds;
    }
  }
}
