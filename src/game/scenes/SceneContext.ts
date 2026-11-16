import type { GameConfig } from '../../config/GameConfig';
import type { SaveData } from '../../sim/save/SaveData';
import type { GameState } from '../../sim/week/GameState';
import type { LoadOutcome } from '../SaveStore';
import type { SceneRequest } from './SceneRequest';

/** What every scene is given: the config, the save slot, and a way to switch scenes. */
export interface SceneContext {
  readonly config: GameConfig;
  /** Reads the save slot, already checked against the current layout. */
  readonly loadSave: () => LoadOutcome;
  /** Writes the save slot. Returns false if it couldn't be stored. */
  readonly persist: (data: SaveData) => boolean;
  /** Switches scene at the start of the next frame, so the current frame finishes cleanly. */
  readonly goTo: (request: SceneRequest) => void;
  /** Ends `game`'s day: saves tomorrow (so a reload resumes there) and goes to sleep. */
  readonly endDay: (game: GameState) => void;
}
