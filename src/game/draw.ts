import { drawRect, vec2 } from 'littlejsengine';
import type { Color } from 'littlejsengine';
import type { Rect } from './Rect';
import { theme } from './theme';

// Shared drawing helpers. `screenSpace` draws are HUD (pixels) and skip WebGL so they land on
// the 2D layer above the scene; world draws use the engine default.

function webGlFor(screenSpace: boolean): boolean | undefined {
  return screenSpace ? false : undefined;
}

export function drawFilledRect(rect: Rect, color: Color, screenSpace = false): void {
  drawRect(rect.center, rect.size, color, 0, webGlFor(screenSpace), screenSpace);
}

/** Left-to-right fill from 0 to 1. */
export function drawProgressBar(rect: Rect, progress: number, screenSpace = false): void {
  const fillWidth = rect.size.x * Math.min(1, Math.max(0, progress));
  const fill: Rect = {
    center: vec2(rect.center.x - (rect.size.x - fillWidth) / 2, rect.center.y),
    size: vec2(fillWidth, rect.size.y),
  };
  drawFilledRect(rect, theme.colors.progressTrack, screenSpace);
  drawFilledRect(fill, theme.colors.progressFill, screenSpace);
}

/** A row of equal pips across `rect`, one per colour, in world units. */
export function drawPips(rect: Rect, colors: readonly Color[]): void {
  const count = colors.length;
  if (count === 0) {
    return;
  }
  const gap = theme.world.pipGap;
  const width = (rect.size.x - gap * (count - 1)) / count;
  const left = rect.center.x - rect.size.x / 2;
  colors.forEach((color, index) => {
    const center = vec2(left + width / 2 + index * (width + gap), rect.center.y);
    drawFilledRect({ center, size: vec2(width, rect.size.y) }, color);
  });
}

/** Shrinks a rect by `inset` on every side. */
export function insetRect(rect: Rect, inset: number): Rect {
  return { center: rect.center, size: rect.size.subtract(vec2(inset * 2)) };
}
