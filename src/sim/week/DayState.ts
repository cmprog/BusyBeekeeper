import type { WorkDayState } from '../workday/WorkDayState';

/**
 * What only exists for today, by kind of day: it's thrown away at night. A work day runs the task
 * queue; the market and supply days have no state of their own yet (Phase 2 adds it).
 * Anything that lasts longer than a day belongs on `GameState` instead.
 */
export type DayState =
  | { readonly kind: 'work'; readonly workDay: WorkDayState }
  | { readonly kind: 'market' }
  | { readonly kind: 'supply' };
