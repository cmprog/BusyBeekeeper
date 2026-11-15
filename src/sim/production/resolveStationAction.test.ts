import { describe, expect, it } from 'vitest';
import { testConfig as config } from '../testing/testConfig';
import { initialProductionState } from './initialProductionState';
import type { FrameCounts, ProductionState } from './ProductionState';
import { resolveStationAction } from './resolveStationAction';

const base = initialProductionState(config);

function superOnTable(frames: Partial<FrameCounts>): ProductionState['honeySuper'] {
  return {
    location: 'uncappingTable',
    frames: { capped: 0, uncapped: 0, extracted: 0, ...frames },
    honeyPerFrameOz: 32,
  };
}

const carried: ProductionState = {
  ...base,
  honeySuper: {
    location: 'carried',
    frames: { capped: 8, uncapped: 0, extracted: 0 },
    honeyPerFrameOz: 32,
  },
  bucketHoneyOz: 100,
  tankHoneyOz: 100,
};

describe('resolveStationAction', () => {
  it('carrying a super leaves the hands too full for the honey house machines', () => {
    for (const stationId of ['extractor', 'bucket', 'bottlingTank'] as const) {
      expect(resolveStationAction(stationId, carried, config)).toEqual({
        ok: false,
        reason: 'handsFull',
      });
    }
  });

  it('the extractor loads every uncapped frame that fits, taking longer for more frames', () => {
    const state: ProductionState = {
      ...base,
      honeySuper: superOnTable({ uncapped: 3, capped: 3 }),
      extractor: { loadedFrames: 2, honeyOz: 64 },
    };

    expect(resolveStationAction('extractor', state, config)).toEqual({
      ok: true,
      action: { kind: 'loadExtractor', frames: 2 },
      workSeconds: 2 * config.tasks.loadExtractorSecondsPerFrame,
    });
  });

  it('cranking the extractor takes the same time however many frames are loaded', () => {
    for (const loadedFrames of [1, 4]) {
      const state: ProductionState = {
        ...base,
        honeySuper: superOnTable({ capped: 8 - loadedFrames }),
        extractor: { loadedFrames, honeyOz: loadedFrames * 32 },
      };
      expect(resolveStationAction('extractor', state, config)).toEqual({
        ok: true,
        action: { kind: 'extract' },
        workSeconds: config.tasks.extractSeconds,
      });
    }
  });

  it('refuses to extract when the bucket would overflow', () => {
    const state: ProductionState = {
      ...base,
      honeySuper: superOnTable({ extracted: 4 }),
      extractor: { loadedFrames: 4, honeyOz: 128 },
      bucketHoneyOz: config.equipment.bucketCapacityOz - 100,
    };

    expect(resolveStationAction('extractor', state, config)).toEqual({
      ok: false,
      reason: 'bucketFull',
    });
  });

  it('pours only as much of the bucket as the tank has room for', () => {
    const state: ProductionState = {
      ...base,
      bucketHoneyOz: 200,
      tankHoneyOz: config.equipment.bottlingTankCapacityOz - 50,
    };

    expect(resolveStationAction('bucket', state, config)).toMatchObject({
      ok: true,
      action: { kind: 'pourBucket', honeyOz: 50 },
    });
  });

  it.each([
    { limit: 'batch size', tankHoneyOz: 480, emptyBears: 24, labels: 24, bears: 6 },
    { limit: 'honey', tankHoneyOz: 40, emptyBears: 24, labels: 24, bears: 3 },
    { limit: 'empty bears', tankHoneyOz: 480, emptyBears: 2, labels: 24, bears: 2 },
    { limit: 'labels', tankHoneyOz: 480, emptyBears: 24, labels: 1, bears: 1 },
  ])('bottling is limited by $limit', ({ tankHoneyOz, emptyBears, labels, bears }) => {
    const state: ProductionState = {
      ...base,
      tankHoneyOz,
      inventory: { emptyBears, labels, filledBears: 0 },
    };

    expect(resolveStationAction('bottlingTank', state, config)).toEqual({
      ok: true,
      action: { kind: 'bottle', bears },
      workSeconds: bears * config.tasks.bottleSecondsPerBear,
    });
  });

  it.each([
    { reason: 'notEnoughHoney', tankHoneyOz: 11, emptyBears: 0, labels: 0 },
    { reason: 'outOfBears', tankHoneyOz: 480, emptyBears: 0, labels: 0 },
    { reason: 'outOfLabels', tankHoneyOz: 480, emptyBears: 5, labels: 0 },
  ])('bottling fails with $reason', ({ reason, tankHoneyOz, emptyBears, labels }) => {
    const state: ProductionState = {
      ...base,
      tankHoneyOz,
      inventory: { emptyBears, labels, filledBears: 0 },
    };

    expect(resolveStationAction('bottlingTank', state, config)).toEqual({ ok: false, reason });
  });

  it('the empty super can only be picked up once all its frames are back from the extractor', () => {
    const stillExtracting: ProductionState = {
      ...base,
      honeySuper: superOnTable({ extracted: 4 }),
      extractor: { loadedFrames: 4, honeyOz: 128 },
    };
    expect(resolveStationAction('uncappingTable', stillExtracting, config)).toEqual({
      ok: false,
      reason: 'nothingToUncap',
    });

    const done: ProductionState = { ...base, honeySuper: superOnTable({ extracted: 8 }) };
    expect(resolveStationAction('uncappingTable', done, config)).toMatchObject({
      ok: true,
      action: { kind: 'pickUpSuper' },
    });
  });
});
