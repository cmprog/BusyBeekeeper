export const weekdays = [
  'monday',
  'tuesday',
  'wednesday',
  'thursday',
  'friday',
  'saturday',
  'sunday',
] as const;

export type Weekday = (typeof weekdays)[number];

/** What a day is for: Mon–Fri work, Saturday market, Sunday supply (ROADMAP §3.4). */
export type DayKind = 'work' | 'market' | 'supply';

export function dayKindOf(weekday: Weekday): DayKind {
  switch (weekday) {
    case 'monday':
    case 'tuesday':
    case 'wednesday':
    case 'thursday':
    case 'friday':
      return 'work';
    case 'saturday':
      return 'market';
    case 'sunday':
      return 'supply';
  }
}
