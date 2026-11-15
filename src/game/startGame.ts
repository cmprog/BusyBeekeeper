import { engineInit, setCameraPos, setCameraScale, setCanvasFixedSize, vec2 } from 'littlejsengine';
import type { GameConfig } from '../config/GameConfig';
import { BackyardScene } from './BackyardScene';
import { applyTheme } from './theme';
import { cameraScale, viewport } from './viewport';

/** Boots LittleJS and runs the backyard scene (the only scene until Phase 2). */
export function startGame(config: GameConfig): Promise<void> {
  let scene: BackyardScene | undefined;

  function gameInit(): void {
    setCanvasFixedSize(vec2(viewport.canvasWidthPx, viewport.canvasHeightPx));
    setCameraScale(cameraScale);
    setCameraPos(vec2(viewport.worldWidth / 2, viewport.worldHeight / 2));
    applyTheme();
    scene = new BackyardScene(config);
  }

  return engineInit(
    gameInit,
    () => scene?.update(),
    () => undefined,
    () => scene?.render(),
    () => scene?.renderPost(),
  );
}
