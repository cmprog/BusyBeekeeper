import { describe, expect, it } from 'vitest';
import type { GameConfig } from '../../config/GameConfig';
import { testConfig } from '../testing/testConfig';
import { WorkDay } from '../workday/WorkDay';
import { createSaveData } from './createSaveData';
import { isSaveCompatible } from './isSaveCompatible';
import { deserializeSave, serializeSave } from './saveCodec';

const config: GameConfig = { ...testConfig, queue: { maxTasks: 5 } };
const layout = config.backyard.stations;

/** A day partway through: super pulled and set down, one frame mid-uncap, more queued. */
function midDay(): WorkDay {
  const day = new WorkDay(config);
  day.enqueue('hive');
  day.enqueue('table');
  day.enqueue('table');
  day.enqueue('table');
  day.tick(12.5); // pull done (7 s), set down done (12 s), first uncap in progress
  return day;
}

describe('saving and resuming', () => {
  it('a saved mid-day game resumes with the same clock, queue, stock and stats', () => {
    const day = midDay();
    const loaded = deserializeSave(serializeSave(createSaveData(3, day.state)));
    if (!loaded.ok) throw new Error(loaded.message);

    const resumed = new WorkDay(config, loaded.data.workDay);

    // Everything matches except the task in progress, which restarts from its walk.
    expect({ ...resumed.state, activeTask: null }).toEqual({ ...day.state, activeTask: null });
    expect(loaded.data.dayNumber).toBe(3);
  });

  it('a task in progress is saved as restarting from its walk', () => {
    const day = midDay();
    expect(day.state.activeTask?.phase).toBe('working');

    const saved = createSaveData(1, day.state);

    expect(saved.workDay.activeTask).toEqual({
      phase: 'walking',
      task: day.state.activeTask?.task,
    });
  });

  it('a save fits the layout it was made with', () => {
    expect(isSaveCompatible(createSaveData(1, midDay().state), layout)).toBe(true);
  });

  it('a save made with a different station layout is not used', () => {
    const saved = createSaveData(1, midDay().state);
    const renamed = layout.map((station) =>
      station.id === 'tank' ? { ...station, id: 'big-tank' } : station,
    );
    const retyped = layout.map((station) =>
      station.id === 'bucket' ? { ...station, type: 'bottlingTank' as const } : station,
    );

    expect(isSaveCompatible(saved, renamed)).toBe(false);
    expect(isSaveCompatible(saved, retyped)).toBe(false);
    expect(isSaveCompatible(saved, layout.slice(1))).toBe(false);
  });
});
