import type { Scene } from './Scene';
import type { SceneRequest } from './SceneRequest';

/**
 * Owns the current scene and forwards the engine loop to it. A switch is applied at the start
 * of the next update, so the tap that asked for it can't also land in the new scene, and a
 * frame never renders half one scene and half another. No transitions yet.
 */
export class SceneManager {
  readonly #createScene: (request: SceneRequest) => Scene;
  #current: Scene | undefined;
  #pending: SceneRequest | undefined;

  constructor(createScene: (request: SceneRequest) => Scene) {
    this.#createScene = createScene;
  }

  goTo(request: SceneRequest): void {
    this.#pending = request;
  }

  update(): void {
    if (this.#pending) {
      const request = this.#pending;
      this.#pending = undefined;
      this.#current = this.#createScene(request);
      // Skip this frame's update: its input belongs to the scene that asked for the switch.
      return;
    }
    this.#current?.update();
  }

  render(): void {
    this.#current?.render();
  }

  renderPost(): void {
    this.#current?.renderPost();
  }

  pageHidden(): void {
    this.#current?.onPageHidden?.();
  }
}
