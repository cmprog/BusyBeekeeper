import { vec2 } from 'littlejsengine';
import type { GameConfig } from '../../config/GameConfig';
import type { ProductionState } from '../../sim/production/ProductionState';
import type { Station } from '../../sim/stations/Station';
import type { StationAction } from '../../sim/stations/StationAction';
import type { Rect } from '../Rect';
import { theme } from '../theme';

export interface StationRenderContext {
  readonly station: Station;
  /** The station's body, in world units. */
  readonly rect: Rect;
  readonly production: ProductionState;
  readonly config: GameConfig;
  /** What the beekeeper is working on at this station right now, if anything. */
  readonly activeAction: StationAction | undefined;
}

/**
 * How one station type shows its state. The scene draws every station's body, icon and label;
 * the renderer adds what is specific to its type, above the body. Mirrors the sim's station
 * behaviours: one per type, registered in stationRenderers.ts.
 */
export interface StationRenderer {
  drawStatus(context: StationRenderContext): void;
  /** Rotation for the station's icon, e.g. a spinning extractor. Defaults to upright. */
  iconAngle?(context: StationRenderContext): number;
}

/**
 * The `index`th row of status above a station, counting up from its top edge. Rows are
 * `height` tall with `theme.world.superGap` between them, so stacks (supers on a hive) grow up.
 */
export function statusRowRect(stationRect: Rect, index: number, height: number): Rect {
  const top = stationRect.center.y + stationRect.size.y / 2;
  const bottom = top + theme.world.statusGap + index * (height + theme.world.superGap);
  return {
    center: vec2(stationRect.center.x, bottom + height / 2),
    size: vec2(theme.world.superWidth, height),
  };
}
