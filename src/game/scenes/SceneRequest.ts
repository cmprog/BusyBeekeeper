import type { SaveData } from '../../sim/save/SaveData';

/** A scene to switch to, with what it needs to start. */
export type SceneRequest =
  | { readonly kind: 'mainMenu' }
  /** `saved` is the game to resume, or undefined for a new game. */
  | { readonly kind: 'backyard'; readonly saved: SaveData | undefined };
