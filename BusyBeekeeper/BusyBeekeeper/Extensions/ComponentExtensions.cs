using Microsoft.Xna.Framework;
using TccLib.Xna.Framework;

namespace BusyBeekeeper.Extensions
{
    /// <summary>
    /// A collection of extension methods for the Component class.
    /// </summary>
    public static class ComponentExtensions
    {
        /// <summary>
        /// Updates all the behaviors of the given Component.
        /// </summary>
        /// <param name="component">The Component to update.</param>
        /// <param name="gameTime">The current GameTime.</param>
        public static void Update(this Component component, GameTime gameTime)
        {
            foreach (var behavior in component.Behaviors)
            {
                behavior.Update(gameTime);
            }
        }
    }
}
