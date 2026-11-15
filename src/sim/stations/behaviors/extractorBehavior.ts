import {
  fail,
  findStation,
  harvestedSuperOf,
  invariant,
  stationStateOf,
  succeed,
  withFrames,
  withStationState,
} from '../productionAccess';
import type { StationBehavior } from '../StationBehavior';

/**
 * Loads every uncapped frame that fits (from the uncapping table's super); if none can be
 * loaded, cranks what is loaded. Honey drains into the scene's bucket.
 */
export const extractorBehavior: StationBehavior<'extractor'> = {
  initialState: () => ({ type: 'extractor', loadedSuperId: null, loadedFrames: 0, honeyOz: 0 }),

  resolve({ station, state, layout, config }) {
    if (state.carriedSuperId !== null) {
      return fail('handsFull');
    }
    const extractor = stationStateOf(state, station.id, 'extractor');

    const tableSuperId = stationStateOf(
      state,
      findStation(layout, 'uncappingTable').id,
      'uncappingTable',
    ).superId;
    // Loaded frames go back to their own super, so frames from another super can't be added.
    const canLoadFrom =
      tableSuperId !== null &&
      (extractor.loadedSuperId === null || extractor.loadedSuperId === tableSuperId);
    if (canLoadFrom) {
      const uncapped = harvestedSuperOf(state, tableSuperId).frames.uncapped;
      const frames = Math.min(uncapped, config.equipment.extractorFrames - extractor.loadedFrames);
      if (frames > 0) {
        return succeed(
          { kind: 'loadExtractor', superId: tableSuperId, frames },
          frames * config.tasks.loadExtractorSecondsPerFrame,
        );
      }
    }

    if (extractor.loadedFrames > 0) {
      const bucketId = findStation(layout, 'bucket').id;
      const bucket = stationStateOf(state, bucketId, 'bucket');
      if (bucket.honeyOz + extractor.honeyOz > config.equipment.bucketCapacityOz) {
        return fail('bucketFull');
      }
      return succeed({ kind: 'extract', bucketId }, config.tasks.extractSeconds);
    }
    return fail('nothingToExtract');
  },

  apply(action, { station, state }) {
    const extractor = stationStateOf(state, station.id, 'extractor');

    switch (action.kind) {
      case 'loadExtractor': {
        const { frames, honeyPerFrameOz } = harvestedSuperOf(state, action.superId);
        const unloaded = withFrames(state, action.superId, {
          ...frames,
          uncapped: frames.uncapped - action.frames,
        });
        return withStationState(unloaded, station.id, {
          ...extractor,
          loadedSuperId: action.superId,
          loadedFrames: extractor.loadedFrames + action.frames,
          honeyOz: extractor.honeyOz + action.frames * honeyPerFrameOz,
        });
      }
      case 'extract': {
        invariant(extractor.loadedSuperId !== null, 'extracting needs loaded frames');
        const { frames } = harvestedSuperOf(state, extractor.loadedSuperId);
        const bucket = stationStateOf(state, action.bucketId, 'bucket');
        const framesBack = withFrames(state, extractor.loadedSuperId, {
          ...frames,
          extracted: frames.extracted + extractor.loadedFrames,
        });
        const drained = withStationState(framesBack, action.bucketId, {
          ...bucket,
          honeyOz: bucket.honeyOz + extractor.honeyOz,
        });
        return withStationState(drained, station.id, extractorBehavior.initialState());
      }
    }
  },
};
