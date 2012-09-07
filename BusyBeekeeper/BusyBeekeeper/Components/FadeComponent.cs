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
    /// Defines a basic component for performing fade transitions.
    /// </summary>
    public class FadeComponent : Component
    {
        /// <summary>
        /// Initializes a new instance of the FafeComponent class.
        /// </summary>
        public FadeComponent()
        {
            this.SizeProperty = SharedProperty.Create(Vector2.Zero);
            this.PositionProperty = SharedProperty.Create(Vector2.Zero);
            this.FadeAmount = SharedProperty.Create(1f);
            this.FadeColor = SharedProperty.Create(Color.Black);
        }

        /// <summary>
        /// Gets the property containing the size of this component.
        /// </summary>
        public SharedProperty<Vector2> SizeProperty { get; private set; }

        /// <summary>
        /// Gets the property containing the position of this component.
        /// </summary>
        public SharedProperty<Vector2> PositionProperty { get; private set; }

        /// <summary>
        /// Gets the property containing the amout to fade where
        /// 0 represents no alpha, 1 represents full alpha.
        /// </summary>
        public SharedProperty<float> FadeAmount { get; private set; }

        /// <summary>
        /// Gets the property containing the base color to fade.
        /// </summary>
        public SharedProperty<Color> FadeColor { get; private set; }

        /// <summary>
        /// Gets or sets the renderer responsible for rendering this component.
        /// </summary>
        public IRenderer Renderer { get; set; }
    }
}
