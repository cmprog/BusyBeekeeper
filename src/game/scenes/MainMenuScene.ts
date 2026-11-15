import { drawTextScreen, mousePosScreen, mouseWasPressed, vec2 } from 'littlejsengine';
import { drawButton } from '../hud/drawButton';
import { rectContains } from '../Rect';
import type { Rect } from '../Rect';
import { theme } from '../theme';
import { viewport } from '../viewport';
import { mainMenuChoices } from './mainMenuChoices';
import type { MainMenuChoices } from './mainMenuChoices';
import type { Scene } from './Scene';
import type { SceneContext } from './SceneContext';

type MenuMode = 'choose' | 'confirmNewGame';

interface MenuButton {
  readonly label: string;
  readonly rect: Rect;
  readonly onTap: () => void;
}

/**
 * Opens on launch: Continue (when there's a usable save) and New Game, which asks first if it
 * would replace that save. Reads the save slot each time it opens, so it's never stale.
 */
export class MainMenuScene implements Scene {
  readonly #context: SceneContext;
  readonly #choices: MainMenuChoices;
  #mode: MenuMode = 'choose';

  constructor(context: SceneContext) {
    this.#context = context;
    this.#choices = mainMenuChoices(context.loadSave());
  }

  update(): void {
    if (!mouseWasPressed(0)) {
      return;
    }
    const tapped = this.#buttons().find((button) => rectContains(button.rect, mousePosScreen));
    tapped?.onTap();
  }

  render(): void {
    // Nothing in world space: the menu is all HUD.
  }

  renderPost(): void {
    const centerX = viewport.canvasWidthPx / 2;
    const { menu } = theme.hud;

    drawTextScreen(
      '🐝 Busy Beekeeper',
      vec2(centerX, menu.titleTopPx),
      theme.font.sizePx.menuTitle,
      theme.colors.text,
    );
    if (this.#mode === 'confirmNewGame') {
      const day = this.#choices.continueSave?.dayNumber ?? 1;
      drawTextScreen(
        `Start a new game? Your saved game (day ${String(day)}) will be replaced.`,
        vec2(centerX, menu.promptTopPx),
        theme.font.sizePx.menuPrompt,
        theme.colors.text,
      );
    }
    for (const button of this.#buttons()) {
      drawButton(button.rect, button.label, { fontSizePx: theme.font.sizePx.menuButton });
    }
    if (this.#choices.notice) {
      drawTextScreen(
        this.#choices.notice,
        vec2(centerX, menu.noticeTopPx),
        theme.font.sizePx.menuPrompt,
        theme.colors.warning,
      );
    }
  }

  /** The buttons for the current mode, top to bottom. Drawing and tapping both use this list. */
  #buttons(): readonly MenuButton[] {
    const { continueSave, confirmNewGame } = this.#choices;
    const newGame = (): void => {
      this.#context.goTo({ kind: 'backyard', saved: undefined });
    };

    const entries: { label: string; onTap: () => void }[] = [];
    switch (this.#mode) {
      case 'choose':
        if (continueSave) {
          entries.push({
            label: `Continue (day ${String(continueSave.dayNumber)})`,
            onTap: () => {
              this.#context.goTo({ kind: 'backyard', saved: continueSave });
            },
          });
        }
        entries.push({
          label: 'New Game',
          onTap: confirmNewGame
            ? () => {
                this.#mode = 'confirmNewGame';
              }
            : newGame,
        });
        break;
      case 'confirmNewGame':
        entries.push(
          { label: 'Replace save', onTap: newGame },
          {
            label: 'Cancel',
            onTap: () => {
              this.#mode = 'choose';
            },
          },
        );
        break;
    }

    const { firstButtonCenterYPx, buttonWidthPx, buttonHeightPx, buttonGapPx } = theme.hud.menu;
    return entries.map((entry, index) => ({
      ...entry,
      rect: {
        center: vec2(
          viewport.canvasWidthPx / 2,
          firstButtonCenterYPx + index * (buttonHeightPx + buttonGapPx),
        ),
        size: vec2(buttonWidthPx, buttonHeightPx),
      },
    }));
  }
}
