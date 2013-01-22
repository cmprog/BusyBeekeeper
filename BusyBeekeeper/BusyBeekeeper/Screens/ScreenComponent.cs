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
        /// Handles the given input state.
        /// </summary>
        /// <param name="inputState">The input state.</param>
        public virtual void HandleInput(InputState inputState) { }

        /// <summary>
        /// Draws the component.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime) { }
    }
}
