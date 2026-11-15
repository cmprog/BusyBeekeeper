import { vec2 } from 'littlejsengine';
import type { Color } from 'littlejsengine';
import type { Station } from '../sim/stations/Station';
import type { StationType } from '../sim/stations/StationType';
import type { Rect } from './Rect';
import { theme } from './theme';

/** A labelled region of a scene drawn behind its stations, e.g. the Bee Yard or Honey House. */
export interface SceneArea {
  readonly label: string;
  readonly color: Color;
  /** The area encloses every station of these types, so it follows the layout automatically. */
  readonly stationTypes: readonly StationType[];
}

/**
 * The area's bounds: its stations' bodies plus padding, with room below for the station labels
 * and room on top for the status drawn above stations and the area's label. Undefined if the
 * layout has none of its stations.
 */
export function areaRect(area: SceneArea, layout: readonly Station[]): Rect | undefined {
  const stations = layout.filter((station) => area.stationTypes.includes(station.type));
  if (stations.length === 0) {
    return undefined;
  }
  const half = theme.world.stationSize / 2;
  const padding = theme.world.areaPadding;
  const xs = stations.map((station) => station.position.x);
  const ys = stations.map((station) => station.position.y);
  const left = Math.min(...xs) - half - padding;
  const right = Math.max(...xs) + half + padding;
  // The bottom edge clears the station labels, which sit below the stations.
  const { stationLabelOffset, stationLabelSize, areaBottomPadding } = theme.world;
  const bottom = Math.min(...ys) - stationLabelOffset - stationLabelSize - areaBottomPadding;
  const top = Math.max(...ys) + half + padding + theme.world.areaTopRoom;
  return {
    center: vec2((left + right) / 2, (bottom + top) / 2),
    size: vec2(right - left, top - bottom),
  };
}
