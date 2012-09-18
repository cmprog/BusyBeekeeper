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
    /// The avatar selection component is basically just a bordered texture. It
    /// does have extra state which keeps track of whether it is selected or not.
    /// </summary>
    public class AvatarSelectionComponent : Component
    {
        /// <summary>
        /// Initializes a new instance of the TextButtonComponent class.
        /// </summary>
        public AvatarSelectionComponent()
        {
            this.TextureProperty = SharedProperty.Create(default(Texture2D));
            this.SizeProperty = SharedProperty.Create(new Vector2(50, 100));
            this.PositionProperty = SharedProperty.Create(Vector2.Zero);
            this.IsSelectedProperty = NotifyingSharedProperty.Create(this.MessageDispatcher, "IsSelectedProperty", false);
            this.BorderColorProperty = SharedProperty.Create(Color.Black);
        }

        /// <summary>
        /// Gets the property containing the text displayed in the button.
        /// </summary>
        public SharedProperty<Texture2D> TextureProperty { get; private set; }

        /// <summary>
        /// Gets the property containing the size of the button.
        /// </summary>
        public SharedProperty<Vector2> SizeProperty { get; private set; }

        /// <summary>
        /// Gets the property containing the position of the button.
        /// </summary>
        public SharedProperty<Vector2> PositionProperty { get; private set; }

        /// <summary>
        /// Gets the property containing the flag indicating whether this avatar is
        /// selected or not.
        /// </summary>
        public SharedProperty<bool> IsSelectedProperty { get; private set; }

        /// <summary>
        /// Gets the property containing the color for the border of the avatar image.
        /// </summary>
        public SharedProperty<Color> BorderColorProperty { get; private set; }

        /// <summary>
        /// Gets or sets the renderer responsible for rendering the menu button.
        /// </summary>
        public IRenderer Renderer { get; set; }
    }
}
