import type { ActiveTask, WorkDayState } from '../workday/WorkDayState';

/**
 * Current save format version. Whenever `SaveData` (or anything it contains, such as
 * `WorkDayState` or a station's state) changes shape, bump this and add a migration in
 * `migrations.ts`, with a test that an old save still loads.
 */
export const CURRENT_SAVE_VERSION = 1;

/**
 * A work day as saved. A task in progress is saved as restarting from its walk: its partial work
 * is lost on reload, but a save never has to hold a half-applied action.
 */
export type SavedWorkDayState = Omit<WorkDayState, 'activeTask'> & {
  readonly activeTask: Extract<ActiveTask, { phase: 'walking' }> | null;
};

/** The persisted game. */
export interface SaveData {
  readonly version: typeof CURRENT_SAVE_VERSION;
  /** 1 for the first day of a new game. */
  readonly dayNumber: number;
  readonly workDay: SavedWorkDayState;
}
