import * as v from 'valibot';
import { countSchema } from '../../save/schemaParts';
import { fail, findStation, stationStateOf, succeed, withStationState } from '../productionAccess';
import type { StationBehavior } from '../StationBehavior';

/** Pours as much of the bucket as the scene's bottling tank has room for. */
export const bucketBehavior: StationBehavior<'bucket'> = {
  initialState: () => ({ type: 'bucket', honeyOz: 0 }),
  stateSchema: v.object({ type: v.literal('bucket'), honeyOz: countSchema }),

  resolve({ station, state, layout, config }) {
    if (state.carriedSuperId !== null) {
      return fail('handsFull');
    }
    const bucket = stationStateOf(state, station.id, 'bucket');
    if (bucket.honeyOz === 0) {
      return fail('bucketEmpty');
    }
    const tankId = findStation(layout, 'bottlingTank').id;
    const tank = stationStateOf(state, tankId, 'bottlingTank');
    const room = config.equipment.bottlingTankCapacityOz - tank.honeyOz;
    if (room <= 0) {
      return fail('tankFull');
    }
    return succeed(
      { kind: 'pourBucket', tankId, honeyOz: Math.min(bucket.honeyOz, room) },
      config.tasks.pourBucketSeconds,
    );
  },

  apply(action, { station, state }) {
    const bucket = stationStateOf(state, station.id, 'bucket');
    const tank = stationStateOf(state, action.tankId, 'bottlingTank');
    const poured = withStationState(state, station.id, {
      ...bucket,
      honeyOz: bucket.honeyOz - action.honeyOz,
    });
    return withStationState(poured, action.tankId, {
      ...tank,
      honeyOz: tank.honeyOz + action.honeyOz,
    });
  },
};
