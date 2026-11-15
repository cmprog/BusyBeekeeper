import type { GameConfig } from '../../config/GameConfig';
import { distanceBetween } from '../Position';
import { fillHive } from '../production/fillHive';
import { initialProductionState } from '../production/initialProductionState';
import type { ProductionState } from '../production/ProductionState';
import type { Station, StationId } from '../stations/Station';
import type { PlannedAction } from '../stations/StationAction';
import type { StationContext } from '../stations/StationBehavior';
import { applyPlannedAction, resolveStationTask } from '../stations/stationBehaviors';
import { countCompletedAction, countFailedTask, emptyDayStats } from './DayStats';
import type { Task } from './Task';
import type { WorkDayEvent } from './WorkDayEvent';
import type { ActiveTask, WorkDayState } from './WorkDayState';

export type EnqueueRefusal = 'queueFull' | 'dayOver';

export type EnqueueResult =
  | { readonly ok: true; readonly task: Task }
  | { readonly ok: false; readonly reason: EnqueueRefusal };

/** How far the active task got in one step, and what happened if it finished. */
interface TaskProgress {
  readonly usedSeconds: number;
  readonly outcome?: WorkDayEvent;
  /** Set when the task completed: the action to apply, and where. */
  readonly completed?: { readonly planned: PlannedAction; readonly station: Station };
}

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

  /** A fresh day. Pass the previous day's production so stock carries over. */
  static initialState(
    config: GameConfig,
    production: ProductionState = initialProductionState(config),
  ): WorkDayState {
    return {
      status: 'running',
      elapsedSeconds: 0,
      beekeeperPosition: config.backyard.beekeeperStart,
      activeTask: null,
      queuedTasks: [],
      nextTaskId: 1,
      production,
      stats: emptyDayStats,
    };
  }

  /** Read-only snapshot. Safe to save or render; changes go through the methods below. */
  get state(): WorkDayState {
    return this.#state;
  }

  get remainingSeconds(): number {
    return Math.max(0, this.#config.time.workDaySeconds - this.#state.elapsedSeconds);
  }

  /**
   * Adds a task at a station to the end of the queue, starting it right away if the beekeeper
   * is idle. The station must be in this scene's layout.
   */
  enqueue(stationId: StationId): EnqueueResult {
    this.#station(stationId); // throws for an unknown station: always a caller bug
    const state = this.#state;
    if (state.status !== 'running') {
      return { ok: false, reason: 'dayOver' };
    }
    const taskCount = state.queuedTasks.length + (state.activeTask ? 1 : 0);
    if (taskCount >= this.#config.queue.maxTasks) {
      return { ok: false, reason: 'queueFull' };
    }

    const task: Task = { id: state.nextTaskId, stationId };
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

  /** Advances time. Returns what happened, in order, for the presentation layer. */
  tick(dtSeconds: number): readonly WorkDayEvent[] {
    if (!Number.isFinite(dtSeconds) || dtSeconds < 0) {
      throw new RangeError(`tick needs a finite, non-negative dt (got ${String(dtSeconds)})`);
    }

    const events: WorkDayEvent[] = [];
    let remaining = dtSeconds;
    while (remaining > 0 && this.#state.status !== 'ended') {
      const running = this.#state.status === 'running';
      // While running, never step past the end of the day: the day has to close at that exact
      // moment so the queue is dropped before another task can start.
      const step = running ? Math.min(remaining, this.remainingSeconds) : remaining;

      const activeTask = this.#state.activeTask;
      const progress: TaskProgress = activeTask
        ? this.#progressActiveTask(activeTask, step)
        : { usedSeconds: step };
      const used = progress.usedSeconds;
      remaining -= used;

      // The step's time passes before its outcome applies, so a super pulled at the end of the
      // step includes the honey the bees made during it.
      this.#state = {
        ...this.#state,
        elapsedSeconds: running ? this.#state.elapsedSeconds + used : this.#state.elapsedSeconds,
        production: fillHive(this.#state.production, used, this.#config),
      };
      if (progress.completed) {
        const { planned, station } = progress.completed;
        this.#state = {
          ...this.#state,
          production: applyPlannedAction(planned, this.#context(station)),
        };
      }
      if (progress.outcome) {
        this.#count(progress.outcome);
        events.push(progress.outcome);
      }

      if (running && this.remainingSeconds <= 0) {
        this.#closeDay();
      }
      this.#startNextTask();
      this.#endIfWoundDown();
    }
    return events;
  }

  /**
   * Advances the active task by up to `limitSeconds`, stopping early if it completes or fails.
   * Production effects are left to the caller (see `tick`).
   */
  #progressActiveTask(activeTask: ActiveTask, limitSeconds: number): TaskProgress {
    const task = activeTask.task;
    const station = this.#station(task.stationId);
    let current = activeTask;
    let used = 0;

    if (current.phase === 'walking') {
      const position = this.#state.beekeeperPosition;
      const target = station.position;
      const arriveSeconds =
        distanceBetween(position, target) / this.#config.beekeeper.walkSpeedUnitsPerSecond;
      if (limitSeconds < arriveSeconds) {
        const fraction = limitSeconds / arriveSeconds;
        this.#state = {
          ...this.#state,
          beekeeperPosition: {
            x: position.x + (target.x - position.x) * fraction,
            y: position.y + (target.y - position.y) * fraction,
          },
        };
        return { usedSeconds: limitSeconds };
      }

      used = arriveSeconds;
      this.#state = { ...this.#state, beekeeperPosition: target };
      const resolution = resolveStationTask(this.#context(station));
      if (!resolution.ok) {
        this.#state = { ...this.#state, activeTask: null };
        return {
          usedSeconds: used,
          outcome: { type: 'taskFailed', task, reason: resolution.reason },
        };
      }
      current = {
        phase: 'working',
        task,
        planned: resolution.planned,
        workTotalSeconds: resolution.workSeconds,
        workRemainingSeconds: resolution.workSeconds,
      };
    }

    const available = limitSeconds - used;
    if (available < current.workRemainingSeconds) {
      this.#state = {
        ...this.#state,
        activeTask: { ...current, workRemainingSeconds: current.workRemainingSeconds - available },
      };
      return { usedSeconds: limitSeconds };
    }

    this.#state = { ...this.#state, activeTask: null };
    return {
      usedSeconds: used + current.workRemainingSeconds,
      outcome: { type: 'taskCompleted', task, action: current.planned.action },
      completed: { planned: current.planned, station },
    };
  }

  #count(outcome: WorkDayEvent): void {
    const stats =
      outcome.type === 'taskCompleted'
        ? countCompletedAction(this.#state.stats, outcome.action)
        : countFailedTask(this.#state.stats);
    this.#state = { ...this.#state, stats };
  }

  #station(stationId: StationId): Station {
    const station = this.#config.backyard.stations.find((candidate) => candidate.id === stationId);
    if (!station) {
      throw new RangeError(`No station '${stationId}' in this scene`);
    }
    return station;
  }

  #context(station: Station): StationContext {
    return {
      station,
      state: this.#state.production,
      layout: this.#config.backyard.stations,
      config: this.#config,
    };
  }

  #startNextTask(): void {
    const state = this.#state;
    const [next, ...rest] = state.queuedTasks;
    if (state.status !== 'running' || state.activeTask || !next) {
      return;
    }
    this.#state = {
      ...state,
      activeTask: { phase: 'walking', task: next },
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
}
