using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TccLib.Xna.Framework.Renderers
{
    /// <summary>
    /// This renderer will draw a simple border around a rectangular bounds.
    /// </summary>
    public class SquareBorderRenderer : IRenderer
    {
        /// <summary>
        /// Initializes a new instance of the SquareBorderRenderer class.
        /// </summary>
        /// <param name="textureProperty">The property containing the texture to draw.</param>
        /// <param name="positionProperty">The property containing the position to draw.</param>
        public SquareBorderRenderer(
            ISharedProperty<Texture2D> textureProperty,
            ISharedProperty<Vector2> positionProperty,
            ISharedProperty<Vector2> sizeProperty)
            : this(textureProperty, positionProperty, sizeProperty, SharedProperty.Create(Color.White))
        {
        }

        /// <summary>
        /// Initializes a new instance of the BasicRenderer class.
        /// </summary>
        /// <param name="textureProperty">The property containing the texture to draw.</param>
        /// <param name="positionProperty">The property containing the position to draw.</param>
        /// <param name="colorProperty">The property containing the color to tint the texture.</param>
        public SquareBorderRenderer(
            ISharedProperty<Texture2D> textureProperty,
            ISharedProperty<Vector2> positionProperty,
            ISharedProperty<Vector2> sizeProperty,
            ISharedProperty<Color> colorProperty)
        {
            System.Diagnostics.Debug.Assert(positionProperty != null, "Cannot work with a null position property.");
            System.Diagnostics.Debug.Assert(textureProperty != null, "Cannot work with a null texture property.");
            System.Diagnostics.Debug.Assert(sizeProperty != null, "Cannot work with a null size property.");
            System.Diagnostics.Debug.Assert(colorProperty != null, "Cannot work with a null color property.");

            this.PositionProperty = positionProperty;
            this.SizeProperty = sizeProperty;
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
        /// Gets the property containing the size to draw the border.
        /// </summary>
        protected ISharedProperty<Vector2> SizeProperty { get; private set; }

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
            var texture = this.TextureProperty.Value;
            var position = this.PositionProperty.Value;
            var size = this.SizeProperty.Value;
            var color = this.ColorProperty.Value;

            var horizontalScale = new Vector2(size.X, 1);
            var verticalScale = new Vector2(1, size.Y);

            // Top
            spriteBatch.Draw(texture, position, null, color, 0, Vector2.Zero, horizontalScale, SpriteEffects.None, 1);

            // Left
            spriteBatch.Draw(texture, position, null, color, 0, Vector2.Zero, verticalScale, SpriteEffects.None, 1);

            // Right
            spriteBatch.Draw(texture, position + (Vector2.UnitX * size.X), null, color, 0, Vector2.Zero, verticalScale, SpriteEffects.None, 1);

            // Bottom
            spriteBatch.Draw(texture, position + (Vector2.UnitY * size.Y), null, color, 0, Vector2.Zero, horizontalScale, SpriteEffects.None, 1);
        }
    }
}
