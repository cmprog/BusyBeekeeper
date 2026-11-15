/** Kinds of station. The type picks the behaviour (see stationBehaviors.ts). */
export const stationTypes = [
  'hive',
  'uncappingTable',
  'extractor',
  'bucket',
  'bottlingTank',
] as const;

export type StationType = (typeof stationTypes)[number];
