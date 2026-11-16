import type { ProductionState } from '../production/ProductionState';
import type { CalendarDay } from './CalendarDay';
import type { DayState } from './DayState';

/**
 * The whole game at a point in time. Everything here except `day` carries over from one day to
 * the next (stock, and later Beebucks, deliveries and upgrades); `day` is today's own state.
 */
export interface GameState {
  readonly calendar: CalendarDay;
  readonly production: ProductionState;
  readonly day: DayState;
}
