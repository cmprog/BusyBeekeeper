import type { GameConfig } from '../../config/GameConfig';
import type { HoneySuperState, ProductionState } from './ProductionState';
import type { StationAction } from './StationAction';

/** Far below an ounce, far above accumulated float error. */
const FILL_TOLERANCE_OZ = 1e-6;

type OffHiveSuper = Extract<HoneySuperState, { location: 'carried' | 'uncappingTable' }>;

/**
 * Applies a completed action. The action was resolved against this same state on arrival (only
 * the beekeeper changes it in between), so a broken precondition here is a bug and throws.
 */
export function applyStationAction(
  action: StationAction,
  state: ProductionState,
  config: GameConfig,
): ProductionState {
  switch (action.kind) {
    case 'pullSuper': {
      const honeySuper = state.honeySuper;
      if (honeySuper.location !== 'hive') {
        throw invariantError(action);
      }
      return {
        ...state,
        honeySuper: {
          location: 'carried',
          frames: { capped: config.shallowSuper.frames, uncapped: 0, extracted: 0 },
          // A partly full super gives proportionally less honey, rounded down to whole ounces.
          // `fill` builds up from many float additions, so 0.75 can arrive as 0.7499999…; the
          // tolerance stops that rounding down a whole ounce and keeps results tick-size independent.
          honeyPerFrameOz: Math.floor(
            honeySuper.fill * config.shallowSuper.honeyPerFrameOz + FILL_TOLERANCE_OZ,
          ),
        },
      };
    }
    case 'placeSuper': {
      const honeySuper = offHiveSuper(state, 'carried', action);
      // A super put back without being processed keeps its honey.
      const fill =
        honeySuper.frames.capped > 0
          ? honeySuper.honeyPerFrameOz / config.shallowSuper.honeyPerFrameOz
          : 0;
      return { ...state, honeySuper: { location: 'hive', fill } };
    }
    case 'setDownSuper':
      return {
        ...state,
        honeySuper: { ...offHiveSuper(state, 'carried', action), location: 'uncappingTable' },
      };
    case 'pickUpSuper':
      return {
        ...state,
        honeySuper: { ...offHiveSuper(state, 'uncappingTable', action), location: 'carried' },
      };
    case 'uncapFrame': {
      const honeySuper = offHiveSuper(state, 'uncappingTable', action);
      const { capped, uncapped } = honeySuper.frames;
      return {
        ...state,
        honeySuper: {
          ...honeySuper,
          frames: { ...honeySuper.frames, capped: capped - 1, uncapped: uncapped + 1 },
        },
      };
    }
    case 'loadExtractor': {
      const honeySuper = offHiveSuper(state, 'uncappingTable', action);
      return {
        ...state,
        honeySuper: {
          ...honeySuper,
          frames: { ...honeySuper.frames, uncapped: honeySuper.frames.uncapped - action.frames },
        },
        extractor: {
          loadedFrames: state.extractor.loadedFrames + action.frames,
          honeyOz: state.extractor.honeyOz + action.frames * honeySuper.honeyPerFrameOz,
        },
      };
    }
    case 'extract': {
      const honeySuper = offHiveSuper(state, 'uncappingTable', action);
      return {
        ...state,
        honeySuper: {
          ...honeySuper,
          frames: {
            ...honeySuper.frames,
            extracted: honeySuper.frames.extracted + state.extractor.loadedFrames,
          },
        },
        extractor: { loadedFrames: 0, honeyOz: 0 },
        bucketHoneyOz: state.bucketHoneyOz + state.extractor.honeyOz,
      };
    }
    case 'pourBucket':
      return {
        ...state,
        bucketHoneyOz: state.bucketHoneyOz - action.honeyOz,
        tankHoneyOz: state.tankHoneyOz + action.honeyOz,
      };
    case 'bottle': {
      const { emptyBears, labels, filledBears } = state.inventory;
      return {
        ...state,
        tankHoneyOz: state.tankHoneyOz - action.bears * config.containers.bearHoneyOz,
        inventory: {
          emptyBears: emptyBears - action.bears,
          labels: labels - action.bears,
          filledBears: filledBears + action.bears,
        },
      };
    }
  }
}

function offHiveSuper(
  state: ProductionState,
  location: OffHiveSuper['location'],
  action: StationAction,
): OffHiveSuper {
  const honeySuper = state.honeySuper;
  if (honeySuper.location !== location) {
    throw invariantError(action);
  }
  return honeySuper;
}

function invariantError(action: StationAction): Error {
  return new Error(`Cannot apply '${action.kind}': the super is not where it was on arrival`);
}
