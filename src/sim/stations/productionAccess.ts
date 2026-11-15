import type { FrameCounts, HoneySuper, SuperId } from '../production/HoneySuper';
import type { ProductionState } from '../production/ProductionState';
import type { TaskFailure } from '../production/TaskFailure';
import type { Station, StationId } from './Station';
import type { StationStateByType } from './StationState';
import type { StationType } from './StationType';

// Small typed helpers shared by the station behaviours: look up state (throwing on a broken
// invariant, which is always a bug), build results, and write updated state back immutably.

export function invariant(condition: boolean, message: string): asserts condition {
  if (!condition) {
    throw new Error(`Invariant broken: ${message}`);
  }
}

export function succeed<A>(
  action: A,
  workSeconds: number,
): { readonly ok: true; readonly action: A; readonly workSeconds: number } {
  return { ok: true, action, workSeconds };
}

export function fail(reason: TaskFailure): { readonly ok: false; readonly reason: TaskFailure } {
  return { ok: false, reason };
}

export function stationStateOf<K extends StationType>(
  state: ProductionState,
  stationId: StationId,
  type: K,
): StationStateByType[K] {
  const stationState = state.stations[stationId];
  invariant(stationState?.type === type, `station '${stationId}' should be a ${type}`);
  // Narrowing a union by a generic key needs the cast; the check above guarantees it.
  return stationState as StationStateByType[K];
}

/** The first station of a type in the scene. Phase 1 scenes have exactly one of each. */
export function findStation(layout: readonly Station[], type: StationType): Station {
  const station = layout.find((candidate) => candidate.type === type);
  invariant(station !== undefined, `the scene should have a ${type}`);
  return station;
}

export function superOf(state: ProductionState, superId: SuperId): HoneySuper {
  const honeySuper = state.supers[superId];
  invariant(honeySuper !== undefined, `super '${superId}' should exist`);
  return honeySuper;
}

/** A super that is off the hive and being processed. */
export function harvestedSuperOf(
  state: ProductionState,
  superId: SuperId,
): { readonly frames: FrameCounts; readonly honeyPerFrameOz: number } {
  const { contents } = superOf(state, superId);
  invariant(contents.stage === 'harvested', `super '${superId}' should be harvested`);
  return contents;
}

export function withStationState(
  state: ProductionState,
  stationId: StationId,
  stationState: StationStateByType[StationType],
): ProductionState {
  return { ...state, stations: { ...state.stations, [stationId]: stationState } };
}

export function withSuper(state: ProductionState, honeySuper: HoneySuper): ProductionState {
  return { ...state, supers: { ...state.supers, [honeySuper.id]: honeySuper } };
}

/** Replaces a harvested super's frame counts. */
export function withFrames(
  state: ProductionState,
  superId: SuperId,
  frames: FrameCounts,
): ProductionState {
  const contents = harvestedSuperOf(state, superId);
  return withSuper(state, { id: superId, contents: { stage: 'harvested', ...contents, frames } });
}
