import type { SaveMigration } from './runMigrations';

/**
 * Ordered save migrations. When `SaveData` changes shape:
 *  1. bump `CURRENT_SAVE_VERSION` in SaveData.ts,
 *  2. append `{ fromVersion: <previous version>, migrate }` here, returning the new shape,
 *  3. add a test that a save at the previous version loads correctly.
 *
 * Never edit or remove a shipped migration: players may still have saves at any old version.
 */
export const saveMigrations: readonly SaveMigration[] = [];
