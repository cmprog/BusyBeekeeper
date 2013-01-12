using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BusyBeekeeper
{
    internal interface IGameScreen
    {
        IGameScreenManager ScreenManager { get; set; }

        void LoadContent();
        void UnloadContent();

        void Update(GameTime gameTime);
        void HandleInput(InputState inputState);
        void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
