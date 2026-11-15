import type { Position } from '../Position';
import type { StationType } from './StationType';

export type StationId = string;

/**
 * One station placed in a scene, e.g. one of several hives in a yard. Part of the scene layout
 * (config), so it never changes during play; its changing state lives in `ProductionState`.
 */
export interface Station {
  /** Unique across the whole game, so tasks and saves can refer to it. */
  readonly id: StationId;
  readonly type: StationType;
  readonly position: Position;
}
