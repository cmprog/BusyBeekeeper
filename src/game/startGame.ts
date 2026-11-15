import {
  drawCircle,
  drawRect,
  drawText,
  drawTextScreen,
  engineInit,
  mousePos,
  mouseWasPressed,
  setCameraPos,
  setCameraScale,
  setCanvasFixedSize,
  vec2,
} from 'littlejsengine';
import type { Vector2 } from 'littlejsengine';
import { applyTheme, theme } from './theme';
import { cameraScale, viewport } from './viewport';

// Phase 0 placeholder scene: proves rendering, emoji text, and mouse/touch input in world space.
// Replaced by real scenes once the sim exists.

const hivePos = vec2(viewport.worldWidth / 2, viewport.worldHeight / 2);
const hiveSize = vec2(4, 4);

let hiveSelected = false;
let lastTap: Vector2 | undefined;

function isInside(point: Vector2, center: Vector2, size: Vector2): boolean {
  return Math.abs(point.x - center.x) <= size.x / 2 && Math.abs(point.y - center.y) <= size.y / 2;
}

function gameInit(): void {
  setCanvasFixedSize(vec2(viewport.canvasWidthPx, viewport.canvasHeightPx));
  setCameraScale(cameraScale);
  setCameraPos(vec2(viewport.worldWidth / 2, viewport.worldHeight / 2));
  applyTheme();
}

function gameUpdate(): void {
  // LittleJS maps touch to mouse button 0, so one code path covers both.
  if (mouseWasPressed(0)) {
    lastTap = mousePos.copy();
    if (isInside(mousePos, hivePos, hiveSize)) {
      hiveSelected = !hiveSelected;
    }
  }
}

function gameRender(): void {
  const hiveColor = hiveSelected ? theme.colors.interactiveActive : theme.colors.interactive;
  drawRect(hivePos, hiveSize, hiveColor);
  drawText('🐝', hivePos, 2);

  if (lastTap) {
    drawCircle(lastTap, 0.5, theme.colors.tapMarker);
  }
}

function gameRenderPost(): void {
  const tapText = lastTap
    ? `tap: ${lastTap.x.toFixed(1)}, ${lastTap.y.toFixed(1)}`
    : 'tap anywhere';
  drawTextScreen(
    `Busy Beekeeper — ${tapText}`,
    vec2(viewport.canvasWidthPx / 2, theme.spacing.hudTopPx),
    theme.font.sizePx.hud,
    theme.colors.text,
  );
}

export function startGame(): Promise<void> {
  return engineInit(gameInit, gameUpdate, () => undefined, gameRender, gameRenderPost);
}
