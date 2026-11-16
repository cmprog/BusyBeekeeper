import type { GameConfig } from '../../config/GameConfig';
import { fillHive } from '../production/fillHive';
import { initialProductionState } from '../production/initialProductionState';
import { WorkDay } from '../workday/WorkDay';
import { nextCalendarDay } from './CalendarDay';
import type { DayState } from './DayState';
import type { GameState } from './GameState';
import { dayKindOf } from './Weekday';
import type { Weekday } from './Weekday';

// Moving from one day to the next: everything on GameState carries over, and `day` starts fresh.

export function newGameState(config: GameConfig): GameState {
  const weekday = config.calendar.startWeekday;
  return {
    calendar: { week: 1, weekday },
    production: initialProductionState(config),
    day: startDay(weekday, config),
  };
}

/**
 * Ends today (call once a work day has ended) and starts tomorrow. Bees sleep at night, so
 * nothing fills overnight. On non-work days they forage for a configured number of work days'
 * worth of filling, independent of how long the day's clock runs.
 */
export function finishDay(game: GameState, config: GameConfig): GameState {
  const { day } = game;
  let { production } = game;
  if (day.kind !== 'work') {
    const growthSeconds = config.time.workDaySeconds * config.hive.offDayGrowthWorkDays[day.kind];
    production = fillHive(production, growthSeconds, config);
  }
  const calendar = nextCalendarDay(game.calendar);
  return { ...game, calendar, production, day: startDay(calendar.weekday, config) };
}

function startDay(weekday: Weekday, config: GameConfig): DayState {
  const kind = dayKindOf(weekday);
  switch (kind) {
    case 'work':
      return { kind, workDay: WorkDay.initialState(config) };
    case 'market':
    case 'supply':
      return { kind };
  }
}
