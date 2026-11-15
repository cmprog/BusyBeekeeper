import { CURRENT_SAVE_VERSION } from './SaveData';
import type { SaveData } from './SaveData';
import { saveMigrations } from './migrations';
import { runMigrations } from './runMigrations';
import { validateSaveData } from './saveSchema';
import type { VersionedRecord } from './runMigrations';

export type SaveLoadFailureReason = 'corrupt' | 'invalid' | 'newerVersion' | 'migrationFailed';

export type DeserializeResult =
  | { readonly ok: true; readonly data: SaveData }
  | { readonly ok: false; readonly reason: SaveLoadFailureReason; readonly message: string };

export function serializeSave(data: SaveData): string {
  return JSON.stringify(data);
}

/**
 * Parses a save, migrates it to the current version, and validates its shape.
 * Never throws: every failure comes back as `{ ok: false, reason }`.
 */
export function deserializeSave(json: string): DeserializeResult {
  let parsed: unknown;
  try {
    parsed = JSON.parse(json);
  } catch {
    return fail('corrupt', 'Save is not valid JSON');
  }

  if (!isVersionedRecord(parsed)) {
    return fail('invalid', 'Save has no valid version number');
  }

  // A save from a newer build can't be understood. Refuse it rather than half-load it.
  if (parsed.version > CURRENT_SAVE_VERSION) {
    return fail(
      'newerVersion',
      `Save version ${parsed.version} is newer than this game's ${CURRENT_SAVE_VERSION}`,
    );
  }

  let migrated: VersionedRecord;
  try {
    migrated = runMigrations(parsed, saveMigrations, CURRENT_SAVE_VERSION);
  } catch (error) {
    return fail('migrationFailed', error instanceof Error ? error.message : 'Unknown error');
  }

  const validation = validateSaveData(migrated);
  if (!validation.ok) {
    return fail('invalid', `Save data does not match the expected shape (${validation.message})`);
  }
  return { ok: true, data: validation.data };
}

function fail(reason: SaveLoadFailureReason, message: string): DeserializeResult {
  return { ok: false, reason, message };
}

function isVersionedRecord(value: unknown): value is VersionedRecord {
  if (typeof value !== 'object' || value === null || Array.isArray(value)) {
    return false;
  }
  const version = (value as Record<string, unknown>)['version'];
  return typeof version === 'number' && Number.isInteger(version) && version >= 1;
}
