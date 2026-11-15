import type { Station } from '../stations/Station';
import type { SaveData } from './SaveData';

/**
 * Whether a valid save fits the current scene layout. The schema checks shape; this checks the
 * save refers to stations and supers that actually exist, so a layout change between builds
 * can't load a save the sim would then trip over. (A migration should normally adapt old saves
 * to a new layout; this is the safety net.)
 */
export function isSaveCompatible(data: SaveData, layout: readonly Station[]): boolean {
  const { production, activeTask, queuedTasks } = data.workDay;
  const stationStates = production.stations;

  const layoutMatches =
    layout.length === Object.keys(stationStates).length &&
    layout.every((station) => stationStates[station.id]?.type === station.type);

  const layoutIds = new Set(layout.map((station) => station.id));
  const tasks = activeTask ? [activeTask.task, ...queuedTasks] : queuedTasks;
  const tasksMatch = tasks.every((task) => layoutIds.has(task.stationId));

  const referencedSupers = [
    production.carriedSuperId,
    ...Object.values(stationStates).map((state) => {
      switch (state.type) {
        case 'hive':
        case 'uncappingTable':
          return state.superId;
        case 'extractor':
          return state.loadedSuperId;
        case 'bucket':
        case 'bottlingTank':
          return null;
      }
    }),
  ];
  const supersExist = referencedSupers.every(
    (superId) => superId === null || superId in production.supers,
  );

  return layoutMatches && tasksMatch && supersExist;
}
