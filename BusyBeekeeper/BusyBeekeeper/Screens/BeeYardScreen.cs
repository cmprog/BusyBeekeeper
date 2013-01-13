using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BusyBeekeeper.Data;
using BusyBeekeeper.Core;
using BusyBeekeeper.Screens.CommonComponents;
using BusyBeekeeper.DataRepositories;
using BusyBeekeeper.Data.Graphics.BeeYard;

namespace BusyBeekeeper.Screens
{
    internal sealed class BeeYardScreen : GameScreenBase
    {
        private Texture2D mBlankTexture;

        private Button mButtonBeeWorld = new Button();
        private Button mButtonMowGrass = new Button();
        private BeeYardHiveComponent[] mHiveComponents;
        private BeeYardHubComponent mHudComponent;

        private Player mPlayer;
        private BeeYard mBeeYard;
        private BeeYardManager mBeeYardManager;

        private SpriteFont mMetaInfoFont;

        private SuperRepository mSuperRepository;

        public override void LoadContent()
        {
            base.LoadContent();

            this.mBlankTexture = this.ContentManager.Load<Texture2D>("Sprites/Blank");
            this.mMetaInfoFont = this.ContentManager.Load<SpriteFont>("Fonts/DefaultTiny");
            this.mSuperRepository = new SuperRepository(this.ContentManager);

            this.mButtonBeeWorld.Click += this.ButtonBeeWorld_Click;
            this.mButtonBeeWorld.Font = this.ContentManager.Load<SpriteFont>("Fonts/DefaultSmall");
            this.mButtonBeeWorld.Text = "Travel";
            this.mButtonBeeWorld.TextColor = Color.White;
            this.mButtonBeeWorld.BackgroundRenderer = new SolidBackgroundRenderer(this.mBlankTexture, Color.Blue);
            this.mButtonBeeWorld.Size = new Vector2(75, 30);
            this.mButtonBeeWorld.Position = new Vector2(10, this.ScreenSize.Y - this.mButtonBeeWorld.Size.Y - 10);

            this.mButtonMowGrass.Click += this.ButtonMowGrass_Click;
            this.mButtonMowGrass.Font = this.ContentManager.Load<SpriteFont>("Fonts/DefaultSmall");
            this.mButtonMowGrass.Text = "Mow Grass";
            this.mButtonMowGrass.TextColor = Color.Black;
            this.mButtonMowGrass.BackgroundRenderer = new SolidBackgroundRenderer(this.mBlankTexture, Color.Yellow);
            this.mButtonMowGrass.Size = new Vector2(100, 35);
            this.mButtonMowGrass.Position = new Vector2(this.ScreenSize.X - this.mButtonMowGrass.Size.X - 10, 10);

            this.mPlayer = this.ScreenManager.Player;
            System.Diagnostics.Debug.Assert(this.mPlayer.CurrentBeeHive == null);
            System.Diagnostics.Debug.Assert(this.mPlayer.CurrentBeeYard != null);
            this.mBeeYard = this.mPlayer.CurrentBeeYard;
            this.mBeeYardManager = this.ScreenManager.BeeWorldManager.PlayerManager.BeeYardManagers[this.mBeeYard.Id];

            var lAssetName = string.Concat("GraphicsData/BeeYard/HiveInformation_", this.mBeeYard.Id);
            var lHiveInfos = this.ContentManager.Load<BeeYardHiveInfo[]>(lAssetName);

            this.mHiveComponents = new BeeYardHiveComponent[lHiveInfos.Length];
            var lGreenBackgroundRenderer = new SolidBackgroundRenderer(this.mBlankTexture, Color.Green);

            for (int lIndex = 0; lIndex < this.mHiveComponents.Length; lIndex++)
            {
                var lHiveComponent = new BeeYardHiveComponent(
                    this.mBlankTexture,
                    this.mBeeYard.BeeHives[lIndex], 
                    lHiveInfos[lIndex], 
                    this.mSuperRepository);

                this.mHiveComponents[lIndex] = lHiveComponent;
            }

            this.mHudComponent = new BeeYardHubComponent(this.ScreenManager.BeeWorldManager, this.ScreenSize);
            this.mHudComponent.LoadContent(this.ContentManager);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.mHudComponent.Update(gameTime);
        }

        public override void HandleInput(InputState inputState)
        {
            base.HandleInput(inputState);

            if (this.mButtonMowGrass.HandleInput(inputState)) return;
            if (this.mButtonBeeWorld.HandleInput(inputState)) return;
            if (this.mHudComponent.HandleInput(inputState)) return;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
            spriteBatch.Draw(this.mBlankTexture, Vector2.Zero, null, Color.Black, 0, Vector2.Zero, this.ScreenSize, SpriteEffects.None, 0);

            this.mButtonBeeWorld.Draw(spriteBatch, gameTime);
            this.mButtonMowGrass.Draw(spriteBatch, gameTime);

            foreach (var lHiveComponent in this.mHiveComponents) lHiveComponent.Draw(spriteBatch, gameTime);
            this.mHudComponent.Draw(spriteBatch, gameTime);

            var lGrassGrowthText = string.Concat("Grass Growth : ", this.mBeeYard.GrassGrowth);
            var lGrassGrowthPosition = new Vector2(10, 10);
            var lGrassGrothSize = this.mMetaInfoFont.MeasureString(lGrassGrowthText);

            spriteBatch.DrawString(this.mMetaInfoFont, lGrassGrowthText, lGrassGrowthPosition, Color.Green);
        }

        private void ButtonBeeWorld_Click(Button button)
        {
            this.ScreenManager.TransitionTo(new BeeWorldScreen());
        }

        private void ButtonMowGrass_Click(Button button)
        {
            this.mButtonMowGrass.Text = "Mowing...";
            this.mButtonMowGrass.IsEnabled = false;
            this.mButtonBeeWorld.IsEnabled = false;

            this.mBeeYardManager.MowLawn(
                this.ScreenManager.BeeWorldManager, 
                this.mPlayer.LawnMower, 
                LawnMowingComplete);
        }

        private void LawnMowingComplete()
        {
            this.mButtonMowGrass.Text = "Mow Lawn";
            this.mButtonMowGrass.IsEnabled = true;
            this.mButtonBeeWorld.IsEnabled = true;
        }
    }
}
