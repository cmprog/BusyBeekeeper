import type { Position } from '../sim/Position';
import type { Station } from '../sim/stations/Station';

/**
 * Shape of the central tuning / balance config (ROADMAP §3.9).
 *
 * Every balance value the game uses lives here. Names carry their unit as a suffix
 * (`Seconds`, `Beebucks`, `Oz`, …) so call sites can't mix units by accident. Honey is measured
 * in whole ounces so production math stays exact. Positions are in world units (the 32×18
 * world, see src/game/viewport.ts).
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
  /** What a new game starts with (ROADMAP §4.8). */
  readonly startingKit: {
    readonly emptyBears: number;
    readonly labels: number;
    /** How full the starting super is, from 0 (empty) to 1 (full). */
    readonly superFill: number;
  };
  readonly beekeeper: {
    readonly walkSpeedUnitsPerSecond: number;
  };
  readonly queue: {
    /** Most tasks the beekeeper can hold at once, counting the active task. An upgrade path. */
    readonly maxTasks: number;
  };
  readonly hive: {
    /** Time for the bees to fill an empty super completely. */
    readonly superFillSeconds: number;
  };
  readonly shallowSuper: {
    readonly frames: number;
    /** Honey in one completely full frame. */
    readonly honeyPerFrameOz: number;
  };
  readonly equipment: {
    readonly extractorFrames: number;
    readonly bucketCapacityOz: number;
    readonly bottlingTankCapacityOz: number;
    /** Most bears filled by one bottling task. An upgrade path. */
    readonly bottlingBatchBears: number;
  };
  readonly containers: {
    readonly bearHoneyOz: number;
  };
  /** Work time for each action, once the beekeeper has arrived at the station. */
  readonly tasks: {
    /** Pulling, placing, setting down or picking up a super. */
    readonly handleSuperSeconds: number;
    readonly uncapFrameSeconds: number;
    readonly loadExtractorSecondsPerFrame: number;
    /** Cranking the manual extractor. Fixed, however many frames are loaded. */
    readonly extractSeconds: number;
    readonly pourBucketSeconds: number;
    readonly bottleSecondsPerBear: number;
  };
  /** Layout of the backyard scene. Distances set walk times, so they are balance values too. */
  readonly backyard: {
    readonly beekeeperStart: Position;
    /** Station ids must be unique across the whole game. */
    readonly stations: readonly Station[];
  };
}
