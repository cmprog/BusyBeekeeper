import { describe, expect, it } from 'vitest';
import type { GameConfig } from '../../config/GameConfig';
import { defaultConfig } from '../../config/defaultConfig';
import { WorkDay } from './WorkDay';
import type { WorkDayState } from './WorkDayState';

// Round numbers so expected times are exact: hive is 5 units from the start (a 3-4-5 triangle)
// and the honey house is 3 units from the hive. At 1 unit/s, start → hive takes 5 s.
const config: GameConfig = {
  ...defaultConfig,
  time: { ...defaultConfig.time, workDaySeconds: 60 },
  beekeeper: { walkSpeedUnitsPerSecond: 1 },
  queue: { maxTasks: 3 },
  tasks: { placeholderWorkSeconds: 2 },
  backyard: {
    beekeeperStart: { x: 0, y: 0 },
    stations: { hive: { x: 3, y: 4 }, honeyHouse: { x: 0, y: 4 } },
  },
};

function activeTaskId(day: WorkDay): number | undefined {
  return day.state.activeTask?.task.id;
}

describe('WorkDay', () => {
  it('runs tasks in queue order, each taking walk time plus work time', () => {
    const day = new WorkDay(config);
    day.enqueue('hive'); // 5 s walk + 2 s work
    day.enqueue('honeyHouse'); // 3 s walk + 2 s work

    day.tick(6.5);
    expect(activeTaskId(day)).toBe(1);
    expect(day.state.activeTask?.phase).toBe('working');

    day.tick(0.5); // t = 7: first task done, second starts walking from the hive
    expect(activeTaskId(day)).toBe(2);
    expect(day.state.beekeeperPosition).toEqual({ x: 3, y: 4 });

    day.tick(5); // t = 12: second task done
    expect(day.state.activeTask).toBeNull();
    expect(day.state.beekeeperPosition).toEqual({ x: 0, y: 4 });
  });

  it('refuses a task when the queue is full, counting the active task', () => {
    const day = new WorkDay(config);
    day.enqueue('hive');
    day.enqueue('hive');
    day.enqueue('hive');

    expect(day.enqueue('hive')).toEqual({ ok: false, reason: 'queueFull' });
    expect(day.state.queuedTasks).toHaveLength(2);
  });

  it('cancelling a queued task removes only that task', () => {
    const day = new WorkDay(config);
    day.enqueue('hive');
    day.enqueue('honeyHouse');
    day.enqueue('hive');

    expect(day.cancel(2)).toBe(true);

    expect(activeTaskId(day)).toBe(1);
    expect(day.state.queuedTasks.map((task) => task.id)).toEqual([3]);
  });

  it('cancelling the active task stops the beekeeper in place and starts the next task', () => {
    const day = new WorkDay(config);
    day.enqueue('hive');
    day.enqueue('honeyHouse');
    day.tick(2.5); // halfway to the hive

    expect(day.cancel(1)).toBe(true);

    expect(day.state.beekeeperPosition).toEqual({ x: 1.5, y: 2 });
    expect(activeTaskId(day)).toBe(2);
    expect(day.state.activeTask?.phase).toBe('walking');
  });

  it('when time runs out, finishes the active task, drops the queue, and refuses new tasks', () => {
    const day = new WorkDay(config);
    day.tick(55); // 5 s left
    day.enqueue('hive'); // takes 7 s, so it runs past the end of the day
    day.enqueue('honeyHouse');

    day.tick(5);
    expect(day.state.status).toBe('windingDown');
    expect(day.state.queuedTasks).toEqual([]);
    expect(activeTaskId(day)).toBe(1);
    expect(day.enqueue('hive')).toEqual({ ok: false, reason: 'dayOver' });

    day.tick(2);
    expect(day.state.status).toBe('ended');
    expect(day.state.activeTask).toBeNull();
  });

  it('ending the day early follows the same rules and keeps the unused time', () => {
    const day = new WorkDay(config);
    day.enqueue('hive');
    day.enqueue('honeyHouse');
    day.tick(1);

    day.endDayEarly();
    expect(day.state.status).toBe('windingDown');
    expect(day.state.queuedTasks).toEqual([]);

    day.tick(6); // the active task finishes
    expect(day.state.status).toBe('ended');
    expect(day.remainingSeconds).toBe(59);
  });

  it('ends immediately when ended early with nothing active', () => {
    const day = new WorkDay(config);
    day.endDayEarly();
    expect(day.state.status).toBe('ended');
  });

  it('gives the same result for one large tick as for many small ticks', () => {
    // Banked time and offline catch-up depend on this (ROADMAP §3.3).
    function play(tickSizeSeconds: number, totalSeconds: number): WorkDayState {
      const day = new WorkDay({ ...config, time: { ...config.time, workDaySeconds: 15 } });
      day.enqueue('hive');
      day.enqueue('honeyHouse');
      day.enqueue('hive');
      for (let t = 0; t < totalSeconds; t += tickSizeSeconds) {
        day.tick(tickSizeSeconds);
      }
      return day.state;
    }

    for (const totalSeconds of [3, 9.5, 14, 20]) {
      const big = play(totalSeconds, totalSeconds);
      const small = play(0.25, totalSeconds);
      expect(small.status).toBe(big.status);
      expect(small.activeTask?.task.id).toBe(big.activeTask?.task.id);
      expect(small.activeTask?.phase).toBe(big.activeTask?.phase);
      expect(small.queuedTasks).toEqual(big.queuedTasks);
      expect(small.elapsedSeconds).toBeCloseTo(big.elapsedSeconds, 9);
      expect(small.beekeeperPosition.x).toBeCloseTo(big.beekeeperPosition.x, 9);
      expect(small.beekeeperPosition.y).toBeCloseTo(big.beekeeperPosition.y, 9);
    }
  });
});
