import type { CalendarDay } from '../../sim/week/CalendarDay';
import type { GameState } from '../../sim/week/GameState';

/** A scene to switch to, with what it needs to start. */
export type SceneRequest =
  | { readonly kind: 'mainMenu' }
  /** Play a day: the scene depends on the kind of day (work, market, supply). */
  | { readonly kind: 'day'; readonly game: GameState }
  /** The night after `night`, then on to `next` (already saved). */
  | { readonly kind: 'sleep'; readonly night: CalendarDay; readonly next: GameState };
