import type { GameConfig } from '../../config/GameConfig';
import type { ProductionState } from './ProductionState';

/** The bees fill the super while it is on the hive, up to full. Off the hive, nothing fills. */
export function fillHive(
  state: ProductionState,
  seconds: number,
  config: GameConfig,
): ProductionState {
  const honeySuper = state.honeySuper;
  if (honeySuper.location !== 'hive' || honeySuper.fill >= 1 || seconds === 0) {
    return state;
  }
  const fill = Math.min(1, honeySuper.fill + seconds / config.hive.superFillSeconds);
  return { ...state, honeySuper: { location: 'hive', fill } };
}
