import type { TaskFailure } from '../sim/production/TaskFailure';

/** Short player-facing message for each failure. A `Record`, so none can be missed. */
export const taskFailureText: Readonly<Record<TaskFailure, string>> = {
  handsFull: 'Hands are full: set the super down first',
  noSuperOnHive: 'No super on the hive',
  noSuperOnTable: 'No super on the uncapping table',
  nothingToUncap: 'Nothing left to uncap',
  nothingToExtract: 'No uncapped frames to extract',
  bucketFull: 'The bucket is too full: pour it first',
  bucketEmpty: 'The bucket is empty',
  tankFull: 'The bottling tank is full',
  notEnoughHoney: 'Not enough honey in the tank',
  outOfBears: 'Out of honey bears',
  outOfLabels: 'Out of labels',
};
