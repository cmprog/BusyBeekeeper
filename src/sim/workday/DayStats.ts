import type { StationAction } from '../stations/StationAction';

/** Running totals for one day, shown in the end-of-day summary. */
export interface DayStats {
  readonly tasksCompleted: number;
  readonly tasksFailed: number;
  readonly framesUncapped: number;
  readonly honeyExtractedOz: number;
  readonly bearsBottled: number;
}

export const emptyDayStats: DayStats = {
  tasksCompleted: 0,
  tasksFailed: 0,
  framesUncapped: 0,
  honeyExtractedOz: 0,
  bearsBottled: 0,
};

/** Adds a completed action. Exhaustive, so every new action has to decide what it counts. */
export function countCompletedAction(stats: DayStats, action: StationAction): DayStats {
  const counted = { ...stats, tasksCompleted: stats.tasksCompleted + 1 };
  switch (action.kind) {
    case 'uncapFrame':
      return { ...counted, framesUncapped: counted.framesUncapped + 1 };
    case 'extract':
      return { ...counted, honeyExtractedOz: counted.honeyExtractedOz + action.honeyOz };
    case 'bottle':
      return { ...counted, bearsBottled: counted.bearsBottled + action.bears };
    case 'pullSuper':
    case 'placeSuper':
    case 'setDownSuper':
    case 'pickUpSuper':
    case 'loadExtractor':
    case 'pourBucket':
      return counted;
  }
}

export function countFailedTask(stats: DayStats): DayStats {
  return { ...stats, tasksFailed: stats.tasksFailed + 1 };
}
