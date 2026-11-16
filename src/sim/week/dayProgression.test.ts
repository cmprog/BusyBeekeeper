import { describe, expect, it } from 'vitest';
import type { GameConfig } from '../../config/GameConfig';
import { fillHive } from '../production/fillHive';
import { superOf } from '../stations/productionAccess';
import { testConfig } from '../testing/testConfig';
import type { CalendarDay } from './CalendarDay';
import { finishDay, newGameState } from './dayProgression';
import type { GameState } from './GameState';

// testConfig: a 60 s work day and a 20 s super fill, so one work day fills a super.
const config: GameConfig = {
  ...testConfig,
  startingKit: { ...testConfig.startingKit, superFill: 0 },
  hive: { ...testConfig.hive, superFillSeconds: 240 },
};

/** The starting super's fill (it starts on the hive). */
function fillOf(game: GameState): number {
  const { contents } = superOf(game.production, 'super-1');
  return contents.stage === 'filling' ? contents.fill : Number.NaN;
}

describe('the week', () => {
  it('starts on Thursday of week 1 and runs Fri, market Sat, supply Sun, then work again', () => {
    const days: (CalendarDay & { kind: string })[] = [];
    let game = newGameState(config);
    for (let i = 0; i < 5; i++) {
      days.push({ ...game.calendar, kind: game.day.kind });
      game = finishDay(game, config);
    }

    expect(days).toEqual([
      { week: 1, weekday: 'thursday', kind: 'work' },
      { week: 1, weekday: 'friday', kind: 'work' },
      { week: 1, weekday: 'saturday', kind: 'market' },
      { week: 1, weekday: 'sunday', kind: 'supply' },
      { week: 2, weekday: 'monday', kind: 'work' },
    ]);
  });

  it('bees fill nothing overnight', () => {
    const thursday = newGameState(config);

    expect(fillOf(finishDay(thursday, config))).toBe(fillOf(thursday));
  });

  it('on Saturday bees fill as much as one full work day, however long the market runs', () => {
    const friday = finishDay(newGameState(config), config);
    const saturday = finishDay(friday, config);
    const oneWorkDay = fillHive(saturday.production, config.time.workDaySeconds, config);
    const longMarket = { ...config, time: { ...config.time, marketDaySeconds: 600 } };

    const sunday = finishDay(saturday, longMarket);

    expect(sunday.production).toEqual(oneWorkDay);
    expect(fillOf(sunday)).toBeCloseTo(0.25);
  });

  it('on Sunday bees fill only what the config allows (none by default)', () => {
    let sunday = newGameState(config);
    while (sunday.calendar.weekday !== 'sunday') {
      sunday = finishDay(sunday, config);
    }
    const busySundays = {
      ...config,
      hive: { ...config.hive, offDayGrowthWorkDays: { market: 1, supply: 0.5 } },
    };

    expect(fillOf(finishDay(sunday, config))).toBe(fillOf(sunday));
    expect(fillOf(finishDay(sunday, busySundays))).toBeCloseTo(fillOf(sunday) + 0.125);
  });
});
