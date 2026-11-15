import type { StationType } from '../../sim/stations/StationType';
import { bottlingTankRenderer } from './renderers/bottlingTankRenderer';
import { bucketRenderer } from './renderers/bucketRenderer';
import { extractorRenderer } from './renderers/extractorRenderer';
import { hiveRenderer } from './renderers/hiveRenderer';
import { uncappingTableRenderer } from './renderers/uncappingTableRenderer';
import type { StationRenderer } from './StationRenderer';

/** Every station type's renderer. A `Record`, so a type without one is a compile error. */
export const stationRenderers: Readonly<Record<StationType, StationRenderer>> = {
  hive: hiveRenderer,
  uncappingTable: uncappingTableRenderer,
  extractor: extractorRenderer,
  bucket: bucketRenderer,
  bottlingTank: bottlingTankRenderer,
};
