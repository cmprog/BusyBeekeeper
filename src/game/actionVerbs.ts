import type { StationAction } from '../sim/stations/StationAction';

/** Short verb shown under the active task's icon while the beekeeper works. */
export const actionVerbs: Readonly<Record<StationAction['kind'], string>> = {
  pullSuper: 'Pull',
  placeSuper: 'Place',
  setDownSuper: 'Set down',
  uncapFrame: 'Uncap',
  pickUpSuper: 'Pick up',
  loadExtractor: 'Load',
  extract: 'Extract',
  pourBucket: 'Pour',
  bottle: 'Bottle',
};
