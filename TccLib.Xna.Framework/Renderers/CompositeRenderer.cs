using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TccLib.Xna.Framework.Renderers
{
    /// <summary>
    /// This renderer renders things by calling multiple other renderers in order.
    /// </summary>
    public class CompositeRenderer : IRenderer
    {
        /// <summary>
        /// Initializes a new instance of the CompositeRenderer class.
        /// </summary>
        /// <param name="renderers">The renderers the compose.</param>
        public CompositeRenderer(IEnumerable<IRenderer> renderers)
        {
            System.Diagnostics.Debug.Assert(renderers != null, "Must have a non-null collection of renderers.");

            this.Renderers = renderers;
        }

        /// <summary>
        /// Initializes a new instance of the CompositeRenderer class.
        /// </summary>
        /// <param name="renderers">The renderers the compose.</param>
        public CompositeRenderer(params IRenderer[] renderers)
            : this((IEnumerable<IRenderer>)renderers)
        {
        }

        /// <summary>
        /// Gets or sets the collection of renderers which make up this renderer.
        /// </summary>
        private IEnumerable<IRenderer> Renderers { get; set; }

        /// <summary>
        /// Renders something by calling all the IRenderer objects which make us up.
        /// Hopefully they will make something pretty.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used to draw draw sprites.</param>
        /// <param name="gameTime">The current game time.</param>
        public void Render(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var renderer in this.Renderers)
            {
                renderer.Render(spriteBatch, gameTime);
            }
        }
    }
}
