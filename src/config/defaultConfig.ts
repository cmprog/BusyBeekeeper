import type { GameConfig } from './GameConfig';

/** The shipped balance values. Tune here, not at call sites. */
export const defaultConfig: GameConfig = {
  time: {
    workDaySeconds: 120,
    marketDaySeconds: 120,
  },
  economy: {
    startingBeebucks: 100,
  },
};
