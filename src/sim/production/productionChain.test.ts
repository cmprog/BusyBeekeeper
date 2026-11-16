import { describe, expect, it } from 'vitest';
import type { GameConfig } from '../../config/GameConfig';
import { stationStateOf, withSuper } from '../stations/productionAccess';
import type { StationId } from '../stations/Station';
import { applyPlannedAction } from '../stations/stationBehaviors';
import { testConfig } from '../testing/testConfig';
import { WorkDay } from '../workday/WorkDay';
import type { WorkDayEvent } from '../workday/WorkDayEvent';
import { fillHive } from './fillHive';
import { initialProductionState, STARTING_SUPER_ID } from './initialProductionState';
import type { ProductionState } from './ProductionState';

// 8 frames × 32 oz = 256 oz in a full super; 12 oz bears → 21 bears with 4 oz left over.
const config: GameConfig = {
  ...testConfig,
  time: { ...testConfig.time, workDaySeconds: 10_000 },
  queue: { maxTasks: 100 },
};
const layout = config.backyard.stations;

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
  let total = state.inventory.filledBears * config.containers.bearHoneyOz;
  for (const { contents } of Object.values(state.supers)) {
    if (contents.stage === 'harvested') {
      total += (contents.frames.capped + contents.frames.uncapped) * contents.honeyPerFrameOz;
    }
  }
  for (const station of Object.values(state.stations)) {
    if (
      station.type === 'extractor' ||
      station.type === 'bucket' ||
      station.type === 'bottlingTank'
    ) {
      total += station.honeyOz;
    }
  }
  return total;
}

function withHiveFill(state: ProductionState, fill: number): ProductionState {
  return withSuper(state, { id: STARTING_SUPER_ID, contents: { stage: 'filling', fill } });
}

describe('production chain', () => {
  it('turns a full super into labelled bears without losing any honey', () => {
    const day = new WorkDay(config);
    const steps: StationId[] = [
      'hive', // pull
      'table', // set down
      ...Array<StationId>(8).fill('table'), // uncap each frame
      'extractor', // load 4
      'extractor', // extract
      'extractor', // load 4
      'extractor', // extract
      'bucket', // pour
      'tank', // 6 bears
      'tank', // 6 bears
      'tank', // 6 bears
      'tank', // 3 bears
    ];

    for (const stationId of steps) {
      expect(perform(day, stationId)?.type).toBe('taskCompleted');
      expect(totalHoneyOz(day.production)).toBe(256);
    }

    expect(perform(day, 'tank')).toMatchObject({ reason: 'notEnoughHoney' });
    expect(perform(day, 'table')).toMatchObject({ action: { kind: 'pickUpSuper' } });
    expect(perform(day, 'hive')).toMatchObject({ action: { kind: 'placeSuper' } });

    const production = day.production;
    expect(production.inventory).toEqual({ emptyBears: 3, labels: 3, filledBears: 21 });
    expect(stationStateOf(production, 'tank', 'bottlingTank').honeyOz).toBe(4);
    expect(stationStateOf(production, 'bucket', 'bucket').honeyOz).toBe(0);
    expect(stationStateOf(production, 'hive', 'hive').superId).toBe(STARTING_SUPER_ID);
    expect(day.state.stats).toEqual({
      tasksCompleted: steps.length + 2,
      tasksFailed: 1,
      framesUncapped: 8,
      honeyExtractedOz: 256,
      bearsBottled: 21,
    });
  });

  it('a partly full super gives proportionally less honey, rounded down', () => {
    const hive = layout.find((station) => station.id === 'hive');
    if (!hive) throw new Error('test layout needs a hive');
    const state = withHiveFill(initialProductionState(config), 0.55);

    const pulled = applyPlannedAction(
      { stationType: 'hive', action: { kind: 'pullSuper' } },
      { station: hive, state, layout, config },
    );

    // 0.55 × 32 oz = 17.6 oz per frame
    expect(pulled.supers[STARTING_SUPER_ID]?.contents).toMatchObject({
      stage: 'harvested',
      honeyPerFrameOz: 17,
    });
  });

  it('the hive fills only while the super is on it, and stops at full', () => {
    const empty = withHiveFill(initialProductionState(config), 0);
    const contentsAfter = (seconds: number): unknown =>
      fillHive(empty, seconds, config).supers[STARTING_SUPER_ID]?.contents;

    expect(contentsAfter(5)).toEqual({ stage: 'filling', fill: 0.25 });
    expect(contentsAfter(500)).toEqual({ stage: 'filling', fill: 1 });

    const day = new WorkDay(config, empty);
    perform(day, 'hive'); // pull
    const pulled = day.production;
    expect(fillHive(pulled, 5, config)).toBe(pulled);
  });

  it('stock carries over into the next day', () => {
    const day = new WorkDay(config);
    perform(day, 'hive');

    const nextDay = new WorkDay(config, day.production);

    expect(nextDay.production.carriedSuperId).toBe(STARTING_SUPER_ID);
    expect(nextDay.state.elapsedSeconds).toBe(0);
  });
});
