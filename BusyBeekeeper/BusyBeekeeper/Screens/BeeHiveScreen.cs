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
    internal sealed class BeeHiveScreen : GameScreenBase
    {
        #region Instance Fields --------------------------------------------------------
        
        private Texture2D mBlankTexture;

        private Texture2D mSuperTexture;

        private readonly Button mButtonBeeYard = new Button();
        private readonly Button mButtonBeeWorld = new Button();
        private readonly Button mButtonSmokeHive = new Button();
        private readonly Button mButtonAddSuper = new Button();
        private readonly Button mButtonRemoveSuper = new Button();
        private readonly Button mButtonExtractHoney = new Button();
        private readonly BeeHiveSuperComponent[] mSuperComponents;
        private BeeHiveHudComponent mHudComponent;
        private InventoryItemSelectorComponent mInventorySelectorComponent;
        private readonly IList<ScreenComponent> mComponents = new List<ScreenComponent>();

        private Player mPlayer;
        private BeeHive mBeeHive;
        private BeeHiveManager mBeeHiveManager;

        private SpriteFont mMetaInfoFont;
        private SuperRepository mSuperRepository;

        #endregion

        #region Constructors -----------------------------------------------------------

        #endregion

        #region Instance Properties ----------------------------------------------------
        
        #endregion

        #region Instance Methods -------------------------------------------------------

        public override void LoadContent()
        {
            base.LoadContent();

            var lPlayerManager = this.ScreenManager.BeeWorldManager.PlayerManager;
            this.mPlayer = lPlayerManager.Player;
            System.Diagnostics.Debug.Assert(this.mPlayer.Location == PlayerLocation.BeeHive);
            this.mBeeHive = this.mPlayer.CurrentBeeHive;
            var lBeeYardManager = lPlayerManager.BeeYardManagers[this.mPlayer.CurrentBeeYard.Id];
            this.mBeeHiveManager = lBeeYardManager.BeeHiveManagers[this.mBeeHive.Id];

            this.mBlankTexture = this.ContentManager.Load<Texture2D>("Sprites/Blank");
            this.mMetaInfoFont = this.ContentManager.Load<SpriteFont>("Fonts/DefaultTiny");
            this.mSuperRepository = new SuperRepository(this.ContentManager);

            this.mButtonBeeWorld.Click += this.ButtonBeeWorld_Click;
            this.mButtonBeeWorld.Font = this.ContentManager.Load<SpriteFont>("Fonts/DefaultSmall");
            this.mButtonBeeWorld.Text = "World";
            this.mButtonBeeWorld.TextColor = Color.White;
            this.mButtonBeeWorld.BackgroundRenderer = new SolidBackgroundRenderer(this.mBlankTexture, Color.Blue);
            this.mButtonBeeWorld.Size = new Vector2(75, 30);
            this.mButtonBeeWorld.Position = new Vector2(10, this.ScreenSize.Y - this.mButtonBeeWorld.Size.Y - 10);

            this.mButtonBeeYard.Click += this.ButtonBeeYard_Click;
            this.mButtonBeeYard.Font = this.ContentManager.Load<SpriteFont>("Fonts/DefaultSmall");
            this.mButtonBeeYard.Text = "Yard";
            this.mButtonBeeYard.TextColor = Color.White;
            this.mButtonBeeYard.BackgroundRenderer = new SolidBackgroundRenderer(this.mBlankTexture, Color.Blue);
            this.mButtonBeeYard.Size = this.mButtonBeeWorld.Size;
            this.mButtonBeeYard.Position = new Vector2(
                this.mButtonBeeWorld.Position.X,
                this.mButtonBeeWorld.Position.Y - this.mButtonBeeWorld.Size.Y - 10);

            var lGoldBackgroundRenderer = new SolidBackgroundRenderer(this.mBlankTexture, Color.Gold);

            this.mButtonAddSuper.Click += this.ButtonAddSuper_Click;
            this.mButtonAddSuper.Text = "Add Super";
            this.mButtonAddSuper.TextColor = Color.Black;
            this.mButtonAddSuper.Font = this.ContentManager.Load<SpriteFont>("Fonts/DefaultSmall");
            this.mButtonAddSuper.BackgroundRenderer = lGoldBackgroundRenderer;
            this.mButtonAddSuper.Size = new Vector2(125, 45);
            this.mButtonAddSuper.Position = new Vector2(this.ScreenSize.X - this.mButtonAddSuper.Size.X - 10, 10);

            this.mButtonRemoveSuper.Click += this.ButtonRemoveSuper_Click;
            this.mButtonRemoveSuper.Text = "Remove Super";
            this.mButtonRemoveSuper.TextColor = Color.Black;
            this.mButtonRemoveSuper.Font = this.ContentManager.Load<SpriteFont>("Fonts/DefaultSmall");
            this.mButtonRemoveSuper.BackgroundRenderer = lGoldBackgroundRenderer;
            this.mButtonRemoveSuper.Size = this.mButtonAddSuper.Size;
            this.mButtonRemoveSuper.Position = new Vector2(
                this.mButtonAddSuper.Position.X,
                this.mButtonAddSuper.Position.Y + this.mButtonAddSuper.Size.Y + 10);

            this.mButtonSmokeHive.Click += this.ButtonSmokeHive_Click;
            this.mButtonSmokeHive.Text = "Use Smoker";
            this.mButtonSmokeHive.TextColor = Color.Black;
            this.mButtonSmokeHive.Font = this.ContentManager.Load<SpriteFont>("Fonts/DefaultSmall");
            this.mButtonSmokeHive.BackgroundRenderer = lGoldBackgroundRenderer;
            this.mButtonSmokeHive.Size = this.mButtonRemoveSuper.Size;
            this.mButtonSmokeHive.Position = new Vector2(
                this.mButtonRemoveSuper.Position.X,
                this.mButtonRemoveSuper.Position.Y + this.mButtonRemoveSuper.Size.Y + 10);

            this.mButtonExtractHoney.Click += this.ButtonExtractHoney_Click;
            this.mButtonExtractHoney.Text = "Extract Honey";
            this.mButtonExtractHoney.TextColor = Color.Black;
            this.mButtonExtractHoney.Font = this.ContentManager.Load<SpriteFont>("Fonts/DefaultSmall");
            this.mButtonExtractHoney.BackgroundRenderer = lGoldBackgroundRenderer;
            this.mButtonExtractHoney.Size = this.mButtonSmokeHive.Size;
            this.mButtonExtractHoney.Position = new Vector2(
                this.mButtonSmokeHive.Position.X,
                this.mButtonSmokeHive.Position.Y + this.mButtonSmokeHive.Size.Y + 10);

            this.mHudComponent = new BeeHiveHudComponent(this.ScreenManager.BeeWorldManager, this.ScreenSize);
            this.mHudComponent.LoadContent(this.ContentManager);

            this.mInventorySelectorComponent = new InventoryItemSelectorComponent(this.ScreenSize);
            this.mInventorySelectorComponent.LoadContent(this.ContentManager);

            this.mSuperTexture = new Texture2D(this.ScreenManager.Game.GraphicsDevice, 128, 128);
            this.mSuperTexture.SetData(Enumerable.Repeat(Color.Pink, 128 * 128).ToArray());

            this.mComponents.Add(this.mButtonBeeWorld);
            this.mComponents.Add(this.mButtonBeeYard);
            this.mComponents.Add(this.mButtonAddSuper);
            this.mComponents.Add(this.mButtonRemoveSuper);
            this.mComponents.Add(this.mButtonSmokeHive);
            this.mComponents.Add(this.mButtonExtractHoney);
            this.mComponents.Add(this.mHudComponent);
            this.mComponents.Add(this.mInventorySelectorComponent);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.mInventorySelectorComponent.Update(gameTime);
        }

        public override void HandleInput(InputState inputState)
        {
            base.HandleInput(inputState);

            // Must handle input backwards to make the topmost component
            // the first to have a change to handle the input.
            for (int lIndex = this.mComponents.Count - 1; lIndex >= 0; lIndex--)
            {
                var lComponent = this.mComponents[lIndex];
                if (lComponent.HandleInput(inputState)) return;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);

            spriteBatch.Draw(this.mBlankTexture, Vector2.Zero, null, Color.Black, 0, Vector2.Zero, this.ScreenSize, SpriteEffects.None, 0);
            
            const float lcSuperWidth = 500;
            const float lcSuperHeightPerDepth = 50;
            var lSuperBottomLeftLocation = new Vector2(
                (this.ScreenSize.X - lcSuperWidth) / 2,
                this.mHudComponent.Position.Y - 10f);

            foreach (var lSuper in this.mBeeHive.Supers)
            {
                var lSuperSize = new Vector2(lcSuperWidth, lcSuperHeightPerDepth * lSuper.Depth);
                var lSuperPosition = new Vector2(
                    lSuperBottomLeftLocation.X,
                    lSuperBottomLeftLocation.Y - lSuperSize.Y);

                spriteBatch.Draw(this.mBlankTexture, lSuperPosition, null, Color.White, 0, Vector2.Zero, lSuperSize, SpriteEffects.None, 0);

                lSuperBottomLeftLocation = new Vector2(lSuperPosition.X, lSuperPosition.Y - 1);
            }

            foreach (var lComponent in this.mComponents) lComponent.Draw(spriteBatch, gameTime);

            const float lcTextMarginBottom = 1f;

            var lPopulationText = string.Concat("Population : ", this.mBeeHive.Population);
            var lPopulationTextSize = this.mMetaInfoFont.MeasureString(lPopulationText);
            var lPopulationTextPosition = new Vector2(10, 10);

            var lHoneyCollectedText = string.Concat("Honey Collected : ", this.mBeeHive.HoneyCollected);
            var lHoneyCollectedTextSize = this.mMetaInfoFont.MeasureString(lHoneyCollectedText);
            var lHoneyCollectedTextPosition = new Vector2(
                lPopulationTextPosition.X,
                lPopulationTextPosition.Y + lPopulationTextSize.Y + lcTextMarginBottom);

            var lColonyStrengthText = string.Concat("Colony Strength : ", this.mBeeHive.ColonyStrength);
            var lColonyStrengthTextSize = this.mMetaInfoFont.MeasureString(lColonyStrengthText);
            var lColonyStrengthTextPosition = new Vector2(
                lHoneyCollectedTextPosition.X,
                lHoneyCollectedTextPosition.Y + lHoneyCollectedTextSize.Y + lcTextMarginBottom);

            var lColonyAggressivenessText = string.Concat("Colony Agressiveness : ", this.mBeeHive.ColonyAgressiveness);
            var lColonyAggressivenessTextSize = this.mMetaInfoFont.MeasureString(lColonyAggressivenessText);
            var lColonyAggressivenessTextPosition = new Vector2(
                lColonyStrengthTextPosition.X,
                lColonyStrengthTextPosition.Y + lColonyAggressivenessTextSize.Y + lcTextMarginBottom);

            var lColonySwarmLiklinessText = string.Concat("Colony Swarm Likliness : ", this.mBeeHive.ColonySwarmLikeliness);
            var lColonySwarmLiklinessTextSize = this.mMetaInfoFont.MeasureString(lColonySwarmLiklinessText);
            var lColonySwarmLiklinessTextPosition = new Vector2(
                lColonyAggressivenessTextPosition.X,
                lColonyAggressivenessTextPosition.Y + lColonyAggressivenessTextSize.Y + lcTextMarginBottom);

            spriteBatch.DrawString(this.mMetaInfoFont, lPopulationText, lPopulationTextPosition, Color.Green);
            spriteBatch.DrawString(this.mMetaInfoFont, lHoneyCollectedText, lHoneyCollectedTextPosition, Color.Green);
            spriteBatch.DrawString(this.mMetaInfoFont, lColonyStrengthText, lColonyStrengthTextPosition, Color.Green);
            spriteBatch.DrawString(this.mMetaInfoFont, lColonyAggressivenessText, lColonyAggressivenessTextPosition, Color.Green);
            spriteBatch.DrawString(this.mMetaInfoFont, lColonySwarmLiklinessText, lColonySwarmLiklinessTextPosition, Color.Green);
        }

        private void ButtonBeeWorld_Click(Button button)
        {
            var lWorldScreen = new BeeWorldScreen();
            this.ScreenManager.TransitionTo(lWorldScreen);
        }

        private void ButtonBeeYard_Click(Button button)
        {
            var lYardScreen = new BeeYardScreen();
            var lPlayerManager = this.ScreenManager.BeeWorldManager.PlayerManager;
            lPlayerManager.TravelToBeeYard();
            this.ScreenManager.TransitionTo(lYardScreen);
        }

        private void AddSuper(InventoryItem item)
        {
            var lSuper = (Super)item.Tag;

            this.mBeeHive.Supers.Add(lSuper);
            this.mPlayer.Supers.Remove(lSuper);
        }

        private void ButtonAddSuper_Click(Button button)
        {
            this.ScreenManager.BeeWorldManager.IsPaused = true;

            this.mInventorySelectorComponent.Items.Clear();
            this.mInventorySelectorComponent.Items.AddRange(
                this.mPlayer.Supers.Select(x => this.ToInventoryItem(x, 1)));
            //this.mInventorySelectorComponent.Items.AddRange(
            //    this.mPlayer.Supers
            //    .GroupBy(x => x.MetaId)
            //    .Select(x => this.ToInventoryItem(x.First(), x.Count())));

            this.mInventorySelectorComponent.SelectActionText = "Add Super";
            this.mInventorySelectorComponent.Show(
                this.AddSuper, () => this.ScreenManager.BeeWorldManager.IsPaused = false);
        }

        private void ButtonRemoveSuper_Click(Button button)
        {
        }

        private void ButtonSmokeHive_Click(Button button)
        {
            this.ToggleInput(false);
            this.mBeeHiveManager.SmokeHive(this.mPlayer.Smoker, this.SmokingFinished);
        }

        private void ButtonExtractHoney_Click(Button button)
        {
        }

        private void SmokingFinished()
        {
            this.mButtonSmokeHive.Text = "Smoke Hive";
            this.ToggleInput(true);
        }

        private void ToggleInput(bool enable)
        {
            this.mButtonAddSuper.IsEnabled = enable;
            this.mButtonBeeWorld.IsEnabled = enable;
            this.mButtonBeeYard.IsEnabled = enable;
            this.mButtonExtractHoney.IsEnabled = enable;
            this.mButtonRemoveSuper.IsEnabled = enable;
            this.mButtonSmokeHive.IsEnabled = enable;
        }
        
        private InventoryItem ToInventoryItem(Super super, int count)
        {
            var lInventoryItem = new InventoryItem();
            lInventoryItem.Tag = super;
            lInventoryItem.Name = super.Name;
            lInventoryItem.Description = super.Description;
            lInventoryItem.Quantity = count;
            lInventoryItem.Texture = this.mSuperTexture;
            return lInventoryItem;
        }

        #endregion

        #region Static Methods ---------------------------------------------------------
        
        #endregion
    }
}
