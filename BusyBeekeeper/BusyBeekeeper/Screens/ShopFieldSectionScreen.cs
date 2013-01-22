using BusyBeekeeper.Core;
using BusyBeekeeper.Data;
using BusyBeekeeper.Screens.CommonComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BusyBeekeeper.Screens
{
    internal sealed class ShopFieldSectionScreen : GameScreenBase
    {
        #region Instance Fields --------------------------------------------------------

        private Player mPlayer;
        private PlayerManager mPlayerManager;

        private Texture2D mBlankTexture;
        private ButtonMenuComponent mButtonMenuComponent;
        private ConfirmationDialogComponent mConfirmationDialogComponent;

        private readonly MenuButton mMenuButtonBuy = new MenuButton();
        private readonly MenuButton mMenuButtonBack = new MenuButton();

        #endregion

        #region Constructors -----------------------------------------------------------

        public ShopFieldSectionScreen()
        {
            //
            // mMenuButtonBuy
            //
            this.mMenuButtonBuy.Text = "Buy";
            this.mMenuButtonBuy.Click += this.MenuButtonBuy_Click;
            //
            // mMenuButtonBack
            //
            this.mMenuButtonBack.Text = "Back";
            this.mMenuButtonBack.Click += this.MenuButtonBack_Click;
        }

        #endregion

        #region Instance Properties ----------------------------------------------------

        #endregion

        #region Instance Methods -------------------------------------------------------
        
        public override void LoadContent()
        {
            base.LoadContent();

            this.mPlayerManager = this.ScreenManager.BeeWorldManager.PlayerManager;
            this.mPlayer = this.mPlayerManager.Player;

            this.mBlankTexture = this.ContentManager.Load<Texture2D>("Sprites/Blank");

            this.mConfirmationDialogComponent = new ConfirmationDialogComponent(this.ScreenSize);
            this.mConfirmationDialogComponent.LoadContent(this.ContentManager);
            this.mConfirmationDialogComponent.CancelText = "No";
            this.mConfirmationDialogComponent.ConfirmText = "Yes";
            this.mConfirmationDialogComponent.Confirm += this.ConfirmationDialogComponent_Confirm;

            this.mButtonMenuComponent = new ButtonMenuComponent(this.ScreenSize);
            this.mButtonMenuComponent.LoadContent(this.ContentManager);
            this.mButtonMenuComponent.MenuButtons.Add(this.mMenuButtonBuy);
            this.mButtonMenuComponent.MenuButtons.Add(this.mMenuButtonBack);
        }

        private void ConfirmationDialogComponent_Confirm(ConfirmationDialogComponent obj)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.mButtonMenuComponent.Update(gameTime);
            this.mConfirmationDialogComponent.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);

            spriteBatch.Draw(this.mBlankTexture, Vector2.Zero, null, Color.Black, 0, Vector2.Zero, this.ScreenSize, SpriteEffects.None, 0);

            this.mButtonMenuComponent.Draw(spriteBatch, gameTime);
            this.mConfirmationDialogComponent.Draw(spriteBatch, gameTime);
        }

        public override void HandleInput(InputState inputState)
        {
            base.HandleInput(inputState);

            this.mConfirmationDialogComponent.HandleInput(inputState);
            this.mButtonMenuComponent.HandleInput(inputState);
        }

        private void MenuButtonBack_Click(MenuButton menuButton)
        {
            var lShopScreen = new ShopScreen();
            this.ScreenManager.TransitionTo(lShopScreen);
        }

        private void MenuButtonBuy_Click(MenuButton menuButton)
        {
            this.mConfirmationDialogComponent.MessageText = "Are you sure?";
            this.mConfirmationDialogComponent.IsVisible = true;
        }

        #endregion
        
    }
}
