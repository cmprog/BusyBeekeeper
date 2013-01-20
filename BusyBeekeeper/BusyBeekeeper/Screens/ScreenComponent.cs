using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BusyBeekeeper.Screens
{
    internal abstract class ScreenComponent
    {
        public object Tag { get; set; }

        public virtual void LoadContent(ContentManager contentManager) { }

        /// <summary>
        /// Allows the component a chance to update.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime) { }

        /// <summary>
        /// Handles the given input state. If this component handles the input
        /// in a way which leaves the input considered "consumed" then this method
        /// will return true, otherwise it returns false. Input is considered "consumed"
        /// when the action in responce to the input makes it meaningless for other
        /// components to handle the input.
        /// </summary>
        /// <param name="inputState">The input state.</param>
        /// <returns>True if the input was fully consumed, otherwise false.</returns>
        public virtual bool HandleInput(InputState inputState) { return false; }

        /// <summary>
        /// Draws the component.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime) { }
    }
}
