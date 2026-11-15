import { describe, expect, it } from 'vitest';
import { CURRENT_SAVE_VERSION } from './SaveData';
import type { SaveData } from './SaveData';
import { deserializeSave, serializeSave } from './saveCodec';

const save: SaveData = { version: CURRENT_SAVE_VERSION, beebucks: 100, rngSeed: 123456789 };

describe('save codec', () => {
  it('round-trips a save without loss', () => {
    expect(deserializeSave(serializeSave(save))).toEqual({ ok: true, data: save });
  });

  it('rejects corrupt JSON instead of throwing', () => {
    expect(deserializeSave('{"version": 1, "beebu')).toMatchObject({
      ok: false,
      reason: 'corrupt',
    });
  });

  it('refuses a save from a newer game version rather than half-loading it', () => {
    const fromTheFuture = JSON.stringify({ ...save, version: CURRENT_SAVE_VERSION + 1 });

    expect(deserializeSave(fromTheFuture)).toMatchObject({ ok: false, reason: 'newerVersion' });
  });

  it.each([
    ['no version', { beebucks: 100, rngSeed: 1 }],
    ['wrong field type', { ...save, beebucks: '100' }],
    ['missing field', { version: CURRENT_SAVE_VERSION, beebucks: 100 }],
    ['not an object', [save]],
  ])('rejects a save with an invalid shape (%s)', (_label, data) => {
    expect(deserializeSave(JSON.stringify(data))).toMatchObject({ ok: false, reason: 'invalid' });
  });
});
