using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TccLib.Xna.Framework;

namespace BusyBeekeeper.Components
{
    /// <summary>
    /// Defines a simple menu button. Displays text on a background really.
    /// </summary>
    public class TextButtonComponent : Component
    {
        /// <summary>
        /// Initializes a new instance of the TextButtonComponent class.
        /// </summary>
        public TextButtonComponent()
        {
            this.TextProperty = SharedProperty.Create(string.Empty);
            this.SizeProperty = SharedProperty.Create(new Vector2(50, 100));
            this.PositionProperty = SharedProperty.Create(Vector2.Zero);
            this.FontProperty = SharedProperty.Create(default(SpriteFont));
        }

        /// <summary>
        /// Gets the property containing the text displayed in the button.
        /// </summary>
        public SharedProperty<string> TextProperty { get; private set; }

        /// <summary>
        /// Gets the property containing the size of the button.
        /// </summary>
        public SharedProperty<Vector2> SizeProperty { get; private set; }

        /// <summary>
        /// Gets the property containing the position of the button.
        /// </summary>
        public SharedProperty<Vector2> PositionProperty { get; private set; }

        /// <summary>
        /// Gets the property containing the font for the button.
        /// </summary>
        public SharedProperty<SpriteFont> FontProperty { get; private set; }

        /// <summary>
        /// Gets or sets the renderer responsible for rendering the menu button.
        /// </summary>
        public IRenderer Renderer { get; set; }
    }
}
