using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BusyBeekeeper.Screens.Components
{
    internal interface IBackgroundRenderer
    {
        void Draw(SpriteBatch spriteBatch, GameTime gameTime, Vector2 position, Vector2 size);
    }
}
