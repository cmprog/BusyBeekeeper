import { rgb, setCanvasClearColor, setFontDefault } from 'littlejsengine';
import type { Color } from 'littlejsengine';

/**
 * Central visual theme: colours, typography, and sizes for the presentation layer.
 *
 * All colours are created here: lint forbids `rgb`/`hsl`/`new Color` elsewhere in game/.
 *
 * Two size spaces:
 * - `Px` values are canvas pixels in the fixed 1280×720 space (see viewport.ts), used by the HUD.
 * - `world` values are world units (40 px each), used by things drawn in the scene.
 *
 * Two colour tiers: `palette` holds the raw named colours, and `theme.colors` maps UI *roles*
 * onto them. Game code uses roles, so a palette tweak re-skins everything consistently.
 *
 * Some colours must be repeated outside TypeScript, so keep these in sync with the palette:
 * index.html CSS (bark, cream), vite.config.ts PWA manifest (bark), public/icon.svg and
 * pwa-assets.config.ts (bark, honey, honeyLight).
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
  wood: color(0.55, 0.36, 0.22),
  white: color(1, 1, 1),
  red: color(0.85, 0.27, 0.22),
} as const;

export const theme = {
  colors: {
    background: palette.grass,
    text: palette.cream,
    textMuted: color(0.96, 0.91, 0.77, 0.6),
    /** Brief highlight on whatever was just tapped. */
    tapFlash: palette.honeyLight,
    hive: palette.honey,
    honeyHouse: palette.wood,
    beekeeper: palette.white,
    progressTrack: color(0.12, 0.1, 0.08, 0.6),
    progressFill: palette.honeyLight,
    hudPanel: color(0.12, 0.1, 0.08, 0.8),
    hudSlotEmpty: color(0.12, 0.1, 0.08, 0.35),
    button: palette.honey,
    warning: palette.red,
  },
  font: {
    family: 'system-ui, sans-serif',
    sizePx: {
      hud: 24,
      clock: 40,
      hint: 18,
      slotIcon: 36,
      button: 22,
    },
  },
  hud: {
    /** Gap between HUD elements and the canvas edges. */
    edgePx: 24,
    /** Distance of the clock's centre from the top edge. */
    clockTopPx: 40,
    hintTopPx: 80,
    warningTopPx: 116,
    queueSlotSizePx: 72,
    queueSlotGapPx: 12,
    queueProgressHeightPx: 8,
    buttonWidthPx: 200,
    buttonHeightPx: 56,
  },
  world: {
    stationSize: 4,
    stationIconSize: 2,
    stationLabelSize: 0.6,
    /** Distance of a station's label below its centre. */
    stationLabelOffset: 2.6,
    beekeeperSize: 1.4,
    beekeeperIconSize: 1.2,
    progressWidth: 2,
    progressHeight: 0.25,
    /** Distance of the work progress bar above the beekeeper's centre. */
    progressOffset: 1.2,
  },
  /** How long a tap highlight or warning stays visible. */
  flashSeconds: 0.2,
  warningSeconds: 1.2,
} as const;

/** Applies engine-wide theme settings. Call once from gameInit. */
export function applyTheme(): void {
  setCanvasClearColor(theme.colors.background);
  setFontDefault(theme.font.family);
}
