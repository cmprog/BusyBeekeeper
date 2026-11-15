import type { SuperId } from '../production/HoneySuper';
import type { StationId } from './Station';
import type { StationType } from './StationType';

/**
 * What the beekeeper can do at each station type, decided on arrival. Counts and the other
 * stations involved are fixed at that moment, so the work time and the result always match.
 * Plain data, so a task in progress can be saved.
 */
export interface StationActionByType {
  readonly hive: { readonly kind: 'pullSuper' } | { readonly kind: 'placeSuper' };
  readonly uncappingTable:
    | { readonly kind: 'setDownSuper' }
    | { readonly kind: 'uncapFrame' }
    | { readonly kind: 'pickUpSuper' };
  readonly extractor:
    | { readonly kind: 'loadExtractor'; readonly superId: SuperId; readonly frames: number }
    | { readonly kind: 'extract'; readonly bucketId: StationId };
  readonly bucket: {
    readonly kind: 'pourBucket';
    readonly tankId: StationId;
    readonly honeyOz: number;
  };
  readonly bottlingTank: { readonly kind: 'bottle'; readonly bears: number };
}

export type StationAction = StationActionByType[StationType];

/**
 * An action paired with its station type, kept correlated so it can be handed back to the right
 * behaviour on completion. `PlannedAction<'hive'>` only allows hive actions.
 */
export type PlannedAction<K extends StationType = StationType> = {
  [P in K]: { readonly stationType: P; readonly action: StationActionByType[P] };
}[K];
