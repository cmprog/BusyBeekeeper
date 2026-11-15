import type { Position } from '../sim/Position';
import type { StationId } from '../sim/StationId';

/**
 * Shape of the central tuning / balance config (ROADMAP §3.9).
 *
 * Every balance value the game uses lives here. Names carry their unit as a suffix
 * (`Seconds`, `Beebucks`, …) so call sites can't mix units by accident. Positions are in world
 * units (the 32×18 world, see src/game/viewport.ts).
 */
export interface GameConfig {
  readonly time: {
    /** Real-time length of a Mon–Fri work day. */
    readonly workDaySeconds: number;
    /** Real-time length of Saturday's market day, tuned separately from work days. */
    readonly marketDaySeconds: number;
  };
  readonly economy: {
    readonly startingBeebucks: number;
  };
  readonly beekeeper: {
    readonly walkSpeedUnitsPerSecond: number;
  };
  readonly queue: {
    /** Most tasks the beekeeper can hold at once, counting the active task. An upgrade path. */
    readonly maxTasks: number;
  };
  readonly tasks: {
    readonly placeholderWorkSeconds: number;
  };
  /** Layout of the backyard scene. Distances set walk times, so they are balance values too. */
  readonly backyard: {
    readonly beekeeperStart: Position;
    readonly stations: Readonly<Record<StationId, Position>>;
  };
}
