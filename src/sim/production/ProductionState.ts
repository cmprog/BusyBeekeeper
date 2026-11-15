import type { StationId } from '../stations/Station';
import type { StationState } from '../stations/StationState';
import type { HoneySuper, SuperId } from './HoneySuper';

/**
 * The beekeeping operation's stock and equipment (ROADMAP §4.1). Plain readonly data that
 * outlives a single day.
 */
export interface ProductionState {
  readonly supers: Readonly<Record<SuperId, HoneySuper>>;
  /** One entry per station in the layout, keyed by station id. */
  readonly stations: Readonly<Record<StationId, StationState>>;
  /** The super in the beekeeper's hands. Their hands are full while carrying one. */
  readonly carriedSuperId: SuperId | null;
  readonly inventory: {
    readonly emptyBears: number;
    readonly labels: number;
    readonly filledBears: number;
  };
}
