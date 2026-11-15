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
  startingKit: {
    emptyBears: 24,
    labels: 24,
    superFill: 1,
  },
  beekeeper: {
    walkSpeedUnitsPerSecond: 5,
  },
  queue: {
    maxTasks: 3,
  },
  hive: {
    superFillSeconds: 90,
  },
  shallowSuper: {
    frames: 8,
    honeyPerFrameOz: 32,
  },
  equipment: {
    extractorFrames: 4,
    bucketCapacityOz: 320,
    bottlingTankCapacityOz: 480,
    bottlingBatchBears: 6,
  },
  containers: {
    bearHoneyOz: 12,
  },
  tasks: {
    handleSuperSeconds: 1,
    uncapFrameSeconds: 2,
    loadExtractorSecondsPerFrame: 0.5,
    extractSeconds: 8,
    pourBucketSeconds: 3,
    bottleSecondsPerBear: 1,
  },
  backyard: {
    beekeeperStart: { x: 9, y: 6 },
    stations: [
      { id: 'backyard-hive-1', type: 'hive', position: { x: 5, y: 9 } },
      { id: 'uncapping-table', type: 'uncappingTable', position: { x: 12, y: 11 } },
      { id: 'extractor', type: 'extractor', position: { x: 19, y: 11 } },
      { id: 'bucket', type: 'bucket', position: { x: 19, y: 5.5 } },
      { id: 'bottling-tank', type: 'bottlingTank', position: { x: 26, y: 9 } },
    ],
  },
};
