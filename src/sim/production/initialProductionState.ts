import type { GameConfig } from '../../config/GameConfig';
import type { ProductionState } from './ProductionState';

/** A new game: the starting kit (ROADMAP §4.8). */
export function initialProductionState(config: GameConfig): ProductionState {
  return {
    honeySuper: { location: 'hive', fill: config.startingKit.superFill },
    extractor: { loadedFrames: 0, honeyOz: 0 },
    bucketHoneyOz: 0,
    tankHoneyOz: 0,
    inventory: {
      emptyBears: config.startingKit.emptyBears,
      labels: config.startingKit.labels,
      filledBears: 0,
    },
  };
}
