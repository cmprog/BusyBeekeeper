import { beforeEach, describe, expect, it, vi } from 'vitest';
import { defaultConfig } from '../config/defaultConfig';
import { createSaveData } from '../sim/save/createSaveData';
import { WorkDay } from '../sim/workday/WorkDay';
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

const save = createSaveData(1, WorkDay.initialState(defaultConfig));

describe('SaveStore', () => {
  beforeEach(() => {
    // Failures are logged by design; keep test output clean.
    vi.spyOn(console, 'error').mockImplementation(() => undefined);
  });

  it('round-trips a save through storage', () => {
    const store = new SaveStore(new MemoryStorage());

    store.save(save);

    expect(store.load()).toEqual({ kind: 'loaded', data: save });
  });

  it('backs up an unreadable save verbatim before reporting failure', () => {
    const storage = new MemoryStorage();
    const unreadable = '{"version": 1, "dayNumber": "lots"}';
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
    expect(store.save(save)).toBe(false);
  });

  it('backs up a valid save that does not fit the current game before reporting failure', () => {
    const storage = new MemoryStorage();
    const store = new SaveStore(storage);
    store.save(save);

    const outcome = store.load(() => false);

    expect(outcome).toEqual({ kind: 'failed', reason: 'incompatible', backedUp: true });
    expect(storage.items.get(saveKeys.backup)).toBe(storage.items.get(saveKeys.save));
  });
});
