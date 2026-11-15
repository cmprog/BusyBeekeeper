import * as v from 'valibot';
import {
  fail,
  harvestedSuperOf,
  invariant,
  stationStateOf,
  succeed,
  superOf,
  withStationState,
  withSuper,
} from '../productionAccess';
import type { StationBehavior } from '../StationBehavior';

/** Fill tolerance when rounding pulled honey down: far below an ounce, far above float error. */
const FILL_TOLERANCE_OZ = 1e-6;

/** Pull the super to harvest it, or put the carried super back on. */
export const hiveBehavior: StationBehavior<'hive'> = {
  initialState: () => ({ type: 'hive', superId: null }),
  stateSchema: v.object({ type: v.literal('hive'), superId: v.nullable(v.string()) }),

  resolve({ station, state, config }) {
    const hive = stationStateOf(state, station.id, 'hive');
    if (state.carriedSuperId !== null) {
      return hive.superId === null
        ? succeed({ kind: 'placeSuper' }, config.tasks.handleSuperSeconds)
        : fail('hiveHasSuper');
    }
    return hive.superId === null
      ? fail('noSuperOnHive')
      : succeed({ kind: 'pullSuper' }, config.tasks.handleSuperSeconds);
  },

  apply(action, { station, state, config }) {
    const hive = stationStateOf(state, station.id, 'hive');
    const { frames, honeyPerFrameOz: fullFrameOz } = config.shallowSuper;

    switch (action.kind) {
      case 'pullSuper': {
        invariant(hive.superId !== null, 'pulling needs a super on the hive');
        const { contents } = superOf(state, hive.superId);
        invariant(contents.stage === 'filling', 'a super on a hive is filling');
        const pulled = withSuper(state, {
          id: hive.superId,
          contents: {
            stage: 'harvested',
            frames: { capped: frames, uncapped: 0, extracted: 0 },
            // A partly full super gives proportionally less honey, rounded down to whole
            // ounces. `fill` builds up from many float additions, so 0.75 can arrive as
            // 0.7499999…; the tolerance stops that losing an ounce and keeps results
            // independent of tick size.
            honeyPerFrameOz: Math.floor(contents.fill * fullFrameOz + FILL_TOLERANCE_OZ),
          },
        });
        return withStationState({ ...pulled, carriedSuperId: hive.superId }, station.id, {
          ...hive,
          superId: null,
        });
      }
      case 'placeSuper': {
        const superId = state.carriedSuperId;
        invariant(superId !== null, 'placing needs a carried super');
        const carried = harvestedSuperOf(state, superId);
        // A super put back without being processed keeps its honey.
        const fill = carried.frames.capped > 0 ? carried.honeyPerFrameOz / fullFrameOz : 0;
        const placed = withSuper(state, { id: superId, contents: { stage: 'filling', fill } });
        return withStationState({ ...placed, carriedSuperId: null }, station.id, {
          ...hive,
          superId,
        });
      }
    }
  },
};
