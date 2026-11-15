import type { Position } from '../Position';
import type { Task } from './Task';

/**
 * - `running`: the clock is counting and tasks can be queued.
 * - `windingDown`: time is up (or the day was ended early). The active task finishes, nothing
 *   else runs, and nothing can be queued.
 * - `ended`: the day is over.
 */
export type DayStatus = 'running' | 'windingDown' | 'ended';

export interface ActiveTask {
  readonly task: Task;
  /** Every task first walks to its station, then works there. */
  readonly phase: 'walking' | 'working';
  readonly workRemainingSeconds: number;
}

/** Complete state of one work day. Plain readonly data, so it can go straight into a save. */
export interface WorkDayState {
  readonly status: DayStatus;
  /**
   * Day time used so far. Stops at the day length, and stops early if the day is ended early,
   * so `workDaySeconds - elapsedSeconds` is the unused time (a future banked-time upgrade).
   */
  readonly elapsedSeconds: number;
  readonly beekeeperPosition: Position;
  readonly activeTask: ActiveTask | null;
  /** Waiting tasks in run order, not including the active task. */
  readonly queuedTasks: readonly Task[];
  readonly nextTaskId: number;
}
