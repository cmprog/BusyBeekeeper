using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TccLib.Xna.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BusyBeekeeper.Components
{
    /// <summary>
    /// Defines a simple menu button. Displays text on a background really.
    /// </summary>
    public class MenuButtonComponent : Component
    {
        /// <summary>
        /// Initializes a new instance of the MenuButtomComponent class.
        /// </summary>
        public MenuButtonComponent()
        {
            this.TextProperty = SharedProperty.Create(string.Empty);
            this.SizeProperty = SharedProperty.Create(new Vector2(50, 100));
            this.PositionProperty = SharedProperty.Create(Vector2.Zero);
        }

        /// <summary>
        /// Gets the property containing the text 
        /// </summary>
        public SharedProperty<string> TextProperty { get; private set; }
        public SharedProperty<Vector2> SizeProperty { get; private set; }
        public SharedProperty<Vector2> PositionProperty { get; private set; }
        public SharedProperty<SpriteFont> FontProperty { get; private set; }

        /// <summary>
        /// The renderer responsible for rendering the menu button.
        /// </summary>
        public IRenderer Renderer { get; set; }
    }
}
