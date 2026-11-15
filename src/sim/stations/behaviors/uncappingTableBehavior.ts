import * as v from 'valibot';
import type { ProductionState } from '../../production/ProductionState';
import type { SuperId } from '../../production/HoneySuper';
import {
  fail,
  harvestedSuperOf,
  invariant,
  stationStateOf,
  succeed,
  withFrames,
  withStationState,
} from '../productionAccess';
import type { Station } from '../Station';
import type { StationBehavior } from '../StationBehavior';

/** Set the carried super down, uncap its frames one by one, then pick the empty super up. */
export const uncappingTableBehavior: StationBehavior<'uncappingTable'> = {
  initialState: () => ({ type: 'uncappingTable', superId: null }),
  stateSchema: v.object({ type: v.literal('uncappingTable'), superId: v.nullable(v.string()) }),

  resolve({ station, state, layout, config }) {
    const table = stationStateOf(state, station.id, 'uncappingTable');
    if (state.carriedSuperId !== null) {
      return table.superId === null
        ? succeed({ kind: 'setDownSuper' }, config.tasks.handleSuperSeconds)
        : fail('tableOccupied');
    }
    if (table.superId === null) {
      return fail('noSuperOnTable');
    }

    const { frames } = harvestedSuperOf(state, table.superId);
    if (frames.capped > 0) {
      return succeed({ kind: 'uncapFrame' }, config.tasks.uncapFrameSeconds);
    }
    // Every frame has been through the extractor and is back in the super: take it to the hive.
    if (frames.uncapped === 0 && !isInAnExtractor(table.superId, state, layout)) {
      return succeed({ kind: 'pickUpSuper' }, config.tasks.handleSuperSeconds);
    }
    return fail('nothingToUncap');
  },

  apply(action, { station, state }) {
    const table = stationStateOf(state, station.id, 'uncappingTable');

    switch (action.kind) {
      case 'setDownSuper': {
        const superId = state.carriedSuperId;
        invariant(superId !== null, 'setting down needs a carried super');
        return withStationState({ ...state, carriedSuperId: null }, station.id, {
          ...table,
          superId,
        });
      }
      case 'uncapFrame': {
        invariant(table.superId !== null, 'uncapping needs a super on the table');
        const { frames } = harvestedSuperOf(state, table.superId);
        return withFrames(state, table.superId, {
          ...frames,
          capped: frames.capped - 1,
          uncapped: frames.uncapped + 1,
        });
      }
      case 'pickUpSuper':
        invariant(table.superId !== null, 'picking up needs a super on the table');
        return withStationState({ ...state, carriedSuperId: table.superId }, station.id, {
          ...table,
          superId: null,
        });
    }
  },
};

function isInAnExtractor(
  superId: SuperId,
  state: ProductionState,
  layout: readonly Station[],
): boolean {
  return layout.some(
    (station) =>
      station.type === 'extractor' &&
      stationStateOf(state, station.id, 'extractor').loadedSuperId === superId,
  );
}
