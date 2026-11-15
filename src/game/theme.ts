import { rgb, setCanvasClearColor, setFontDefault } from 'littlejsengine';
import type { Color } from 'littlejsengine';

/**
 * Central visual theme: colours, typography, and spacing for the presentation layer.
 *
 * All colours are created here: lint forbids `rgb`/`hsl`/`new Color` elsewhere in game/.
 * Typography sizes and spacing are in canvas pixels (the fixed 1280×720 space, see viewport.ts).
 *
 * Two tiers: `palette` holds the raw named colours, and `theme.colors` maps UI *roles* onto
 * them. Game code uses roles, so a palette tweak re-skins everything consistently.
 *
 * index.html repeats the page background/text colours in CSS (needed before JS loads), so
 * keep those in sync with `palette.bark` / `palette.cream`.
 */

/** Frozen so a shared colour can't be mutated by accident (LittleJS Colors are mutable). */
function color(r: number, g: number, b: number, a = 1): Color {
  return Object.freeze(rgb(r, g, b, a));
}

const palette = {
  grass: color(0.36, 0.55, 0.27),
  honey: color(0.85, 0.6, 0.15),
  honeyLight: color(1, 0.85, 0.2),
  cream: color(0.96, 0.91, 0.77),
  bark: color(0.12, 0.1, 0.08),
  white: color(1, 1, 1),
} as const;

export const theme = {
  colors: {
    background: palette.grass,
    text: palette.cream,
    interactive: palette.honey,
    interactiveActive: palette.honeyLight,
    tapMarker: color(1, 1, 1, 0.8),
  },
  font: {
    family: 'system-ui, sans-serif',
    sizePx: {
      hud: 24,
    },
  },
  spacing: {
    /** Distance of HUD text from the top edge of the canvas. */
    hudTopPx: 30,
  },
} as const;

/** Applies engine-wide theme settings. Call once from gameInit. */
export function applyTheme(): void {
  setCanvasClearColor(theme.colors.background);
  setFontDefault(theme.font.family);
}
