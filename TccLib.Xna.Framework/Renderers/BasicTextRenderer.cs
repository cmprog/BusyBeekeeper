using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TccLib.Xna.Framework.Renderers
{
    /// <summary>
    /// This simple renderer draws text at given location.
    /// </summary>
    public class BasicTextRenderer : IRenderer
    {
        /// <summary>
        /// Initializes a new instance of the BasicTextRenderer class.
        /// </summary>
        /// <param name="textProperty">A property containing the text to draw.</param>
        /// <param name="fontProperty">A property containing the font to draw with.</param>
        /// <param name="positionProperty">A property containing the position to draw at.</param>
        public BasicTextRenderer(
            ISharedProperty<string> textProperty,
            ISharedProperty<SpriteFont> fontProperty,
            ISharedProperty<Vector2> positionProperty)
            : this(textProperty, fontProperty, positionProperty, SharedProperty.Create(Color.Black))
        {
        }

        /// <summary>
        /// Initializes a new instance of the BasicTextRenderer class.
        /// </summary>
        /// <param name="textProperty">A property containing the text to draw.</param>
        /// <param name="fontProperty">A property containing the font to draw with.</param>
        /// <param name="positionProperty">A property containing the position to draw at.</param>
        /// <param name="colorProperty">A property containing the Color to draw the text.</param>
        public BasicTextRenderer(
           ISharedProperty<string> textProperty,
           ISharedProperty<SpriteFont> fontProperty,
           ISharedProperty<Vector2> positionProperty,
           ISharedProperty<Color> colorProperty)
        {
            System.Diagnostics.Debug.Assert(textProperty != null, "Must provide a non-null textProperty.");
            System.Diagnostics.Debug.Assert(fontProperty != null, "Must provide a non-null fontProperty.");
            System.Diagnostics.Debug.Assert(positionProperty != null, "Must provide a non-null positionProperty.");
            System.Diagnostics.Debug.Assert(colorProperty != null, "Must provide a non-null colorProperty.");

            this.FontProperty = fontProperty;
            this.TextProperty = textProperty;
            this.PositionProperty = positionProperty;
            this.ColorProperty = colorProperty;
        }

        /// <summary>
        /// Gets the property containing the SpriteFont used to draw the text.
        /// </summary>
        protected ISharedProperty<SpriteFont> FontProperty { get; private set; }

        /// <summary>
        /// Gets the property containing the text to draw.
        /// </summary>
        protected ISharedProperty<string> TextProperty { get; private set; }

        /// <summary>
        /// Gets the property containing the Vector2 describing the position to draw the text.
        /// </summary>
        protected ISharedProperty<Vector2> PositionProperty { get; private set; }

        /// <summary>
        /// Gets the property containing the Color to ting the text.
        /// </summary>
        protected ISharedProperty<Color> ColorProperty { get; private set; }

        /// <summary>
        /// Renders the text by drawing it at the given position.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used to draw draw sprites.</param>
        /// <param name="gameTime">The current game time.</param>
        public void Render(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.DrawString(
                this.FontProperty.Value,
                this.TextProperty.Value, 
                this.PositionProperty.Value, 
                this.ColorProperty.Value);
        }
    }
}
