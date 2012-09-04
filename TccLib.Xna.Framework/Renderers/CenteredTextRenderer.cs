using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TccLib.Xna.Framework.Renderers
{
    /// <summary>
    /// This simple renderer renders text centered in a given bounds. The bounds
    /// is specified by a position and a size.
    /// </summary>
    public class CenteredTextRenderer : BasicTextRenderer, IRenderer
    {
        /// <summary>
        /// Initializes a new instance of the CenteredTextRenderer class.
        /// </summary>
        /// <param name="textProperty">A property containing the text to draw.</param>
        /// <param name="fontProperty">A property containing the font to draw with.</param>
        /// <param name="positionProperty">A property containing the position to draw at.</param>
        /// <param name="sizeProperty">A property containing the size of the bounding rectangle.</param>
        public CenteredTextRenderer(
            ISharedProperty<string> textProperty,
            ISharedProperty<SpriteFont> fontProperty,
            ISharedProperty<Vector2> positionProperty,
            ISharedProperty<Vector2> sizeProperty)
            : this(textProperty, fontProperty, positionProperty, sizeProperty, SharedProperty.Create(Color.Black))
        {
        }

        /// <summary>
        /// Initializes a new instance of the CenteredTextRenderer class.
        /// </summary>
        /// <param name="textProperty">A property containing the text to draw.</param>
        /// <param name="fontProperty">A property containing the font to draw with.</param>
        /// <param name="positionProperty">A property containing the position to draw at.</param>
        /// <param name="sizeProperty">A property containing the size of the bounding rectangle.</param>
        /// <param name="colorProperty">A property containing the Color to draw the text.</param>
        public CenteredTextRenderer(
           ISharedProperty<string> textProperty,
           ISharedProperty<SpriteFont> fontProperty,
           ISharedProperty<Vector2> positionProperty,
           ISharedProperty<Vector2> sizeProperty,
           ISharedProperty<Color> colorProperty)
            : base(textProperty, fontProperty, positionProperty, colorProperty)
        {
            System.Diagnostics.Debug.Assert(sizeProperty != null, "Must provide a non-null sizeProperty.");

            this.SizeProperty = sizeProperty;
        }

        /// <summary>
        /// Gets or sets the property containing the Vector2 describing the size of the rectangular bounds
        /// in which to center the text.
        /// </summary>
        private ISharedProperty<Vector2> SizeProperty { get; set; }

        /// <summary>
        /// Renders the text by drawing it at the given position.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used to draw draw sprites.</param>
        /// <param name="gameTime">The current game time.</param>
        public void Render(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var textSize = this.FontProperty.Value.MeasureString(this.TextProperty.Value);

            spriteBatch.DrawString(
                this.FontProperty.Value,
                this.TextProperty.Value,
                this.PositionProperty.Value + ((this.SizeProperty.Value - textSize) / 2f),
                this.ColorProperty.Value);
        }
    }
}
