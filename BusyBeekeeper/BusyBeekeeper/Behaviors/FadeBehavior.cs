using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TccLib.Xna.Framework;

namespace BusyBeekeeper.Behaviors
{
    /// <summary>
    /// This behaviors updates a color property based on the a float property representing
    /// amount of alpha to introduce.
    /// </summary>
    public class FadeBehavior : IBehavior
    {
        /// <summary>
        /// Initializes a new instance of the FadeBehavior class.
        /// </summary>
        /// <param name="colorProperty">A property containing the base color to update.</param>
        /// <param name="alphaProperty">A property containing the alpha properties of the color.</param>
        public FadeBehavior(
            ISharedProperty<Color> colorProperty,
            ISharedProperty<float> alphaProperty)
        {
            this.ColorProperty = colorProperty;
            this.AlphaProperty = alphaProperty;
        }

        /// <summary>
        /// Gets or sets the property containing the color to work with.
        /// </summary>
        private ISharedProperty<Color> ColorProperty { get; set; }

        /// <summary>
        /// Gets or sets the property containing the amount of alpha to introduce
        /// 0 represents no alpha, 1 represents full alpha.
        /// </summary>
        private ISharedProperty<float> AlphaProperty { get; set; }

        /// <summary>
        /// Updates the color by setting it equal to the same color with a change in alpha.
        /// </summary>
        /// <param name="gameTime">The current GameTime.</param>
        public void Update(GameTime gameTime)
        {
            this.ColorProperty.Value = new Color(
                this.ColorProperty.Value.R,
                this.ColorProperty.Value.G,
                this.ColorProperty.Value.B,
                (int)(this.AlphaProperty.Value * 255));
        }
    }
}
