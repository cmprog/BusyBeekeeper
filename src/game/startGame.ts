import { engineInit, setCameraPos, setCameraScale, setCanvasFixedSize, vec2 } from 'littlejsengine';
import type { GameConfig } from '../config/GameConfig';
import { isSaveCompatible } from '../sim/save/isSaveCompatible';
import { SaveStore } from './SaveStore';
import type { LoadOutcome } from './SaveStore';
import { BackyardScene } from './scenes/BackyardScene';
import { MainMenuScene } from './scenes/MainMenuScene';
import type { Scene } from './scenes/Scene';
import type { SceneContext } from './scenes/SceneContext';
import { SceneManager } from './scenes/SceneManager';
import type { SceneRequest } from './scenes/SceneRequest';
import { applyTheme } from './theme';
import { cameraScale, viewport } from './viewport';

/** Boots LittleJS and opens the main menu. */
export function startGame(config: GameConfig): Promise<void> {
  const store = openSaveStore();
  const context: SceneContext = {
    config,
    loadSave: (): LoadOutcome =>
      store
        ? store.load((data) => isSaveCompatible(data, config.backyard.stations))
        : { kind: 'failed', reason: 'storageUnavailable', backedUp: false },
    persist: (data) => store?.save(data) ?? false,
    goTo: (request) => {
      scenes.goTo(request);
    },
  };
  const scenes = new SceneManager((request) => createScene(request, context));

  function gameInit(): void {
    setCanvasFixedSize(vec2(viewport.canvasWidthPx, viewport.canvasHeightPx));
    setCameraScale(cameraScale);
    setCameraPos(vec2(viewport.worldWidth / 2, viewport.worldHeight / 2));
    applyTheme();

    scenes.goTo({ kind: 'mainMenu' });

    // Phones often kill a backgrounded app without warning, so save whenever the page is hidden.
    document.addEventListener('visibilitychange', () => {
      if (document.visibilityState === 'hidden') {
        scenes.pageHidden();
      }
    });
  }

  return engineInit(
    gameInit,
    () => {
      scenes.update();
    },
    () => undefined,
    () => {
      scenes.render();
    },
    () => {
      scenes.renderPost();
    },
  );
}

function createScene(request: SceneRequest, context: SceneContext): Scene {
  switch (request.kind) {
    case 'mainMenu':
      return new MainMenuScene(context);
    case 'backyard':
      return new BackyardScene(context, request.saved);
  }
}

/** Undefined if the browser blocks storage outright (even reading `localStorage` can throw). */
function openSaveStore(): SaveStore | undefined {
  try {
    return new SaveStore(window.localStorage);
  } catch (error) {
    console.error('Save storage is unavailable', error);
    return undefined;
  }
}
