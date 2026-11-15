/**
 * Screen/world layout constants for the presentation layer.
 *
 * The canvas renders at a fixed 16:9 resolution that LittleJS scales to fit the window, so the
 * layout is identical on every device. World units are what game objects are positioned in:
 * the visible world spans 0..worldWidth × 0..worldHeight (LittleJS world space is y-up).
 */
export const viewport = {
  canvasWidthPx: 1280,
  canvasHeightPx: 720,
  worldWidth: 32,
  worldHeight: 18,
} as const;

/** Pixels per world unit. */
export const cameraScale = viewport.canvasWidthPx / viewport.worldWidth;
