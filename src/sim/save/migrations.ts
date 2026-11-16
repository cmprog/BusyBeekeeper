import type { SaveMigration } from './runMigrations';

/**
 * Ordered save migrations, one per released version (see the save rules in CLAUDE.md):
 *  - The first change after a release bumps `CURRENT_SAVE_VERSION` in SaveData.ts and appends
 *    `{ fromVersion: <released version>, migrate }` here, returning the new shape. Every later
 *    change before the next release goes into that same migration.
 *  - Test it against the released version's fixture in `fixtures/`.
 *
 * Never edit or remove a migration from a released version: players may still have saves at
 * any old version. Migrations are self-contained (no imports of live game code), so later
 * changes to the game can't change what an old migration does.
 *
 * Empty: no save version has been released yet.
 */
export const saveMigrations: readonly SaveMigration[] = [];
