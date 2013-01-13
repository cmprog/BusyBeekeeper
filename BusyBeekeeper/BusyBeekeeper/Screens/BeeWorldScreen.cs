using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BusyBeekeeper.Screens.CommonComponents;
using BusyBeekeeper.Data;
using BusyBeekeeper.Data.Graphics.BeeWorld;

namespace BusyBeekeeper.Screens
{
    internal sealed class BeeWorldScreen : GameScreenBase
    {
        private Texture2D mBlankTexture;

        private BeeWorldYardComponent[] mYardComponents;
        private BeeWorldHudComponent mHudComponent;

        public override void LoadContent()
        {
            base.LoadContent();
            this.mBlankTexture = this.ContentManager.Load<Texture2D>("Sprites/Blank");

            var lYardInfos = this.ContentManager.Load<BeeYardWorldInfo[]>("GraphicsData/BeeWorld/YardLocations");
            var lBeeYards = this.ScreenManager.Player.BeeYards;

            var lBlueBackgroundRenderer = new SolidBackgroundRenderer(this.mBlankTexture, Color.Blue);

            this.mYardComponents = new BeeWorldYardComponent[lYardInfos.Length];
            for (int lIndex = 0; lIndex < lYardInfos.Length; lIndex++)
            {
                var lYardInfo = lYardInfos[lIndex];
                var lBeeYard = lBeeYards[lYardInfo.Id];

                var lYardComponent = new BeeWorldYardComponent(lBeeYard, lYardInfo);
                lYardComponent.LoadContent(this.ContentManager);
                lYardComponent.TravelToYard += this.YardComponent_TravelToYard;

                this.mYardComponents[lIndex] = lYardComponent;
            }

            this.mHudComponent = new BeeWorldHudComponent(this.ScreenManager.BeeWorldManager, this.ScreenSize);
            this.mHudComponent.LoadContent(this.ContentManager);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (var lYardComponent in this.mYardComponents) lYardComponent.Update(gameTime);
            this.mHudComponent.Update(gameTime);
        }

        public override void HandleInput(InputState inputState)
        {
            base.HandleInput(inputState);

            foreach (var lYardComponent in this.mYardComponents)
            {
                if (lYardComponent.HandleInput(inputState)) return;
            }

            if (this.mHudComponent.HandleInput(inputState)) return;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
            
            spriteBatch.Draw(this.mBlankTexture, Vector2.Zero, null, Color.Black, 0, Vector2.Zero, this.ScreenSize, SpriteEffects.None, 0);
            foreach (var lYardComponent in this.mYardComponents) lYardComponent.Draw(spriteBatch, gameTime);
            this.mHudComponent.Draw(spriteBatch, gameTime);
        }

        private void YardComponent_TravelToYard(BeeWorldYardComponent yardComponent)
        {
            var lBeeYard = yardComponent.BeeYard;
            var lPlayer = this.ScreenManager.Player;
            lPlayer.TravelTo(lBeeYard);

            var lBeeYardScreen = new BeeYardScreen();
            this.ScreenManager.TransitionTo(lBeeYardScreen);
        }
    }
}
