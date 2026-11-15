import { engineInit, setCameraPos, setCameraScale, setCanvasFixedSize, vec2 } from 'littlejsengine';
import type { GameConfig } from '../config/GameConfig';
import { isSaveCompatible } from '../sim/save/isSaveCompatible';
import { BackyardScene } from './BackyardScene';
import { SaveStore } from './SaveStore';
import type { LoadOutcome } from './SaveStore';
import { applyTheme } from './theme';
import { cameraScale, viewport } from './viewport';

/** Boots LittleJS, restores the save, and runs the backyard scene (the only scene until Phase 2). */
export function startGame(config: GameConfig): Promise<void> {
  const store = openSaveStore();
  const outcome: LoadOutcome = store
    ? store.load((data) => isSaveCompatible(data, config.backyard.stations))
    : { kind: 'failed', reason: 'storageUnavailable', backedUp: false };
  let scene: BackyardScene | undefined;

  function gameInit(): void {
    setCanvasFixedSize(vec2(viewport.canvasWidthPx, viewport.canvasHeightPx));
    setCameraScale(cameraScale);
    setCameraPos(vec2(viewport.worldWidth / 2, viewport.worldHeight / 2));
    applyTheme();

    scene = new BackyardScene(config, {
      saved: outcome.kind === 'loaded' ? outcome.data : undefined,
      persist: (data) => store?.save(data) ?? false,
    });
    const notice = loadNotice(outcome);
    if (notice) {
      scene.showNotice(notice);
    }

    // Phones often kill a backgrounded app without warning, so save whenever the page is hidden.
    document.addEventListener('visibilitychange', () => {
      if (document.visibilityState === 'hidden') {
        scene?.saveNow();
      }
    });
  }

  return engineInit(
    gameInit,
    () => scene?.update(),
    () => undefined,
    () => scene?.render(),
    () => scene?.renderPost(),
  );
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

function loadNotice(outcome: LoadOutcome): string | undefined {
  if (outcome.kind !== 'failed') {
    return undefined;
  }
  if (outcome.reason === 'storageUnavailable') {
    return "Saving isn't available in this browser, so progress won't be kept.";
  }
  const backup = outcome.backedUp ? ' A backup copy was kept.' : '';
  return `Your save couldn't be loaded, so a new game started.${backup}`;
}
