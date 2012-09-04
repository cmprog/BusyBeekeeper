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
        /// <param name="textureProperty">The property containing the texture to draw.</param>
        /// <param name="positionProperty">The property containing the position to draw.</param>
        public BasicRenderer(
            ISharedProperty<Texture2D> textureProperty,
            ISharedProperty<Vector2> positionProperty)
            : this(textureProperty, positionProperty, SharedProperty.Create(Color.White))
        {
        }

        /// <summary>
        /// Initializes a new instance of the BasicRenderer class.
        /// </summary>
        /// <param name="textureProperty">The property containing the texture to draw.</param>
        /// <param name="positionProperty">The property containing the position to draw.</param>
        /// <param name="colorProperty">The property containing the color to tint the texture.</param>
        public BasicRenderer(
            ISharedProperty<Texture2D> textureProperty,
            ISharedProperty<Vector2> positionProperty,
            ISharedProperty<Color> colorProperty)
        {
            System.Diagnostics.Debug.Assert(positionProperty != null, "Cannot work with a null position property.");
            System.Diagnostics.Debug.Assert(textureProperty != null, "Cannot work with a null texture property.");
            System.Diagnostics.Debug.Assert(colorProperty != null, "Cannot work with a null color property.");

            this.PositionProperty = positionProperty;
            this.TextureProperty = textureProperty;
            this.ColorProperty = colorProperty;
        }
        
        /// <summary>
        /// Gets the color to tint the sprite.
        /// </summary>
        protected ISharedProperty<Color> ColorProperty { get; private set; }

        /// <summary>
        /// Gets the property containing the position to draw the texture.
        /// </summary>
        protected ISharedProperty<Vector2> PositionProperty { get; private set; }

        /// <summary>
        /// Gets the property containing the texture to draw.
        /// </summary>
        protected ISharedProperty<Texture2D> TextureProperty { get; private set; }

        /// <summary>
        /// Renders the texture simply be drawing it at the position with no additional tint.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used to draw draw sprites.</param>
        /// <param name="gameTime">The current game time.</param>
        public virtual void Render(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(this.TextureProperty.Value, this.PositionProperty.Value, Color.White);
        }
    }
}
