/**
 * Shape of the central tuning / balance config (ROADMAP §3.9).
 *
 * Every balance value the game uses lives here. Names carry their unit as a suffix
 * (`Seconds`, `Beebucks`, …) so call sites can't mix units by accident.
 */
export interface GameConfig {
  readonly time: {
    /** Real-time length of a Mon–Fri work day. */
    readonly workDaySeconds: number;
    /** Real-time length of Saturday's market day, tuned separately from work days. */
    readonly marketDaySeconds: number;
  };
  readonly economy: {
    readonly startingBeebucks: number;
  };
}
