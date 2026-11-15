import type { Vector2 } from 'littlejsengine';

/** A centred, axis-aligned rectangle, in either world units or screen pixels. */
export interface Rect {
  readonly center: Vector2;
  readonly size: Vector2;
}

/** `point` must be in the same space (world or screen) as `rect`. */
export function rectContains(rect: Rect, point: Vector2): boolean {
  return (
    Math.abs(point.x - rect.center.x) <= rect.size.x / 2 &&
    Math.abs(point.y - rect.center.y) <= rect.size.y / 2
  );
}
