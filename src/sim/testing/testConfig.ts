import type { GameConfig } from '../../config/GameConfig';
import { defaultConfig } from '../../config/defaultConfig';

/**
 * Config for sim tests, with round numbers so expected times are exact. At 1 unit/s: start →
 * hive is 5 s (a 3-4-5 triangle), hive → uncapping table 3 s, then 4 s between each of
 * uncapping table → extractor → bucket → bottling tank.
 */
export const testConfig: GameConfig = {
  ...defaultConfig,
  time: { ...defaultConfig.time, workDaySeconds: 60 },
  beekeeper: { walkSpeedUnitsPerSecond: 1 },
  queue: { maxTasks: 3 },
  hive: { superFillSeconds: 20 },
  tasks: {
    handleSuperSeconds: 2,
    uncapFrameSeconds: 2,
    loadExtractorSecondsPerFrame: 0.5,
    extractSeconds: 8,
    pourBucketSeconds: 3,
    bottleSecondsPerBear: 1,
  },
  backyard: {
    beekeeperStart: { x: 0, y: 0 },
    stations: [
      { id: 'hive', type: 'hive', position: { x: 3, y: 4 } },
      { id: 'table', type: 'uncappingTable', position: { x: 0, y: 4 } },
      { id: 'extractor', type: 'extractor', position: { x: 0, y: 8 } },
      { id: 'bucket', type: 'bucket', position: { x: 0, y: 12 } },
      { id: 'tank', type: 'bottlingTank', position: { x: 0, y: 16 } },
    ],
  },
};
