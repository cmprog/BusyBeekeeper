import type { Color } from 'littlejsengine';
import type { StationId } from '../sim/StationId';
import { theme } from './theme';

export interface StationVisual {
  readonly label: string;
  readonly icon: string;
  readonly color: Color;
}

/** How each station looks. A `Record`, so adding a station without a visual is a type error. */
export const stationVisuals: Readonly<Record<StationId, StationVisual>> = {
  hive: { label: 'Hive', icon: '🐝', color: theme.colors.hive },
  honeyHouse: { label: 'Honey House', icon: '🏠', color: theme.colors.honeyHouse },
};
