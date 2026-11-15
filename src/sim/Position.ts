/**
 * A point in world units.
 *
 * Plain data rather than a LittleJS `Vector2`, so sim state stays JSON-serializable and a
 * loaded save has the same shape as live state.
 */
export interface Position {
  readonly x: number;
  readonly y: number;
}

export function distanceBetween(a: Position, b: Position): number {
  return Math.hypot(b.x - a.x, b.y - a.y);
}
