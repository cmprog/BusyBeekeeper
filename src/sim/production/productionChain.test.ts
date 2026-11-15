import { describe, expect, it } from 'vitest';
import type { GameConfig } from '../../config/GameConfig';
import type { StationId } from '../StationId';
import { testConfig } from '../testing/testConfig';
import { WorkDay } from '../workday/WorkDay';
import type { WorkDayEvent } from '../workday/WorkDayEvent';
import { applyStationAction } from './applyStationAction';
import { fillHive } from './fillHive';
import { initialProductionState } from './initialProductionState';
import type { ProductionState } from './ProductionState';

// 8 frames × 32 oz = 256 oz in a full super; 12 oz bears → 21 bears with 4 oz left over.
const config: GameConfig = {
  ...testConfig,
  time: { ...testConfig.time, workDaySeconds: 10_000 },
  queue: { maxTasks: 100 },
};

/** Queues one task and runs until the beekeeper is idle. Returns its event. */
function perform(day: WorkDay, stationId: StationId): WorkDayEvent | undefined {
  day.enqueue(stationId);
  const events: WorkDayEvent[] = [];
  while (day.state.activeTask) {
    events.push(...day.tick(1));
  }
  return events[0];
}

/** Every ounce in the operation, wherever it currently is. */
function totalHoneyOz(state: ProductionState): number {
  const honeySuper = state.honeySuper;
  const inSuper =
    honeySuper.location === 'hive'
      ? 0
      : (honeySuper.frames.capped + honeySuper.frames.uncapped) * honeySuper.honeyPerFrameOz;
  return (
    inSuper +
    state.extractor.honeyOz +
    state.bucketHoneyOz +
    state.tankHoneyOz +
    state.inventory.filledBears * config.containers.bearHoneyOz
  );
}

describe('production chain', () => {
  it('turns a full super into labelled bears without losing any honey', () => {
    const day = new WorkDay(config);
    const steps: StationId[] = [
      'hive', // pull
      'uncappingTable', // set down
      ...Array<StationId>(8).fill('uncappingTable'), // uncap each frame
      'extractor', // load 4
      'extractor', // extract
      'extractor', // load 4
      'extractor', // extract
      'bucket', // pour
      'bottlingTank', // 6 bears
      'bottlingTank', // 6 bears
      'bottlingTank', // 6 bears
      'bottlingTank', // 3 bears
    ];

    for (const stationId of steps) {
      expect(perform(day, stationId)?.type).toBe('taskCompleted');
      expect(totalHoneyOz(day.state.production)).toBe(256);
    }

    expect(perform(day, 'bottlingTank')).toMatchObject({ reason: 'notEnoughHoney' });
    expect(perform(day, 'uncappingTable')).toMatchObject({ action: { kind: 'pickUpSuper' } });
    expect(perform(day, 'hive')).toMatchObject({ action: { kind: 'placeSuper' } });

    const production = day.state.production;
    expect(production.inventory).toEqual({ emptyBears: 3, labels: 3, filledBears: 21 });
    expect(production.tankHoneyOz).toBe(4);
    expect(production.bucketHoneyOz).toBe(0);
    expect(production.honeySuper.location).toBe('hive');
  });

  it('a partly full super gives proportionally less honey, rounded down', () => {
    const state: ProductionState = {
      ...initialProductionState(config),
      honeySuper: { location: 'hive', fill: 0.55 },
    };

    const pulled = applyStationAction({ kind: 'pullSuper' }, state, config);

    // 0.55 × 32 oz = 17.6 oz per frame
    expect(pulled.honeySuper).toMatchObject({ location: 'carried', honeyPerFrameOz: 17 });
  });

  it('the hive fills only while the super is on it, and stops at full', () => {
    const empty: ProductionState = {
      ...initialProductionState(config),
      honeySuper: { location: 'hive', fill: 0 },
    };
    expect(fillHive(empty, 5, config).honeySuper).toEqual({ location: 'hive', fill: 0.25 });
    expect(fillHive(empty, 500, config).honeySuper).toEqual({ location: 'hive', fill: 1 });

    const pulled = applyStationAction({ kind: 'pullSuper' }, empty, config);
    expect(fillHive(pulled, 5, config)).toBe(pulled);
  });

  it('stock carries over into the next day', () => {
    const day = new WorkDay(config);
    perform(day, 'hive');

    const nextDay = new WorkDay(config, WorkDay.initialState(config, day.state.production));

    expect(nextDay.state.production.honeySuper.location).toBe('carried');
    expect(nextDay.state.elapsedSeconds).toBe(0);
  });
});
