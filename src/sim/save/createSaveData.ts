import type { WorkDayState } from '../workday/WorkDayState';
import { CURRENT_SAVE_VERSION } from './SaveData';
import type { SaveData } from './SaveData';

/** Snapshots the game for saving. A task in progress restarts from its walk on reload. */
export function createSaveData(dayNumber: number, workDay: WorkDayState): SaveData {
  const { activeTask } = workDay;
  return {
    version: CURRENT_SAVE_VERSION,
    dayNumber,
    workDay: {
      ...workDay,
      activeTask: activeTask ? { phase: 'walking', task: activeTask.task } : null,
    },
  };
}
