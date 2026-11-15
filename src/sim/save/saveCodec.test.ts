import { describe, expect, it } from 'vitest';
import { defaultConfig } from '../../config/defaultConfig';
import { WorkDay } from '../workday/WorkDay';
import { createSaveData } from './createSaveData';
import { CURRENT_SAVE_VERSION } from './SaveData';
import type { SaveData } from './SaveData';
import { deserializeSave, serializeSave } from './saveCodec';

const save: SaveData = createSaveData(1, WorkDay.initialState(defaultConfig));

/** The save as plain JSON-shaped data, for building broken variants. */
function saveJson(): Record<string, unknown> {
  return JSON.parse(serializeSave(save)) as Record<string, unknown>;
}

describe('save codec', () => {
  it('round-trips a save without loss', () => {
    expect(deserializeSave(serializeSave(save))).toEqual({ ok: true, data: save });
  });

  it('rejects corrupt JSON instead of throwing', () => {
    expect(deserializeSave('{"version": 1, "dayNu')).toMatchObject({
      ok: false,
      reason: 'corrupt',
    });
  });

  it('refuses a save from a newer game version rather than half-loading it', () => {
    const fromTheFuture = JSON.stringify({ ...saveJson(), version: CURRENT_SAVE_VERSION + 1 });

    expect(deserializeSave(fromTheFuture)).toMatchObject({ ok: false, reason: 'newerVersion' });
  });

  it.each([
    ['no version', () => ({ ...saveJson(), version: undefined })],
    ['wrong field type', () => ({ ...saveJson(), dayNumber: '1' })],
    ['missing field', () => ({ ...saveJson(), workDay: undefined })],
    ['not an object', () => [saveJson()]],
    [
      'invalid nested station state',
      () => {
        const json = saveJson();
        const workDay = json['workDay'] as { production: { stations: Record<string, unknown> } };
        workDay.production.stations['bucket'] = { type: 'bucket', honeyOz: -5 };
        return json;
      },
    ],
    [
      'unknown station type',
      () => {
        const json = saveJson();
        const workDay = json['workDay'] as { production: { stations: Record<string, unknown> } };
        workDay.production.stations['bucket'] = { type: 'teleporter' };
        return json;
      },
    ],
  ])('rejects a save with an invalid shape (%s)', (_label, build) => {
    expect(deserializeSave(JSON.stringify(build()))).toMatchObject({
      ok: false,
      reason: 'invalid',
    });
  });
});
