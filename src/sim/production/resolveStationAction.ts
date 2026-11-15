import type { GameConfig } from '../../config/GameConfig';
import type { StationId } from '../StationId';
import type { ProductionState } from './ProductionState';
import type { StationAction } from './StationAction';
import type { TaskFailure } from './TaskFailure';

export type ActionResolution =
  | { readonly ok: true; readonly action: StationAction; readonly workSeconds: number }
  | { readonly ok: false; readonly reason: TaskFailure };

/**
 * Decides what a station's task does, given the state when the beekeeper arrives.
 * While carrying a super, the beekeeper's hands are full: only the hive and the uncapping table
 * (where supers go) can be used.
 */
export function resolveStationAction(
  stationId: StationId,
  state: ProductionState,
  config: GameConfig,
): ActionResolution {
  switch (stationId) {
    case 'hive':
      return resolveHive(state, config);
    case 'uncappingTable':
      return resolveUncappingTable(state, config);
    case 'extractor':
      return resolveExtractor(state, config);
    case 'bucket':
      return resolveBucket(state, config);
    case 'bottlingTank':
      return resolveBottlingTank(state, config);
  }
}

function succeed(action: StationAction, workSeconds: number): ActionResolution {
  return { ok: true, action, workSeconds };
}

function fail(reason: TaskFailure): ActionResolution {
  return { ok: false, reason };
}

function resolveHive(state: ProductionState, config: GameConfig): ActionResolution {
  switch (state.honeySuper.location) {
    case 'carried':
      // With one super, a carried super means the hive is empty, so it can always go back on.
      return succeed({ kind: 'placeSuper' }, config.tasks.handleSuperSeconds);
    case 'hive':
      return succeed({ kind: 'pullSuper' }, config.tasks.handleSuperSeconds);
    case 'uncappingTable':
      return fail('noSuperOnHive');
  }
}

function resolveUncappingTable(state: ProductionState, config: GameConfig): ActionResolution {
  const honeySuper = state.honeySuper;
  if (honeySuper.location === 'carried') {
    return succeed({ kind: 'setDownSuper' }, config.tasks.handleSuperSeconds);
  }
  if (honeySuper.location !== 'uncappingTable') {
    return fail('noSuperOnTable');
  }
  if (honeySuper.frames.capped > 0) {
    return succeed({ kind: 'uncapFrame' }, config.tasks.uncapFrameSeconds);
  }
  // Every frame has been through the extractor and is back in the super: take it to the hive.
  if (honeySuper.frames.uncapped === 0 && state.extractor.loadedFrames === 0) {
    return succeed({ kind: 'pickUpSuper' }, config.tasks.handleSuperSeconds);
  }
  return fail('nothingToUncap');
}

/** Loads every uncapped frame that fits; if none can be loaded, cranks what is loaded. */
function resolveExtractor(state: ProductionState, config: GameConfig): ActionResolution {
  const honeySuper = state.honeySuper;
  if (honeySuper.location === 'carried') {
    return fail('handsFull');
  }

  const uncapped = honeySuper.location === 'uncappingTable' ? honeySuper.frames.uncapped : 0;
  const room = config.equipment.extractorFrames - state.extractor.loadedFrames;
  const frames = Math.min(uncapped, room);
  if (frames > 0) {
    return succeed(
      { kind: 'loadExtractor', frames },
      frames * config.tasks.loadExtractorSecondsPerFrame,
    );
  }

  if (state.extractor.loadedFrames > 0) {
    if (state.bucketHoneyOz + state.extractor.honeyOz > config.equipment.bucketCapacityOz) {
      return fail('bucketFull');
    }
    return succeed({ kind: 'extract' }, config.tasks.extractSeconds);
  }
  return fail('nothingToExtract');
}

/** Pours as much of the bucket as the bottling tank has room for. */
function resolveBucket(state: ProductionState, config: GameConfig): ActionResolution {
  if (state.honeySuper.location === 'carried') {
    return fail('handsFull');
  }
  if (state.bucketHoneyOz === 0) {
    return fail('bucketEmpty');
  }
  const room = config.equipment.bottlingTankCapacityOz - state.tankHoneyOz;
  if (room <= 0) {
    return fail('tankFull');
  }
  return succeed(
    { kind: 'pourBucket', honeyOz: Math.min(state.bucketHoneyOz, room) },
    config.tasks.pourBucketSeconds,
  );
}

/** Fills and labels as many bears as possible, up to the batch size. */
function resolveBottlingTank(state: ProductionState, config: GameConfig): ActionResolution {
  if (state.honeySuper.location === 'carried') {
    return fail('handsFull');
  }

  const { emptyBears, labels } = state.inventory;
  const bearsOfHoney = Math.floor(state.tankHoneyOz / config.containers.bearHoneyOz);
  const bears = Math.min(config.equipment.bottlingBatchBears, bearsOfHoney, emptyBears, labels);
  if (bears > 0) {
    return succeed({ kind: 'bottle', bears }, bears * config.tasks.bottleSecondsPerBear);
  }

  if (bearsOfHoney === 0) {
    return fail('notEnoughHoney');
  }
  return fail(emptyBears === 0 ? 'outOfBears' : 'outOfLabels');
}
