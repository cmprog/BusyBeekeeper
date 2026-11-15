import type { GameConfig } from '../../config/GameConfig';
import type { HoneySuper } from './HoneySuper';
import type { ProductionState } from './ProductionState';

/** The bees fill every super that is on a hive, up to full. Off a hive, nothing fills. */
export function fillHive(
  state: ProductionState,
  seconds: number,
  config: GameConfig,
): ProductionState {
  if (seconds === 0) {
    return state;
  }

  let supers: Record<string, HoneySuper> | undefined;
  for (const stationState of Object.values(state.stations)) {
    if (stationState.type !== 'hive' || stationState.superId === null) {
      continue;
    }
    const honeySuper = state.supers[stationState.superId];
    if (honeySuper?.contents.stage !== 'filling' || honeySuper.contents.fill >= 1) {
      continue;
    }
    const fill = Math.min(1, honeySuper.contents.fill + seconds / config.hive.superFillSeconds);
    supers ??= { ...state.supers };
    supers[honeySuper.id] = { id: honeySuper.id, contents: { stage: 'filling', fill } };
  }
  // Unchanged state keeps its identity, so callers can cheaply tell nothing filled.
  return supers ? { ...state, supers } : state;
}
