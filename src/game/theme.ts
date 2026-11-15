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
  steel: color(0.5, 0.53, 0.57),
  woodLight: color(0.78, 0.6, 0.4),
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
    table: palette.wood,
    machine: palette.steel,
    beekeeper: palette.white,
    progressTrack: color(0.12, 0.1, 0.08, 0.6),
    progressFill: palette.honeyLight,
    textOutline: palette.bark,
    /** A super's wooden box. */
    superBox: palette.woodLight,
    frameCapped: palette.honey,
    frameUncapped: palette.honeyLight,
    /** An extracted frame, empty and back in its super. */
    frameEmpty: color(0.96, 0.91, 0.77, 0.45),
    /** A slot with no frame in it (an extractor slot, or a frame away in the extractor). */
    frameSlotEmpty: color(0.12, 0.1, 0.08, 0.35),
    yardArea: color(0.3, 0.47, 0.22),
    honeyHouseArea: color(0.6, 0.48, 0.34),
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
      inventory: 24,
      day: 28,
      summaryTitle: 34,
      summaryLine: 22,
      slotVerb: 14,
      menuTitle: 64,
      menuButton: 28,
      menuPrompt: 22,
    },
  },
  hud: {
    /** Gap between HUD elements and the canvas edges. */
    edgePx: 24,
    /** Distance of the clock's centre from the top edge. */
    clockTopPx: 40,
    hintTopPx: 80,
    warningTopPx: 116,
    inventoryItemWidthPx: 96,
    /** Centre of the end-of-day summary panel, from the top edge. */
    summaryCenterYPx: 340,
    summaryWidthPx: 440,
    summaryPaddingPx: 28,
    summaryLineHeightPx: 38,
    inventoryPanelHeightPx: 48,
    /** How far the slot icon moves up to make room for the action verb under it. */
    slotIconLiftPx: 8,
    /** Distance of the action verb below the slot's centre. */
    slotVerbOffsetPx: 22,
    queueSlotSizePx: 72,
    queueSlotGapPx: 12,
    queueProgressHeightPx: 8,
    buttonWidthPx: 200,
    buttonHeightPx: 56,
    /** A secondary button beside the main one (e.g. Menu next to End day). */
    smallButtonWidthPx: 120,
    menu: {
      titleTopPx: 200,
      /** The confirmation question, above the buttons. */
      promptTopPx: 320,
      firstButtonCenterYPx: 400,
      buttonWidthPx: 360,
      buttonHeightPx: 64,
      buttonGapPx: 20,
      noticeTopPx: 620,
    },
  },
  world: {
    stationSize: 3,
    stationIconSize: 1.5,
    stationLabelSize: 0.4,
    /** Distance of a station's label below its centre. */
    stationLabelOffset: 1.8,
    beekeeperSize: 1.4,
    beekeeperIconSize: 1.2,
    progressWidth: 2,
    progressHeight: 0.25,
    /** Distance of the work progress bar above the beekeeper's centre. */
    progressOffset: 1.2,
    /** Gap between a station's top edge and the status drawn above it (supers, gauges). */
    statusGap: 0.25,
    superWidth: 2.7,
    superHeight: 0.6,
    superGap: 0.1,
    /** Frame pips and fill gauges drawn above a station. */
    statusBarHeight: 0.45,
    pipGap: 0.08,
    gaugeTextSize: 0.4,
    /** Dark outline that keeps gauge text readable over both the fill and the empty track. */
    gaugeTextOutline: 0.08,
    /** Nudges gauge text down to sit visually centred on the bar (about 2 px). */
    gaugeTextDrop: 0.05,
    /** Padding around the stations an area (Bee Yard, Honey House) encloses. */
    areaPadding: 0.6,
    /** Padding below the lowest station label in an area. */
    areaBottomPadding: 0.3,
    /** Extra room above an area's stations for their status and the area's label. */
    areaTopRoom: 1.4,
    areaLabelSize: 0.7,
    /** Distance of an area's label below its top edge. */
    areaLabelInset: 0.55,
    carriedSuperWidth: 1.1,
    carriedSuperHeight: 0.45,
    /** Offset of the carried super from the beekeeper's centre. */
    carriedSuperOffsetX: 0.9,
  },
  motion: {
    extractorSpinRadiansPerSecond: 8,
  },
  /** How long a tap highlight or warning stays visible. */
  flashSeconds: 0.2,
  warningSeconds: 1.2,
  /** How long a longer notice (e.g. a save problem) stays visible. */
  noticeSeconds: 6,
} as const;

/** Applies engine-wide theme settings. Call once from gameInit. */
export function applyTheme(): void {
  setCanvasClearColor(theme.colors.background);
  setFontDefault(theme.font.family);
}
