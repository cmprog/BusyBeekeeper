import type { GameState } from '../week/GameState';
import { CURRENT_SAVE_VERSION } from './SaveData';
import type { SaveData, SavedDayState } from './SaveData';

/** Snapshots the game for saving. A task in progress restarts from its walk on reload. */
export function createSaveData({ calendar, production, day }: GameState): SaveData {
  return { version: CURRENT_SAVE_VERSION, calendar, production, day: savedDay(day) };
}

/** The game a loaded save resumes. */
export function gameStateFromSave({ calendar, production, day }: SaveData): GameState {
  return { calendar, production, day };
}

function savedDay(day: GameState['day']): SavedDayState {
  switch (day.kind) {
    case 'work': {
      const { activeTask } = day.workDay;
      return {
        kind: 'work',
        workDay: {
          ...day.workDay,
          activeTask: activeTask ? { phase: 'walking', task: activeTask.task } : null,
        },
      };
    }
    case 'market':
    case 'supply':
      return day;
  }
}
