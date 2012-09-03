using System;
using Microsoft.Xna.Framework;

namespace TccLib.Xna.Framework
{
    /// <summary>
    /// Defines a simple interface for a behavior which contains game logic for a component.
    /// </summary>
    public interface IBehavior
    {
        /// <summary>
        /// Performs any required updating as required by the behavior.
        /// </summary>
        /// <param name="gameTime">The current GameTime for the game.</param>
        void Update(GameTime gameTime);
    }
}
