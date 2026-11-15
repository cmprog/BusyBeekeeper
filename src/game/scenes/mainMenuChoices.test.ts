import { describe, expect, it } from 'vitest';
import { defaultConfig } from '../../config/defaultConfig';
import { createSaveData } from '../../sim/save/createSaveData';
import { WorkDay } from '../../sim/workday/WorkDay';
import { mainMenuChoices } from './mainMenuChoices';

const save = createSaveData(4, WorkDay.initialState(defaultConfig));

describe('main menu choices', () => {
  it('offers Continue and confirms before New Game overwrites a usable save', () => {
    expect(mainMenuChoices({ kind: 'loaded', data: save })).toMatchObject({
      continueSave: save,
      confirmNewGame: true,
    });
  });

  it('starts a new game without asking when there is nothing to lose', () => {
    expect(mainMenuChoices({ kind: 'empty' })).toMatchObject({
      continueSave: undefined,
      confirmNewGame: false,
    });
  });

  it('explains an unreadable save instead of offering Continue', () => {
    const choices = mainMenuChoices({ kind: 'failed', reason: 'corrupt', backedUp: true });

    expect(choices.continueSave).toBeUndefined();
    expect(choices.notice).toContain('backup');
  });
});
