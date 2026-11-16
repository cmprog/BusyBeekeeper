import type { CalendarDay } from '../sim/week/CalendarDay';
import type { Weekday } from '../sim/week/Weekday';

const weekdayNames: Record<Weekday, string> = {
  monday: 'Monday',
  tuesday: 'Tuesday',
  wednesday: 'Wednesday',
  thursday: 'Thursday',
  friday: 'Friday',
  saturday: 'Saturday',
  sunday: 'Sunday',
};

export function weekdayName(weekday: Weekday): string {
  return weekdayNames[weekday];
}

/** e.g. "Thursday, week 1". */
export function dayName({ week, weekday }: CalendarDay): string {
  return `${weekdayName(weekday)}, week ${String(week)}`;
}

/** e.g. "Thu · Week 1", for the HUD corner. */
export function shortDayName({ week, weekday }: CalendarDay): string {
  return `${weekdayName(weekday).slice(0, 3)} · Week ${String(week)}`;
}
