/**
 * Places in a scene the beekeeper can walk to and work at.
 *
 * Placeholder set for the Phase 1 backyard. Production stations (extractor, bottling tank, …)
 * replace these in Phase 1 step 3.
 */
export const stationIds = ['hive', 'honeyHouse'] as const;

export type StationId = (typeof stationIds)[number];
