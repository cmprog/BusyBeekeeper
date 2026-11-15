import type { Position } from '../Position';
import type { ProductionState } from '../production/ProductionState';
import type { PlannedAction } from '../stations/StationAction';
import type { DayStats } from './DayStats';
import type { Task } from './Task';

/**
 * - `running`: the clock is counting and tasks can be queued.
 * - `windingDown`: time is up (or the day was ended early). The active task finishes, nothing
 *   else runs, and nothing can be queued.
 * - `ended`: the day is over.
 */
export type DayStatus = 'running' | 'windingDown' | 'ended';

/** Every task first walks to its station; on arrival its action is decided and work starts. */
export type ActiveTask =
  | { readonly phase: 'walking'; readonly task: Task }
  | {
      readonly phase: 'working';
      readonly task: Task;
      readonly planned: PlannedAction;
      readonly workTotalSeconds: number;
      readonly workRemainingSeconds: number;
    };

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
  /** Carries over from day to day; a work day only borrows it. */
  readonly production: ProductionState;
  readonly stats: DayStats;
}
