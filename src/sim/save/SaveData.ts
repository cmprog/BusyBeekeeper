/**
 * Current save format version. Whenever `SaveData` changes shape, bump this and add a
 * migration in `migrations.ts` (with a test that an old save still loads).
 */
export const CURRENT_SAVE_VERSION = 1;

/** The persisted game state. Placeholder fields until the sim's real state exists. */
export interface SaveData {
  readonly version: typeof CURRENT_SAVE_VERSION;
  readonly beebucks: number;
  /** LittleJS `RandomGenerator.seed`, so the random sequence resumes exactly after a load. */
  readonly rngSeed: number;
}
