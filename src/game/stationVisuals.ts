import type { Color } from 'littlejsengine';
import type { StationType } from '../sim/stations/StationType';
import { theme } from './theme';

export interface StationVisual {
  readonly label: string;
  readonly icon: string;
  readonly color: Color;
}

/** How each station type looks. A `Record`, so a type without a visual is a compile error. */
export const stationVisuals: Readonly<Record<StationType, StationVisual>> = {
  hive: { label: 'Hive', icon: '🐝', color: theme.colors.hive },
  uncappingTable: { label: 'Uncapping', icon: '🔪', color: theme.colors.table },
  extractor: { label: 'Extractor', icon: '🌀', color: theme.colors.machine },
  bucket: { label: 'Bucket', icon: '🪣', color: theme.colors.machine },
  bottlingTank: { label: 'Bottling', icon: '🍯', color: theme.colors.machine },
};
