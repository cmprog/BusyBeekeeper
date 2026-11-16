import { weekdays } from './Weekday';
import type { Weekday } from './Weekday';

/** A day in the game's calendar. Weeks run Monday to Sunday and count from 1. */
export interface CalendarDay {
  readonly week: number;
  readonly weekday: Weekday;
}

export function nextCalendarDay({ week, weekday }: CalendarDay): CalendarDay {
  const next = weekdays[(weekdays.indexOf(weekday) + 1) % weekdays.length] ?? 'monday';
  return { week: next === 'monday' ? week + 1 : week, weekday: next };
}
