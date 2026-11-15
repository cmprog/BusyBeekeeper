import * as v from 'valibot';
import { stationBehaviors } from '../stations/stationBehaviors';
import { stationTypes } from '../stations/StationType';
import { CURRENT_SAVE_VERSION } from './SaveData';
import type { SaveData } from './SaveData';
import { countSchema, finiteSchema, fractionSchema, nonNegativeSchema } from './schemaParts';

// Runtime validation for a loaded save. The interfaces in sim/ stay the source of truth:
// `validateSaveData` only compiles while this schema's output is assignable to `SaveData`, so a
// state field missing here is a compile error. Station states come from each behaviour.

const taskSchema = v.object({ id: countSchema, stationId: v.string() });

const superSchema = v.object({
  id: v.string(),
  contents: v.variant('stage', [
    v.object({ stage: v.literal('filling'), fill: fractionSchema }),
    v.object({
      stage: v.literal('harvested'),
      frames: v.object({ capped: countSchema, uncapped: countSchema, extracted: countSchema }),
      honeyPerFrameOz: countSchema,
    }),
  ]),
});

const stationStateSchema = v.union(stationTypes.map((type) => stationBehaviors[type].stateSchema));

const productionSchema = v.object({
  supers: v.record(v.string(), superSchema),
  stations: v.record(v.string(), stationStateSchema),
  carriedSuperId: v.nullable(v.string()),
  inventory: v.object({ emptyBears: countSchema, labels: countSchema, filledBears: countSchema }),
});

const workDaySchema = v.object({
  status: v.picklist(['running', 'windingDown', 'ended']),
  elapsedSeconds: nonNegativeSchema,
  beekeeperPosition: v.object({ x: finiteSchema, y: finiteSchema }),
  activeTask: v.nullable(v.object({ phase: v.literal('walking'), task: taskSchema })),
  queuedTasks: v.array(taskSchema),
  nextTaskId: countSchema,
  production: productionSchema,
  stats: v.object({
    tasksCompleted: countSchema,
    tasksFailed: countSchema,
    framesUncapped: countSchema,
    honeyExtractedOz: countSchema,
    bearsBottled: countSchema,
  }),
});

const saveDataSchema = v.object({
  version: v.literal(CURRENT_SAVE_VERSION),
  dayNumber: v.pipe(countSchema, v.minValue(1)),
  workDay: workDaySchema,
});

export type SaveValidation =
  { readonly ok: true; readonly data: SaveData } | { readonly ok: false; readonly message: string };

/** Checks that a migrated save has the current shape. */
export function validateSaveData(value: unknown): SaveValidation {
  const result = v.safeParse(saveDataSchema, value);
  if (result.success) {
    return { ok: true, data: result.output };
  }
  const [issue] = result.issues;
  const path = issue.path?.map((item) => String(item.key)).join('.') ?? '(root)';
  return { ok: false, message: `${path}: ${issue.message}` };
}
