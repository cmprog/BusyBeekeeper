namespace TccLib.Xna.Framework
{
    /// <summary>
    /// Defines an interface for an object which has a renderer.
    /// </summary>
    public interface IRenderable
    {
        /// <summary>
        /// Gets or sets the renderer responsible for rendering the menu button.
        /// </summary>
        IRenderer Renderer { get; }
    }
}
