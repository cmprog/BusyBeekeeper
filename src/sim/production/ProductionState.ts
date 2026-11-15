/** Frames of a super that is off the hive. Frames loaded in the extractor are counted there. */
export interface FrameCounts {
  readonly capped: number;
  readonly uncapped: number;
  /** Emptied by the extractor and back in the super, ready to refill. */
  readonly extracted: number;
}

export type HoneySuperState =
  | {
      readonly location: 'hive';
      /** 0 (empty) to 1 (full). The bees fill it over time. */
      readonly fill: number;
    }
  | {
      readonly location: 'carried' | 'uncappingTable';
      readonly frames: FrameCounts;
      /** Honey in each capped or uncapped frame, fixed when the super was pulled. */
      readonly honeyPerFrameOz: number;
    };

/**
 * The beekeeping operation's stock and equipment (ROADMAP §4.1). Plain readonly data that
 * outlives a single day.
 */
export interface ProductionState {
  /** Phase 1 has exactly one super: the starting kit's shallow super. */
  readonly honeySuper: HoneySuperState;
  readonly extractor: {
    readonly loadedFrames: number;
    readonly honeyOz: number;
  };
  readonly bucketHoneyOz: number;
  readonly tankHoneyOz: number;
  readonly inventory: {
    readonly emptyBears: number;
    readonly labels: number;
    readonly filledBears: number;
  };
}
