import { beforeEach, describe, expect, it, vi } from 'vitest';
import { CURRENT_SAVE_VERSION } from '../sim/save/SaveData';
import { SaveStore, saveKeys } from './SaveStore';
import type { KeyValueStorage } from './SaveStore';

class MemoryStorage implements KeyValueStorage {
  readonly items = new Map<string, string>();

  getItem(key: string): string | null {
    return this.items.get(key) ?? null;
  }

  setItem(key: string, value: string): void {
    this.items.set(key, value);
  }
}

describe('SaveStore', () => {
  beforeEach(() => {
    // Failures are logged by design; keep test output clean.
    vi.spyOn(console, 'error').mockImplementation(() => undefined);
  });

  it('round-trips a save through storage', () => {
    const store = new SaveStore(new MemoryStorage());
    const save = { version: CURRENT_SAVE_VERSION, beebucks: 42, rngSeed: 7 } as const;

    store.save(save);

    expect(store.load()).toEqual({ kind: 'loaded', data: save });
  });

  it('backs up an unreadable save verbatim before reporting failure', () => {
    const storage = new MemoryStorage();
    const unreadable = '{"version": 1, "beebucks": "lots"}';
    storage.setItem(saveKeys.save, unreadable);

    const outcome = new SaveStore(storage).load();

    expect(outcome).toEqual({ kind: 'failed', reason: 'invalid', backedUp: true });
    expect(storage.items.get(saveKeys.backup)).toBe(unreadable);
  });

  it('reports a failure instead of throwing when storage is blocked', () => {
    const blocked: KeyValueStorage = {
      getItem: () => {
        throw new Error('SecurityError: storage disabled');
      },
      setItem: () => {
        throw new Error('SecurityError: storage disabled');
      },
    };
    const store = new SaveStore(blocked);

    expect(store.load()).toEqual({
      kind: 'failed',
      reason: 'storageUnavailable',
      backedUp: false,
    });
    expect(store.save({ version: CURRENT_SAVE_VERSION, beebucks: 0, rngSeed: 1 })).toBe(false);
  });
});
