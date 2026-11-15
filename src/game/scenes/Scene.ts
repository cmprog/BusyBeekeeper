/** A screen of the game. The SceneManager forwards the engine's loop to the current scene. */
export interface Scene {
  /** Called from gameUpdate (fixed 60 Hz). */
  update(): void;
  /** Called from gameRender: everything in world space. */
  render(): void;
  /** Called from gameRenderPost: the HUD, in screen space. */
  renderPost(): void;
  /** Called when the page is hidden (tab switched, app backgrounded): a chance to save. */
  onPageHidden?(): void;
}
