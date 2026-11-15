import type { GameConfig } from '../../config/GameConfig';
import { stationBehaviors } from '../stations/stationBehaviors';
import type { StationId } from '../stations/Station';
import type { StationState } from '../stations/StationState';
import type { ProductionState } from './ProductionState';

/** The starting kit's super (ROADMAP §4.8). */
export const STARTING_SUPER_ID = 'super-1';

/** A new game: every station in its initial state, and the starting kit's super on the first hive. */
export function initialProductionState(config: GameConfig): ProductionState {
  const layout = config.backyard.stations;
  const stations: Record<StationId, StationState> = {};
  for (const station of layout) {
    stations[station.id] = stationBehaviors[station.type].initialState();
  }

  const firstHive = layout.find((station) => station.type === 'hive');
  if (!firstHive) {
    throw new Error('The backyard layout needs a hive for the starting super');
  }
  stations[firstHive.id] = { type: 'hive', superId: STARTING_SUPER_ID };

  return {
    supers: {
      [STARTING_SUPER_ID]: {
        id: STARTING_SUPER_ID,
        contents: { stage: 'filling', fill: config.startingKit.superFill },
      },
    },
    stations,
    carriedSuperId: null,
    inventory: {
      emptyBears: config.startingKit.emptyBears,
      labels: config.startingKit.labels,
      filledBears: 0,
    },
  };
}
