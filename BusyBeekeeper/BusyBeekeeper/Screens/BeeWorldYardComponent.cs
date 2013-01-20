using System;
using BusyBeekeeper.Data;
using BusyBeekeeper.Data.Graphics.BeeWorld;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BusyBeekeeper.Screens
{
    internal sealed class BeeWorldYardComponent : ScreenComponent
    {
        private readonly BeeYard mBeeYard;
        private readonly BeeWorldYardInfo mWorldYardInfo;

        private Texture2D mBlankTexture;
        private SpriteFont mFont;

        public BeeWorldYardComponent(BeeYard beeYard, BeeWorldYardInfo worldYardInfo)
        {
            this.mBeeYard = beeYard;
            this.mWorldYardInfo = worldYardInfo;
        }

        public event Action<BeeWorldYardComponent> TravelToYard;

        public BeeYard BeeYard { get { return this.mBeeYard; } }
        public BeeWorldYardInfo WorldYardInfo { get { return this.mWorldYardInfo; } }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);

            this.mBlankTexture = contentManager.Load<Texture2D>("Sprites/Blank");
            this.mFont = contentManager.Load<SpriteFont>("Fonts/DefaultSmall");
        }

        public override bool HandleInput(InputState inputState)
        {
            if (inputState.MouseLeftClickUp())
            {
                if (!this.mBeeYard.IsUnlocked) return false;

                var lCurrentMouseState = inputState.CurrentMouseState;

                var lNameIsClicked = VectorUtilities.HitTest(
                    this.mWorldYardInfo.NamePosition, 
                    this.mWorldYardInfo.NameSize, 
                    lCurrentMouseState.X, lCurrentMouseState.Y);

                if (lNameIsClicked)
                {
                    this.TravelToYard(this);
                }

                return lNameIsClicked;
            }

            return base.HandleInput(inputState);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);

            var lNamePosition = this.mWorldYardInfo.NamePosition;
            var lNameSize = this.mWorldYardInfo.NameSize;

            var lBackColor = this.mBeeYard.IsUnlocked ? Color.Goldenrod : Color.LightGoldenrodYellow;
            spriteBatch.Draw(this.mBlankTexture, lNamePosition, null, lBackColor, 0, Vector2.Zero, lNameSize, SpriteEffects.None, 0);

            var lNameText = this.mBeeYard.Name;
            var lNameTextSize = this.mFont.MeasureString(lNameText);
            var lNameTextPosition = lNamePosition + ((lNameSize - lNameTextSize) / 2f);

            spriteBatch.DrawString(this.mFont, lNameText, lNameTextPosition, Color.Black);
        }
    }
}
