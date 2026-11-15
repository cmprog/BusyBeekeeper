/** Parsed save data of any version, before it's validated as the current `SaveData`. */
export interface VersionedRecord {
  readonly version: number;
  readonly [key: string]: unknown;
}

export interface SaveMigration {
  /** The version this migration upgrades from. It must return data at `fromVersion + 1`. */
  readonly fromVersion: number;
  readonly migrate: (data: VersionedRecord) => VersionedRecord;
}

/**
 * Upgrades `data` one version at a time until it reaches `targetVersion`.
 * Throws if a step is missing or a migration doesn't advance the version by exactly one.
 */
export function runMigrations(
  data: VersionedRecord,
  migrations: readonly SaveMigration[],
  targetVersion: number,
): VersionedRecord {
  let current = data;

  while (current.version < targetVersion) {
    const fromVersion = current.version;
    const migration = migrations.find((candidate) => candidate.fromVersion === fromVersion);
    if (!migration) {
      throw new Error(`No save migration from version ${fromVersion}`);
    }

    current = migration.migrate(current);
    if (current.version !== fromVersion + 1) {
      throw new Error(
        `Save migration from version ${fromVersion} produced version ${current.version}, expected ${fromVersion + 1}`,
      );
    }
  }

  return current;
}
