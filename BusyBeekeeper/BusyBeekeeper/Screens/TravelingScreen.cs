using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BusyBeekeeper.Screens
{
    internal class TravelingScreen : GameScreenBase
    {
        private readonly IGameScreen mNextScreen;

        private Texture2D mBlankTexture;
        private SpriteFont mFont;

        public TravelingScreen(IGameScreen gameScreen)
        {
            this.mNextScreen = gameScreen;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            this.mBlankTexture = this.ContentManager.Load<Texture2D>("Sprites/Blank");
            this.mFont = this.ContentManager.Load<SpriteFont>("Fonts/DefaultBig");
        }

        public void TravelingComplete()
        {
            this.ScreenManager.TransitionTo(this.mNextScreen);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);

            const string lcTravelingText = "Traveling...";
            var lTravelingTextSize = this.mFont.MeasureString(lcTravelingText);
            var lTravelingTextPosition = (this.ScreenSize - lTravelingTextSize) / 2f;

            spriteBatch.Draw(this.mBlankTexture, Vector2.Zero, null, Color.Black, 0, Vector2.Zero, this.ScreenSize, SpriteEffects.None, 0);
            spriteBatch.DrawString(this.mFont, lcTravelingText, lTravelingTextPosition, Color.White);
        }
    }
}
