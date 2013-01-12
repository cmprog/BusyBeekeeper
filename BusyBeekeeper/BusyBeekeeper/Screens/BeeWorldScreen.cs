using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BusyBeekeeper.Screens.Components;
using BusyBeekeeper.Data;
using BusyBeekeeper.Data.Graphics.BeeWorld;

namespace BusyBeekeeper.Screens
{
    internal sealed class BeeWorldScreen : GameScreenBase
    {
        private Texture2D mBlankTexture;

        private Button[] mBeeYardNameButtons;

        private Color mYardColor = Color.Blue;
        private BeeYardWorldInfo[] mYardInfos;

        public override void LoadContent()
        {
            base.LoadContent();
            this.mBlankTexture = this.ContentManager.Load<Texture2D>("Sprites/Blank");
            this.mYardInfos = this.ContentManager.Load<BeeYardWorldInfo[]>("GraphicsData/BeeWorld/YardLocations");
            var lBeeYards = this.ScreenManager.Player.BeeYards;

            var lBlueBackgroundRenderer = new SolidBackgroundRenderer(this.mBlankTexture, Color.Blue);
            
            this.mBeeYardNameButtons = new Button[this.mYardInfos.Length];
            for (int lIndex = 0; lIndex < this.mYardInfos.Length; lIndex++)
            {
                var lYardInfo = this.mYardInfos[lIndex];
                var lBeeYard = lBeeYards[lYardInfo.Id];

                var lButton = new Button();
                lButton.Font = this.ContentManager.Load<SpriteFont>("Fonts/DefaultSmall");
                lButton.TextColor = Color.White;
                lButton.BackgroundRenderer = lBlueBackgroundRenderer;
                lButton.Tag = lYardInfo.Id;
                lButton.Size = lYardInfo.NameSize;
                lButton.Position = lYardInfo.NamePosition;
                lButton.Text = lBeeYard.Name;

                this.mBeeYardNameButtons[lIndex] = lButton;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach (var lButton in this.mBeeYardNameButtons) lButton.Update(gameTime);
        }

        public override void HandleInput(InputState inputState)
        {
            base.HandleInput(inputState);

            if (inputState.MouseLeftClickUp())
            {
                var lCurrentMouseState = inputState.CurrentMouseState;
                var lClickedButton = this.mBeeYardNameButtons
                    .FirstOrDefault(x => x.HitTest(lCurrentMouseState.X, lCurrentMouseState.Y));

                if (lClickedButton != null)
                {
                    var lYardId = (int)lClickedButton.Tag;
                    var lPlayer = this.ScreenManager.Player;
                    var lBeeYard = lPlayer.BeeYards[lYardId];
                    lPlayer.TravelTo(lBeeYard);

                    var lBeeYardScreen = new BeeYardScreen();
                    this.ScreenManager.TransitionTo(lBeeYardScreen);
                    return;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);            
            spriteBatch.Draw(this.mBlankTexture, Vector2.Zero, null, Color.Black, 0, Vector2.Zero, this.ScreenSize, SpriteEffects.None, 0);
            foreach (var lButton in this.mBeeYardNameButtons) lButton.Draw(spriteBatch, gameTime);
        }
    }
}
