using System;
using BusyBeekeeper.Data.Graphics.Shop;
using BusyBeekeeper.Screens.CommonComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BusyBeekeeper.Screens
{
    internal sealed class ShopScreen : GameScreenBase
    {
        #region Instance Fields --------------------------------------------------------

        private Texture2D mBlankTexture;

        private ButtonMenuComponent mButtonMenuComponent;
        private ShopScreenHudComponent mHudComponent;

        private ShopSectionComponent mQueenSectionComponent;
        private ShopSectionComponent mSuperSectionComponent;
        private ShopSectionComponent mFieldSectionComponent;
        private ShopSectionComponent mExtractionSectionComponent;
        private ShopSectionComponent mMarketSectionComponent;
        private ShopSectionComponent mBottlesSectionComponent;

        private readonly MenuButton mMenuButtonTravel = new MenuButton();

        #endregion

        #region Static Fields ----------------------------------------------------------

        private const int sQueenSectionId = 0;
        private const int sSuperSectionId = 1;
        private const int sFieldSectionId = 2;
        private const int sExtractionSectionId = 3;
        private const int sMarketSectionId = 4;
        private const int sBottlesSectionId = 5;

        #endregion

        #region Constructors -----------------------------------------------------------

        public ShopScreen()
        {
            //
            // mMenuButtonTravel
            //
            this.mMenuButtonTravel.Text = "Travel";
            this.mMenuButtonTravel.Click += this.MenuButtonTravel_Click;
        }

        #endregion

        #region Instance Properties ----------------------------------------------------

        #endregion

        #region Instance Methods -------------------------------------------------------

        public override void LoadContent()
        {
            base.LoadContent();

            this.mBlankTexture = this.ContentManager.Load<Texture2D>("Sprites/Blank");
            
            this.mButtonMenuComponent = new ButtonMenuComponent(this.ScreenSize);
            this.mButtonMenuComponent.LoadContent(this.ContentManager);
            this.mButtonMenuComponent.MenuButtons.Add(this.mMenuButtonTravel);

            var lShopSectionInfos = this.ContentManager.Load<ShopSectionInfo[]>("GraphicsData/Shop/ShopSectionInfos");
            //
            // mQueenSectionComponent
            //
            this.mQueenSectionComponent = new ShopSectionComponent(lShopSectionInfos[sQueenSectionId]);
            this.mQueenSectionComponent.Tag = new Func<IGameScreen>(() => new ShopQueenSectionScreen());
            this.mQueenSectionComponent.LoadContent(this.ContentManager);
            this.mQueenSectionComponent.Click += this.ShopSectionComponent_Click;
            //
            // mSuperSectionComponent
            //
            this.mSuperSectionComponent = new ShopSectionComponent(lShopSectionInfos[sSuperSectionId]);
            this.mSuperSectionComponent.Tag = new Func<IGameScreen>(() => new ShopSuperSectionScreen());
            this.mSuperSectionComponent.LoadContent(this.ContentManager);
            this.mSuperSectionComponent.Click += this.ShopSectionComponent_Click;
            //
            // mFieldSectionComponent
            //
            this.mFieldSectionComponent = new ShopSectionComponent(lShopSectionInfos[sFieldSectionId]);
            this.mFieldSectionComponent.Tag = new Func<IGameScreen>(() => new ShopFieldSectionScreen());
            this.mFieldSectionComponent.LoadContent(this.ContentManager);
            this.mFieldSectionComponent.Click += this.ShopSectionComponent_Click;
            //
            // mExtractionSectionComponent
            //
            this.mExtractionSectionComponent = new ShopSectionComponent(lShopSectionInfos[sExtractionSectionId]);
            this.mExtractionSectionComponent.Tag = new Func<IGameScreen>(() => new ShopExtractionSectionScreen());
            this.mExtractionSectionComponent.LoadContent(this.ContentManager);
            this.mExtractionSectionComponent.Click += this.ShopSectionComponent_Click;
            //
            // mMarketSectionComponent
            //
            this.mMarketSectionComponent = new ShopSectionComponent(lShopSectionInfos[sMarketSectionId]);
            this.mMarketSectionComponent.Tag = new Func<IGameScreen>(() => new ShopMarketSectionScreen());
            this.mMarketSectionComponent.LoadContent(this.ContentManager);
            this.mMarketSectionComponent.Click += this.ShopSectionComponent_Click;
            //
            // mBottlesSectionComponent
            //
            this.mBottlesSectionComponent = new ShopSectionComponent(lShopSectionInfos[sBottlesSectionId]);
            this.mBottlesSectionComponent.Tag = new Func<IGameScreen>(() => new ShopBottlesSectionScreen());
            this.mBottlesSectionComponent.LoadContent(this.ContentManager);
            this.mBottlesSectionComponent.Click += this.ShopSectionComponent_Click;
            //
            // mHudComponent
            //
            this.mHudComponent = new ShopScreenHudComponent(this.ScreenManager.BeeWorldManager, this.ScreenSize);
            this.mHudComponent.LoadContent(this.ContentManager);
        }

        private void ShopSectionComponent_Click(ShopSectionComponent sectionComponent)
        {
            var lScreenFactory = (Func<IGameScreen>) sectionComponent.Tag;
            var lScreen = lScreenFactory();
            this.ScreenManager.TransitionTo(lScreen);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.mButtonMenuComponent.Update(gameTime);
            this.mQueenSectionComponent.Update(gameTime);
            this.mHudComponent.Update(gameTime);
        }

        public override void HandleInput(InputState inputState)
        {
            base.HandleInput(inputState);

            this.mButtonMenuComponent.HandleInput(inputState);
            this.mHudComponent.HandleInput(inputState);

            this.mQueenSectionComponent.HandleInput(inputState);
            this.mSuperSectionComponent.HandleInput(inputState);
            this.mFieldSectionComponent.HandleInput(inputState);
            this.mExtractionSectionComponent.HandleInput(inputState);
            this.mMarketSectionComponent.HandleInput(inputState);
            this.mBottlesSectionComponent.HandleInput(inputState);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(this.mBlankTexture, Vector2.Zero, null, Color.Black, 0, Vector2.Zero, this.ScreenSize, SpriteEffects.None, 0);

            this.mQueenSectionComponent.Draw(spriteBatch, gameTime);
            this.mSuperSectionComponent.Draw(spriteBatch, gameTime);
            this.mFieldSectionComponent.Draw(spriteBatch, gameTime);
            this.mExtractionSectionComponent.Draw(spriteBatch, gameTime);
            this.mMarketSectionComponent.Draw(spriteBatch, gameTime);
            this.mBottlesSectionComponent.Draw(spriteBatch, gameTime);

            this.mHudComponent.Draw(spriteBatch, gameTime);
            this.mButtonMenuComponent.Draw(spriteBatch, gameTime);
        }

        private void MenuButtonTravel_Click(MenuButton obj)
        {
            var lWorldScreen = new BeeWorldScreen();
            this.ScreenManager.TransitionTo(lWorldScreen);
        }

        #endregion

    }
}
