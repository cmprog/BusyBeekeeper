using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TccLib.Xna.Framework
{
    /// <summary>
    /// Defines a simple interface for rendering components.
    /// </summary>
    public interface IRenderer
    {
        /// <summary>
        /// Renders something using the given sprite batch and the gime time provided.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used to draw draw sprites.</param>
        /// <param name="gameTime">The current game time.</param>
        void Render(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
