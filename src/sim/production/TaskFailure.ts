/** Why a task couldn't be done when the beekeeper arrived. Shown to the player as a message. */
export type TaskFailure =
  | 'handsFull'
  | 'noSuperOnHive'
  | 'hiveHasSuper'
  | 'noSuperOnTable'
  | 'tableOccupied'
  | 'nothingToUncap'
  | 'nothingToExtract'
  | 'bucketFull'
  | 'bucketEmpty'
  | 'tankFull'
  | 'notEnoughHoney'
  | 'outOfBears'
  | 'outOfLabels';
