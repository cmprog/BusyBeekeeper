import type { SaveData } from '../../sim/save/SaveData';
import type { LoadOutcome } from '../SaveStore';

export interface MainMenuChoices {
  /** The game Continue resumes, or undefined if there's nothing to continue. */
  readonly continueSave: SaveData | undefined;
  /** New Game asks first whenever it would overwrite a game the player could still continue. */
  readonly confirmNewGame: boolean;
  /** Explains a save problem, if there was one. */
  readonly notice: string | undefined;
}

/** What the main menu offers for a given save slot. */
export function mainMenuChoices(outcome: LoadOutcome): MainMenuChoices {
  switch (outcome.kind) {
    case 'empty':
      return { continueSave: undefined, confirmNewGame: false, notice: undefined };
    case 'loaded':
      return { continueSave: outcome.data, confirmNewGame: true, notice: undefined };
    case 'failed':
      return { continueSave: undefined, confirmNewGame: false, notice: failureNotice(outcome) };
  }
}

function failureNotice(outcome: Extract<LoadOutcome, { kind: 'failed' }>): string {
  if (outcome.reason === 'storageUnavailable') {
    return "Saving isn't available in this browser, so progress won't be kept.";
  }
  const backup = outcome.backedUp ? ' A backup copy was kept.' : '';
  return `Your save couldn't be loaded.${backup}`;
}
