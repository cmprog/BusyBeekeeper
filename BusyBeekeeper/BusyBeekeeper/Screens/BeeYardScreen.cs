using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BusyBeekeeper.Screens.Components;

namespace BusyBeekeeper.Screens
{
    internal sealed class BeeYardScreen : GameScreenBase
    {
        private Texture2D mBlankTexture;
        private Button mButtonBeeWorld = new Button();

        public override void LoadContent()
        {
            base.LoadContent();

            this.mBlankTexture = this.ContentManager.Load<Texture2D>("Sprites/Blank");

            this.mButtonBeeWorld.Font = this.ContentManager.Load<SpriteFont>("Fonts/DefaultSmall");
            this.mButtonBeeWorld.Text = "Travel";
            this.mButtonBeeWorld.TextColor = Color.White;
            this.mButtonBeeWorld.BackgroundRenderer = new SolidBackgroundRenderer(this.mBlankTexture, Color.Blue);
            this.mButtonBeeWorld.Size = new Vector2(75, 30);
            this.mButtonBeeWorld.Position = new Vector2(5, 5);
        }

        public override void HandleInput(InputState inputState)
        {
            base.HandleInput(inputState);

            if (inputState.MouseLeftClickUp())
            {
                var lCurrentMouseState = inputState.CurrentMouseState;
                if (this.mButtonBeeWorld.HitTest(lCurrentMouseState.X, lCurrentMouseState.Y))
                {
                    this.ScreenManager.TransitionTo(new BeeWorldScreen());
                    return;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
            spriteBatch.Draw(this.mBlankTexture, Vector2.Zero, null, Color.Black, 0, Vector2.Zero, this.ScreenSize, SpriteEffects.None, 0);
            this.mButtonBeeWorld.Draw(spriteBatch, gameTime);
        }
    }
}
