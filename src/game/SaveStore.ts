import { deserializeSave, serializeSave } from '../sim/save/saveCodec';
import type { SaveLoadFailureReason } from '../sim/save/saveCodec';
import type { SaveData } from '../sim/save/SaveData';

/** The part of the Web Storage API that SaveStore needs. `localStorage` satisfies it. */
export interface KeyValueStorage {
  getItem(key: string): string | null;
  setItem(key: string, value: string): void;
}

export const saveKeys = {
  save: 'busyBeekeeper.save',
  /** The last save that failed to load, kept so a bug never silently wipes progress. */
  backup: 'busyBeekeeper.save.backup',
} as const;

export type LoadOutcome =
  | { readonly kind: 'empty' }
  | { readonly kind: 'loaded'; readonly data: SaveData }
  | {
      readonly kind: 'failed';
      readonly reason: SaveLoadFailureReason | 'incompatible' | 'storageUnavailable';
      readonly backedUp: boolean;
    };

/** Persists the single save slot. Storage is passed in so the rules are testable without a browser. */
export class SaveStore {
  readonly #storage: KeyValueStorage;

  constructor(storage: KeyValueStorage) {
    this.#storage = storage;
  }

  /**
   * Loads the save. If it can't be read, or `isUsable` rejects it (e.g. it doesn't fit the
   * current layout), it's copied to the backup key before failing.
   */
  load(isUsable: (data: SaveData) => boolean = () => true): LoadOutcome {
    let raw: string | null;
    try {
      raw = this.#storage.getItem(saveKeys.save);
    } catch (error) {
      console.error('Save storage is unavailable', error);
      return { kind: 'failed', reason: 'storageUnavailable', backedUp: false };
    }

    if (raw === null) {
      return { kind: 'empty' };
    }

    const result = deserializeSave(raw);
    if (result.ok) {
      if (isUsable(result.data)) {
        return { kind: 'loaded', data: result.data };
      }
      console.error('Save does not fit the current game layout');
      return { kind: 'failed', reason: 'incompatible', backedUp: this.#backup(raw) };
    }

    console.error(`Save failed to load (${result.reason}): ${result.message}`);
    return { kind: 'failed', reason: result.reason, backedUp: this.#backup(raw) };
  }

  /** Returns false if the save couldn't be written (e.g. storage full or blocked). */
  save(data: SaveData): boolean {
    try {
      this.#storage.setItem(saveKeys.save, serializeSave(data));
      return true;
    } catch (error) {
      console.error('Could not write save', error);
      return false;
    }
  }

  #backup(raw: string): boolean {
    try {
      this.#storage.setItem(saveKeys.backup, raw);
      return true;
    } catch (error) {
      console.error('Could not back up the unreadable save', error);
      return false;
    }
  }
}
