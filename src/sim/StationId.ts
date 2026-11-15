/**
 * Places in a scene the beekeeper can walk to and work at. Each station has one task; what it
 * does is decided when the beekeeper arrives (see resolveStationAction.ts).
 */
export const stationIds = [
  'hive',
  'uncappingTable',
  'extractor',
  'bucket',
  'bottlingTank',
] as const;

export type StationId = (typeof stationIds)[number];
