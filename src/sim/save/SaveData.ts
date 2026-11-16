import type { ProductionState } from '../production/ProductionState';
import type { CalendarDay } from '../week/CalendarDay';
import type { DayState } from '../week/DayState';
import type { ActiveTask, WorkDayState } from '../workday/WorkDayState';

/**
 * Current save format version. Not released yet, so its shape can still change freely; see
 * the save rules in CLAUDE.md for what changes once a version ships.
 */
export const CURRENT_SAVE_VERSION = 1;

/**
 * A work day as saved. A task in progress is saved as restarting from its walk: its partial work
 * is lost on reload, but a save never has to hold a half-applied action.
 */
export type SavedWorkDayState = Omit<WorkDayState, 'activeTask'> & {
  readonly activeTask: Extract<ActiveTask, { phase: 'walking' }> | null;
};

/** The current day as saved: a work day uses the saved work day shape. */
export type SavedDayState =
  | Exclude<DayState, { kind: 'work' }>
  | { readonly kind: 'work'; readonly workDay: SavedWorkDayState };

/** The persisted game. Loads straight back as a `GameState`. */
export interface SaveData {
  readonly version: typeof CURRENT_SAVE_VERSION;
  readonly calendar: CalendarDay;
  readonly production: ProductionState;
  readonly day: SavedDayState;
}
