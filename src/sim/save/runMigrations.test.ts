import { describe, expect, it } from 'vitest';
import { runMigrations } from './runMigrations';
import type { SaveMigration } from './runMigrations';

// Synthetic migrations exercise the mechanism. Real ones live in migrations.ts.
const v1ToV2: SaveMigration = {
  fromVersion: 1,
  migrate: (data) => ({ ...data, version: 2, honey: 0 }),
};
const v2ToV3: SaveMigration = {
  fromVersion: 2,
  migrate: ({ coins, ...rest }) => ({ ...rest, version: 3, beebucks: coins }),
};

describe('runMigrations', () => {
  it('upgrades an old save through every step, in order, to the target version', () => {
    // Registered out of order on purpose: steps are chosen by version, not array position.
    const result = runMigrations({ version: 1, coins: 50 }, [v2ToV3, v1ToV2], 3);

    expect(result).toEqual({ version: 3, beebucks: 50, honey: 0 });
  });

  it('fails loudly when a migration step is missing', () => {
    expect(() => runMigrations({ version: 1 }, [v2ToV3], 3)).toThrow(
      'No save migration from version 1',
    );
  });

  it('fails loudly when a migration does not advance the version by exactly one', () => {
    const skipsAVersion: SaveMigration = { fromVersion: 1, migrate: () => ({ version: 3 }) };

    expect(() => runMigrations({ version: 1 }, [skipsAVersion], 3)).toThrow('expected 2');
  });
});
