using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TccLib.Xna.Framework.Renderers
{
    /// <summary>
    /// A simple implementation of the IRenderer interface which draws a texture to
    /// a given position.
    /// </summary>
    public class BasicRenderer : IRenderer
    {
        /// <summary>
        /// Initializes a new instance of the BasicRenderer class.
        /// </summary>
        /// <param name="positionProperty">The property containing the position to draw.</param>
        /// <param name="textureProperty">The property containing the texture to draw.</param>
        public BasicRenderer(ISharedProperty<Vector2> positionProperty, ISharedProperty<Texture2D> textureProperty)
        {
            System.Diagnostics.Debug.Assert(positionProperty != null, "Cannot work with a null position property.");
            System.Diagnostics.Debug.Assert(textureProperty != null, "Cannot work with a null texture property.");

            this.PositionProperty = positionProperty;
            this.TextureProperty = textureProperty;
        }

        /// <summary>
        /// Gets or sets the property containing the position to draw the texture.
        /// </summary>
        private ISharedProperty<Vector2> PositionProperty { get; set; }

        /// <summary>
        /// Gets or sets the property containing the texture to draw.
        /// </summary>
        private ISharedProperty<Texture2D> TextureProperty { get; set; }

        /// <summary>
        /// Renders the texture simply be drawing it at the position with no additional tint.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used to draw draw sprites.</param>
        /// <param name="gameTime">The current game time.</param>
        public void Render(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(this.TextureProperty.Value, this.PositionProperty.Value, Color.White);
        }
    }
}
