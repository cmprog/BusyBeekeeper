import { drawTextScreen, mousePosScreen, mouseWasPressed, vec2 } from 'littlejsengine';
import type { GameState } from '../../sim/week/GameState';
import { dayName } from '../calendarText';
import { drawButton } from '../hud/drawButton';
import { rectContains } from '../Rect';
import type { Rect } from '../Rect';
import { theme } from '../theme';
import { viewport } from '../viewport';
import type { Scene } from './Scene';
import type { SceneContext } from './SceneContext';

const comingSoon = {
  market: 'Market day: the market stall is coming soon.',
  supply: 'Supply day: the shop is coming soon.',
} as const;

/**
 * Stands in for Saturday's market and Sunday's shop until they're built, so the week loop is
 * complete. The day is already saved when it starts, so there's nothing to save here.
 */
export class PlaceholderDayScene implements Scene {
  readonly #context: SceneContext;
  readonly #game: GameState;
  readonly #kind: keyof typeof comingSoon;

  constructor(context: SceneContext, game: GameState, kind: keyof typeof comingSoon) {
    this.#context = context;
    this.#game = game;
    this.#kind = kind;
  }

  update(): void {
    if (!mouseWasPressed(0)) {
      return;
    }
    if (rectContains(this.#buttonRect(0), mousePosScreen)) {
      this.#context.endDay(this.#game);
    } else if (rectContains(this.#buttonRect(1), mousePosScreen)) {
      this.#context.goTo({ kind: 'mainMenu' });
    }
  }

  render(): void {
    // Nothing in world space yet.
  }

  renderPost(): void {
    const centerX = viewport.canvasWidthPx / 2;
    const { menu } = theme.hud;
    drawTextScreen(
      dayName(this.#game.calendar),
      vec2(centerX, menu.titleTopPx),
      theme.font.sizePx.menuTitle,
      theme.colors.text,
    );
    drawTextScreen(
      comingSoon[this.#kind],
      vec2(centerX, menu.promptTopPx),
      theme.font.sizePx.menuPrompt,
      theme.colors.text,
    );
    drawButton(this.#buttonRect(0), 'Continue', { fontSizePx: theme.font.sizePx.menuButton });
    drawButton(this.#buttonRect(1), 'Menu', { fontSizePx: theme.font.sizePx.menuButton });
  }

  /** Buttons stacked like the main menu's. */
  #buttonRect(index: number): Rect {
    const { firstButtonCenterYPx, buttonWidthPx, buttonHeightPx, buttonGapPx } = theme.hud.menu;
    return {
      center: vec2(
        viewport.canvasWidthPx / 2,
        firstButtonCenterYPx + index * (buttonHeightPx + buttonGapPx),
      ),
      size: vec2(buttonWidthPx, buttonHeightPx),
    };
  }
}
