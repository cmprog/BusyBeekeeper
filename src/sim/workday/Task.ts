import type { StationId } from '../StationId';

/**
 * A queued task: walk to a station and do its job there. The job itself (pull a super, uncap a
 * frame, …) is decided on arrival from the state at that moment, because earlier tasks in the
 * queue change what is possible.
 */
export interface Task {
  /** Unique within a day, so a specific task can be cancelled even if others look identical. */
  readonly id: number;
  readonly stationId: StationId;
}
