export type SuperId = string;

/** Frames of a harvested super. Frames loaded in an extractor are counted there instead. */
export interface FrameCounts {
  readonly capped: number;
  readonly uncapped: number;
  /** Emptied by the extractor and back in the super, ready to refill. */
  readonly extracted: number;
}

export type SuperContents =
  | {
      /** On a hive. The bees fill it over time. */
      readonly stage: 'filling';
      /** 0 (empty) to 1 (full). */
      readonly fill: number;
    }
  | {
      /** Off the hive, being processed frame by frame. */
      readonly stage: 'harvested';
      readonly frames: FrameCounts;
      /** Honey in each capped or uncapped frame, fixed when the super was pulled. */
      readonly honeyPerFrameOz: number;
    };

/**
 * A honey super, tracked as its own entity. Where it is comes from whatever refers to it (a hive,
 * an uncapping table, or the beekeeper's hands), so it can never be in two places at once.
 */
export interface HoneySuper {
  readonly id: SuperId;
  readonly contents: SuperContents;
}
