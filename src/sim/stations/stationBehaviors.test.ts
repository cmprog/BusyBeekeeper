import { describe, expect, it } from 'vitest';
import { defaultConfig } from '../../config/defaultConfig';
import type { FrameCounts } from '../production/HoneySuper';
import { initialProductionState, STARTING_SUPER_ID } from '../production/initialProductionState';
import type { ProductionState } from '../production/ProductionState';
import { testConfig as config } from '../testing/testConfig';
import { withStationState, withSuper } from './productionAccess';
import type { StationId } from './Station';
import { resolveStationTask } from './stationBehaviors';
import type { TaskResolution } from './stationBehaviors';

const layout = config.backyard.stations;
const base = initialProductionState(config);

function resolveAt(stationId: StationId, state: ProductionState): TaskResolution {
  const station = layout.find((candidate) => candidate.id === stationId);
  if (!station) throw new Error(`test layout has no '${stationId}'`);
  return resolveStationTask({ station, state, layout, config });
}

/** The starting super, harvested and sitting on the uncapping table. */
function superOnTable(frames: Partial<FrameCounts>): ProductionState {
  const harvested = withSuper(base, {
    id: STARTING_SUPER_ID,
    contents: {
      stage: 'harvested',
      frames: { capped: 0, uncapped: 0, extracted: 0, ...frames },
      honeyPerFrameOz: 32,
    },
  });
  const offHive = withStationState(harvested, 'hive', { type: 'hive', superId: null });
  return withStationState(offHive, 'table', { type: 'uncappingTable', superId: STARTING_SUPER_ID });
}

function withTankAndInventory(
  tankHoneyOz: number,
  emptyBears: number,
  labels: number,
): ProductionState {
  const state = withStationState(base, 'tank', { type: 'bottlingTank', honeyOz: tankHoneyOz });
  return { ...state, inventory: { emptyBears, labels, filledBears: 0 } };
}

describe('station behaviours', () => {
  it('carrying a super leaves the hands too full for the honey house machines', () => {
    const carried: ProductionState = {
      ...withStationState(base, 'hive', { type: 'hive', superId: null }),
      carriedSuperId: STARTING_SUPER_ID,
    };
    for (const stationId of ['extractor', 'bucket', 'tank']) {
      expect(resolveAt(stationId, carried)).toEqual({ ok: false, reason: 'handsFull' });
    }
  });

  it('the extractor loads every uncapped frame that fits, taking longer for more frames', () => {
    const state = withStationState(superOnTable({ uncapped: 3, capped: 3 }), 'extractor', {
      type: 'extractor',
      loadedSuperId: STARTING_SUPER_ID,
      loadedFrames: 2,
      honeyOz: 64,
    });

    expect(resolveAt('extractor', state)).toEqual({
      ok: true,
      planned: {
        stationType: 'extractor',
        action: { kind: 'loadExtractor', superId: STARTING_SUPER_ID, frames: 2 },
      },
      workSeconds: 2 * config.tasks.loadExtractorSecondsPerFrame,
    });
  });

  it('cranking the extractor takes the same time however many frames are loaded', () => {
    for (const loadedFrames of [1, 4]) {
      const state = withStationState(superOnTable({ capped: 8 - loadedFrames }), 'extractor', {
        type: 'extractor',
        loadedSuperId: STARTING_SUPER_ID,
        loadedFrames,
        honeyOz: loadedFrames * 32,
      });
      expect(resolveAt('extractor', state)).toMatchObject({
        ok: true,
        planned: { action: { kind: 'extract' } },
        workSeconds: config.tasks.extractSeconds,
      });
    }
  });

  it('refuses to extract when the bucket would overflow', () => {
    const loaded = withStationState(superOnTable({ extracted: 4 }), 'extractor', {
      type: 'extractor',
      loadedSuperId: STARTING_SUPER_ID,
      loadedFrames: 4,
      honeyOz: 128,
    });
    const state = withStationState(loaded, 'bucket', {
      type: 'bucket',
      honeyOz: config.equipment.bucketCapacityOz - 100,
    });

    expect(resolveAt('extractor', state)).toEqual({ ok: false, reason: 'bucketFull' });
  });

  it('pours only as much of the bucket as the tank has room for', () => {
    const withBucket = withStationState(base, 'bucket', { type: 'bucket', honeyOz: 200 });
    const state = withStationState(withBucket, 'tank', {
      type: 'bottlingTank',
      honeyOz: config.equipment.bottlingTankCapacityOz - 50,
    });

    expect(resolveAt('bucket', state)).toMatchObject({
      ok: true,
      planned: { action: { kind: 'pourBucket', tankId: 'tank', honeyOz: 50 } },
    });
  });

  it.each([
    { limit: 'batch size', tankHoneyOz: 480, emptyBears: 24, labels: 24, bears: 6 },
    { limit: 'honey', tankHoneyOz: 40, emptyBears: 24, labels: 24, bears: 3 },
    { limit: 'empty bears', tankHoneyOz: 480, emptyBears: 2, labels: 24, bears: 2 },
    { limit: 'labels', tankHoneyOz: 480, emptyBears: 24, labels: 1, bears: 1 },
  ])('bottling is limited by $limit', ({ tankHoneyOz, emptyBears, labels, bears }) => {
    expect(resolveAt('tank', withTankAndInventory(tankHoneyOz, emptyBears, labels))).toEqual({
      ok: true,
      planned: { stationType: 'bottlingTank', action: { kind: 'bottle', bears } },
      workSeconds: bears * config.tasks.bottleSecondsPerBear,
    });
  });

  it.each([
    { reason: 'notEnoughHoney', tankHoneyOz: 11, emptyBears: 0, labels: 0 },
    { reason: 'outOfBears', tankHoneyOz: 480, emptyBears: 0, labels: 0 },
    { reason: 'outOfLabels', tankHoneyOz: 480, emptyBears: 5, labels: 0 },
  ])('bottling fails with $reason', ({ reason, tankHoneyOz, emptyBears, labels }) => {
    expect(resolveAt('tank', withTankAndInventory(tankHoneyOz, emptyBears, labels))).toEqual({
      ok: false,
      reason,
    });
  });

  it('the empty super can only be picked up once all its frames are back from the extractor', () => {
    const stillExtracting = withStationState(superOnTable({ extracted: 4 }), 'extractor', {
      type: 'extractor',
      loadedSuperId: STARTING_SUPER_ID,
      loadedFrames: 4,
      honeyOz: 128,
    });
    expect(resolveAt('table', stillExtracting)).toEqual({ ok: false, reason: 'nothingToUncap' });

    expect(resolveAt('table', superOnTable({ extracted: 8 }))).toMatchObject({
      ok: true,
      planned: { action: { kind: 'pickUpSuper' } },
    });
  });

  it('every station in the shipped layout has a unique id', () => {
    // Tasks and saves refer to stations by id, so a duplicate would silently alias two stations.
    const ids = defaultConfig.backyard.stations.map((station) => station.id);
    expect(new Set(ids).size).toBe(ids.length);
  });
});
