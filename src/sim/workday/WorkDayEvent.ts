import type { StationAction } from '../production/StationAction';
import type { TaskFailure } from '../production/TaskFailure';
import type { Task } from './Task';

/** Things that happened during a `tick`, for the presentation layer to react to. */
export type WorkDayEvent =
  | { readonly type: 'taskCompleted'; readonly task: Task; readonly action: StationAction }
  | { readonly type: 'taskFailed'; readonly task: Task; readonly reason: TaskFailure };
