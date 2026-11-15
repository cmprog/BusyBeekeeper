import { fail, stationStateOf, succeed, withStationState } from '../productionAccess';
import type { StationBehavior } from '../StationBehavior';

/** Fills and labels as many bears as possible, up to the batch size. */
export const bottlingTankBehavior: StationBehavior<'bottlingTank'> = {
  initialState: () => ({ type: 'bottlingTank', honeyOz: 0 }),

  resolve({ station, state, config }) {
    if (state.carriedSuperId !== null) {
      return fail('handsFull');
    }
    const tank = stationStateOf(state, station.id, 'bottlingTank');
    const { emptyBears, labels } = state.inventory;
    const bearsOfHoney = Math.floor(tank.honeyOz / config.containers.bearHoneyOz);
    const bears = Math.min(config.equipment.bottlingBatchBears, bearsOfHoney, emptyBears, labels);
    if (bears > 0) {
      return succeed({ kind: 'bottle', bears }, bears * config.tasks.bottleSecondsPerBear);
    }

    if (bearsOfHoney === 0) {
      return fail('notEnoughHoney');
    }
    return fail(emptyBears === 0 ? 'outOfBears' : 'outOfLabels');
  },

  apply(action, { station, state, config }) {
    const tank = stationStateOf(state, station.id, 'bottlingTank');
    const { emptyBears, labels, filledBears } = state.inventory;
    const bottled = withStationState(state, station.id, {
      ...tank,
      honeyOz: tank.honeyOz - action.bears * config.containers.bearHoneyOz,
    });
    return {
      ...bottled,
      inventory: {
        emptyBears: emptyBears - action.bears,
        labels: labels - action.bears,
        filledBears: filledBears + action.bears,
      },
    };
  },
};
