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
  beekeeper: {
    walkSpeedUnitsPerSecond: 5,
  },
  queue: {
    maxTasks: 3,
  },
  tasks: {
    placeholderWorkSeconds: 3,
  },
  backyard: {
    beekeeperStart: { x: 16, y: 9 },
    stations: {
      hive: { x: 6, y: 9 },
      honeyHouse: { x: 26, y: 9 },
    },
  },
};
