import { describe, expect, it } from 'vitest';
import type { GameConfig } from '../../config/GameConfig';
import { testConfig } from '../testing/testConfig';
import { finishDay, newGameState } from '../week/dayProgression';
import type { GameState } from '../week/GameState';
import { WorkDay } from '../workday/WorkDay';
import { createSaveData, gameStateFromSave } from './createSaveData';
import { isSaveCompatible } from './isSaveCompatible';
import { deserializeSave, serializeSave } from './saveCodec';
import type { SaveData } from './SaveData';

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

function midDayGame(): GameState {
  const day = midDay();
  return {
    calendar: { week: 2, weekday: 'tuesday' },
    production: day.production,
    day: { kind: 'work', workDay: day.state },
  };
}

function reload(game: GameState): SaveData {
  const loaded = deserializeSave(serializeSave(createSaveData(game)));
  if (!loaded.ok) throw new Error(loaded.message);
  return loaded.data;
}

/** The game with any task in progress removed, which is the one thing a reload changes. */
function withoutActiveTask({ day, ...rest }: GameState): unknown {
  return {
    ...rest,
    day: day.kind === 'work' ? { ...day, workDay: { ...day.workDay, activeTask: null } } : day,
  };
}

describe('saving and resuming', () => {
  it('a saved mid-day game resumes with the same day, clock, queue, stock and stats', () => {
    const game = midDayGame();

    expect(withoutActiveTask(gameStateFromSave(reload(game)))).toEqual(withoutActiveTask(game));
  });

  it('a task in progress is saved as restarting from its walk', () => {
    const game = midDayGame();
    if (game.day.kind !== 'work') throw new Error('not a work day');
    const { activeTask } = game.day.workDay;
    expect(activeTask?.phase).toBe('working');

    const saved = createSaveData(game);

    expect(saved.day.kind === 'work' && saved.day.workDay.activeTask).toEqual({
      phase: 'walking',
      task: activeTask?.task,
    });
  });

  it('a weekend day saves and resumes', () => {
    let game = newGameState(config);
    while (game.day.kind === 'work') {
      game = finishDay(game, config);
    }

    expect(gameStateFromSave(reload(game))).toEqual(game);
  });

  it('a save fits the layout it was made with', () => {
    expect(isSaveCompatible(createSaveData(midDayGame()), layout)).toBe(true);
  });

  it('a save made with a different station layout is not used', () => {
    const saved = createSaveData(midDayGame());
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
