import type { StationId } from '../StationId';

/**
 * Stand-in task with no effect: walk to a station and work there for a fixed time.
 * Removed once production tasks arrive (Phase 1 step 3).
 */
export interface PlaceholderTask {
  readonly kind: 'placeholder';
  /** Unique within a day, so a specific task can be cancelled even if others look identical. */
  readonly id: number;
  readonly stationId: StationId;
}

/** Every task the beekeeper can queue. Add variants here and handle them exhaustively. */
export type Task = PlaceholderTask;
