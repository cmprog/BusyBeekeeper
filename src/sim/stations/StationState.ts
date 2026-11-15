import type { SuperId } from '../production/HoneySuper';
import type { StationType } from './StationType';

export interface HiveState {
  readonly type: 'hive';
  readonly superId: SuperId | null;
}

export interface UncappingTableState {
  readonly type: 'uncappingTable';
  readonly superId: SuperId | null;
}

export interface ExtractorState {
  readonly type: 'extractor';
  /** The super the loaded frames came from (and go back to). Null when empty. */
  readonly loadedSuperId: SuperId | null;
  readonly loadedFrames: number;
  readonly honeyOz: number;
}

export interface BucketState {
  readonly type: 'bucket';
  readonly honeyOz: number;
}

export interface BottlingTankState {
  readonly type: 'bottlingTank';
  readonly honeyOz: number;
}

/** The changing state of each station type. Plain data, so it can be saved. */
export interface StationStateByType {
  readonly hive: HiveState;
  readonly uncappingTable: UncappingTableState;
  readonly extractor: ExtractorState;
  readonly bucket: BucketState;
  readonly bottlingTank: BottlingTankState;
}

export type StationState = StationStateByType[StationType];
