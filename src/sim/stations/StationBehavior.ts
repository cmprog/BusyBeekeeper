import type { GameConfig } from '../../config/GameConfig';
import type { ProductionState } from '../production/ProductionState';
import type { TaskFailure } from '../production/TaskFailure';
import type { Station } from './Station';
import type { StationActionByType } from './StationAction';
import type { StationStateByType } from './StationState';
import type { StationType } from './StationType';

/** Everything a behaviour can see when deciding or applying an action. */
export interface StationContext {
  /** The station the beekeeper is at. */
  readonly station: Station;
  readonly state: ProductionState;
  /** Every station in the current scene, for actions that involve another station. */
  readonly layout: readonly Station[];
  readonly config: GameConfig;
}

export type BehaviorResolution<A> =
  | { readonly ok: true; readonly action: A; readonly workSeconds: number }
  | { readonly ok: false; readonly reason: TaskFailure };

/**
 * The rules for one station type. Stateless: it works on the plain `ProductionState`, which is
 * what gets saved. Each station type has one in stations/behaviors/, registered in
 * stationBehaviors.ts.
 */
export interface StationBehavior<K extends StationType> {
  /** State for a newly placed station of this type. */
  initialState(): StationStateByType[K];
  /** On arrival: what to do and how long it takes, or why it can't be done. */
  resolve(context: StationContext): BehaviorResolution<StationActionByType[K]>;
  /**
   * On completion: the new state. The action was resolved against this same state (only the
   * beekeeper changes it in between), so a broken precondition here is a bug and throws.
   */
  apply(action: StationActionByType[K], context: StationContext): ProductionState;
}
