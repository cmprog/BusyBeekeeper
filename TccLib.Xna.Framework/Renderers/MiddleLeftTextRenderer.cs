using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TccLib.Xna.Framework.Renderers
{
    /// <summary>
    /// This simple renderer renders text which is vertically centered, but left aligned
    /// within a given bounds. A padding can be specified to which will be applied before
    /// the text is rendered.
    /// </summary>
    public class MiddleLeftTextRenderer : BasicTextRenderer
    {
        /// <summary>
        /// Initializes a new instance of the MiddleLeftTextRenderer class.
        /// </summary>
        /// <param name="textProperty">A property containing the text to draw.</param>
        /// <param name="fontProperty">A property containing the font to draw with.</param>
        /// <param name="positionProperty">A property containing the position to draw at.</param>
        /// <param name="sizeProperty">A property containing the size of the bounding rectangle.</param>
        public MiddleLeftTextRenderer(
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
        public MiddleLeftTextRenderer(
           ISharedProperty<string> textProperty,
           ISharedProperty<SpriteFont> fontProperty,
           ISharedProperty<Vector2> positionProperty,
           ISharedProperty<Vector2> sizeProperty,
           ISharedProperty<Color> colorProperty)
            : this(textProperty, fontProperty, positionProperty, sizeProperty, SharedProperty.Create(Color.Black), SharedProperty.Create(0.0f))
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
        public MiddleLeftTextRenderer(
           ISharedProperty<string> textProperty,
           ISharedProperty<SpriteFont> fontProperty,
           ISharedProperty<Vector2> positionProperty,
           ISharedProperty<Vector2> sizeProperty,
           ISharedProperty<Color> colorProperty,
           ISharedProperty<float> paddingProperty)
            : base(textProperty, fontProperty, positionProperty, colorProperty)
        {
            System.Diagnostics.Debug.Assert(sizeProperty != null, "Must provide a non-null sizeProperty.");
            System.Diagnostics.Debug.Assert(paddingProperty != null, "Must provide a non-null paddingProperty.");

            this.SizeProperty = sizeProperty;
            this.PaddingProperty = paddingProperty;
        }

        /// <summary>
        /// Gets or sets the property containing the Vector2 describing the size of the rectangular bounds
        /// in which to center the text.
        /// </summary>
        private ISharedProperty<Vector2> SizeProperty { get; set; }

        /// <summary>
        /// Gets or sets the property containing a value representing the amount of
        /// padding to apply before rendering the text.
        /// </summary>
        private ISharedProperty<float> PaddingProperty { get; set; }

        /// <summary>
        /// Renders the text by drawing it at the given position.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used to draw draw sprites.</param>
        /// <param name="gameTime">The current game time.</param>
        public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var textSize = this.FontProperty.Value.MeasureString(this.TextProperty.Value);

            var textPosition = new Vector2(
                this.PositionProperty.Value.X + this.PaddingProperty.Value,
                this.PositionProperty.Value.Y + ((this.SizeProperty.Value.Y - textSize.Y) / 2f));

            spriteBatch.DrawString(
                this.FontProperty.Value,
                this.TextProperty.Value,
                textPosition,
                this.ColorProperty.Value);
        }
    }
}
