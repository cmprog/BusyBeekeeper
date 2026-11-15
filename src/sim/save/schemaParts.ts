import * as v from 'valibot';

// Building blocks shared by the save schemas (saveSchema.ts and each station behaviour's
// `stateSchema`).

/** A count: a whole number, zero or more (frames, ounces, bears, ids). */
export const countSchema = v.pipe(v.number(), v.integer(), v.minValue(0));

/** Any finite number (positions, seconds). */
export const finiteSchema = v.pipe(v.number(), v.finite());

export const nonNegativeSchema = v.pipe(v.number(), v.finite(), v.minValue(0));

/** 0 (empty) to 1 (full). */
export const fractionSchema = v.pipe(v.number(), v.minValue(0), v.maxValue(1));
