import { drawTextScreen } from 'littlejsengine';
import { drawFilledRect } from '../draw';
import type { Rect } from '../Rect';
import { theme } from '../theme';

export interface DrawButtonOptions {
  /** A disabled button is drawn greyed out. */
  readonly enabled?: boolean;
  readonly fontSizePx?: number;
}

/** A labelled HUD button, in screen pixels. */
export function drawButton(
  rect: Rect,
  label: string,
  { enabled = true, fontSizePx = theme.font.sizePx.button }: DrawButtonOptions = {},
): void {
  drawFilledRect(rect, enabled ? theme.colors.button : theme.colors.hudSlotEmpty, true);
  drawTextScreen(label, rect.center, fontSizePx, theme.colors.text);
}
