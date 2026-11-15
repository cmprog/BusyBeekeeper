import type { ProductionState } from '../production/ProductionState';
import type { TaskFailure } from '../production/TaskFailure';
import { bottlingTankBehavior } from './behaviors/bottlingTankBehavior';
import { bucketBehavior } from './behaviors/bucketBehavior';
import { extractorBehavior } from './behaviors/extractorBehavior';
import { hiveBehavior } from './behaviors/hiveBehavior';
import { uncappingTableBehavior } from './behaviors/uncappingTableBehavior';
import type { PlannedAction } from './StationAction';
import type { StationBehavior, StationContext } from './StationBehavior';
import type { StationType } from './StationType';

/**
 * Every station type's behaviour. A mapped type, so a station type without a behaviour (or with
 * the wrong one) is a compile error. To add a station type: add it to StationType.ts, give it a
 * state and actions, write its behaviour in behaviors/, and register it here.
 */
export const stationBehaviors: { readonly [K in StationType]: StationBehavior<K> } = {
  hive: hiveBehavior,
  uncappingTable: uncappingTableBehavior,
  extractor: extractorBehavior,
  bucket: bucketBehavior,
  bottlingTank: bottlingTankBehavior,
};

export type TaskResolution =
  | { readonly ok: true; readonly planned: PlannedAction; readonly workSeconds: number }
  | { readonly ok: false; readonly reason: TaskFailure };

/** On arrival: asks the station's behaviour what to do. */
export function resolveStationTask(context: StationContext): TaskResolution {
  return resolveFor(context.station.type, context);
}

/** On completion: hands the planned action back to the behaviour that planned it. */
export function applyPlannedAction(
  planned: PlannedAction,
  context: StationContext,
): ProductionState {
  return applyFor(planned, context);
}

// Generic over K so the type, behaviour and action stay correlated: a hive action can only reach
// the hive behaviour.

// eslint-disable-next-line @typescript-eslint/no-unnecessary-type-parameters -- K is what correlates `type` with the behaviour's action inside the body.
function resolveFor<K extends StationType>(type: K, context: StationContext): TaskResolution {
  const resolution = stationBehaviors[type].resolve(context);
  if (!resolution.ok) {
    return resolution;
  }
  const planned: PlannedAction<K> = { stationType: type, action: resolution.action };
  // Widening PlannedAction<K> to PlannedAction is always safe, but TypeScript can't prove it
  // for a generic K.
  return { ok: true, planned: planned as PlannedAction, workSeconds: resolution.workSeconds };
}

function applyFor<K extends StationType>(
  planned: PlannedAction<K>,
  context: StationContext,
): ProductionState {
  return stationBehaviors[planned.stationType].apply(planned.action, context);
}
