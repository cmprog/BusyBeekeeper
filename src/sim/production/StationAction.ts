/**
 * What the beekeeper actually does at a station, decided on arrival. Counts are fixed at that
 * moment, so the work time and the result always match.
 */
export type StationAction =
  | { readonly kind: 'pullSuper' }
  | { readonly kind: 'placeSuper' }
  | { readonly kind: 'setDownSuper' }
  | { readonly kind: 'uncapFrame' }
  | { readonly kind: 'pickUpSuper' }
  | { readonly kind: 'loadExtractor'; readonly frames: number }
  | { readonly kind: 'extract' }
  | { readonly kind: 'pourBucket'; readonly honeyOz: number }
  | { readonly kind: 'bottle'; readonly bears: number };
