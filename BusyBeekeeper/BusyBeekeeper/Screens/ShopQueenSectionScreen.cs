using System;
using BusyBeekeeper.Core;
using BusyBeekeeper.Data;
using BusyBeekeeper.DataRepositories;
using BusyBeekeeper.Screens.CommonComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BusyBeekeeper.Screens
{
    internal sealed class ShopQueenSectionScreen : GameScreenBase
    {
        #region Instance Fields --------------------------------------------------------

        private Player mPlayer;
        private PlayerManager mPlayerManager;

        private Texture2D mBlankTexture;
        private ButtonMenuComponent mButtonMenuComponent;
        private ConfirmationDialogComponent mConfirmationDialogComponent;
        private ShopScreenHudComponent mHudComponent;

        private QueenBeeRepository mQueenBeeRepository;
        private bool mAreQueensLocked;
        private TimeSpan mNextQueenRefreshTime;

        private readonly Vector2 mItemSize = new Vector2(700, 150);
        private ShopQueenSelectionItemComponent mSelectedItemComponent;
        private readonly ShopQueenSelectionItemComponent mTopItemComponent = new ShopQueenSelectionItemComponent();
        private readonly ShopQueenSelectionItemComponent mMiddleItemComponent = new ShopQueenSelectionItemComponent();
        private readonly ShopQueenSelectionItemComponent mBottomItemComponent = new ShopQueenSelectionItemComponent();

        private readonly MenuButton mMenuButtonBuy = new MenuButton();
        private readonly MenuButton mMenuButtonBack = new MenuButton();

        #endregion

        #region Static Fields ----------------------------------------------------------

        private const int sItemPadding = 10;
        private const int sItemCount = 3;
        private const int sMinutesPerRefresh = 1;

        #endregion

        #region Constructors -----------------------------------------------------------

        public ShopQueenSectionScreen()
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
            //
            // mSelectedItemComponent
            //
            this.mSelectedItemComponent = this.mTopItemComponent;
            this.mSelectedItemComponent.IsSelected = true;
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

            this.mQueenBeeRepository = new QueenBeeRepository(this.ContentManager);

            this.mHudComponent = new ShopScreenHudComponent(this.ScreenManager.BeeWorldManager, this.ScreenSize);
            this.mHudComponent.LoadContent(this.ContentManager);

            this.mConfirmationDialogComponent = new ConfirmationDialogComponent(this.ScreenSize);
            this.mConfirmationDialogComponent.LoadContent(this.ContentManager);
            this.mConfirmationDialogComponent.CancelText = "No";
            this.mConfirmationDialogComponent.ConfirmText = "Yes";
            this.mConfirmationDialogComponent.Cancel += this.ConfirmationDialogComponent_Cancel;
            this.mConfirmationDialogComponent.Confirm += this.ConfirmationDialogComponent_Confirm;

            this.mButtonMenuComponent = new ButtonMenuComponent(this.ScreenSize);
            this.mButtonMenuComponent.LoadContent(this.ContentManager);
            this.mButtonMenuComponent.MenuButtons.Add(this.mMenuButtonBuy);
            this.mButtonMenuComponent.MenuButtons.Add(this.mMenuButtonBack);

            this.mMiddleItemComponent.LoadContent(this.ContentManager);
            this.mMiddleItemComponent.IconTexture = this.mBlankTexture;
            this.mMiddleItemComponent.Size = this.mItemSize;
            this.mMiddleItemComponent.Position = new Vector2(
                (this.ScreenSize.X - this.mItemSize.X) / 2f,
                (this.ScreenSize.Y - this.mItemSize.Y - this.mHudComponent.Size.Y) / 2f);
            this.mMiddleItemComponent.Price = 2;
            this.mMiddleItemComponent.Click += this.SelectItemComponent;

            this.mTopItemComponent.LoadContent(this.ContentManager);
            this.mTopItemComponent.IconTexture = this.mBlankTexture;
            this.mTopItemComponent.Size = this.mItemSize;
            this.mTopItemComponent.Position = new Vector2(
                this.mMiddleItemComponent.Position.X,
                this.mMiddleItemComponent.Position.Y - sItemPadding - this.mItemSize.Y);
            this.mTopItemComponent.Price = 1;
            this.mTopItemComponent.Click += this.SelectItemComponent;

            this.mBottomItemComponent.LoadContent(this.ContentManager);
            this.mBottomItemComponent.IconTexture = this.mBlankTexture;
            this.mBottomItemComponent.Size = this.mItemSize;
            this.mBottomItemComponent.Position =
            this.mBottomItemComponent.Position = new Vector2(
                this.mMiddleItemComponent.Position.X,
                this.mMiddleItemComponent.Position.Y + this.mItemSize.Y + sItemPadding);
            this.mBottomItemComponent.Price = 3;
            this.mBottomItemComponent.Click += this.SelectItemComponent;
        }

        private void ConfirmationDialogComponent_Confirm(ConfirmationDialogComponent obj)
        {
            var lMetaQueenBee = (MetaQueenBee) this.mSelectedItemComponent.Tag;
            var lQueenBee = this.mQueenBeeRepository.CreateObject(lMetaQueenBee);

            this.mPlayerManager.ProcessCoinExchange(-lMetaQueenBee.PurchasePrice);
            this.mPlayer.QueenBees.Add(lQueenBee);

            this.mAreQueensLocked = false;
        }

        private void ConfirmationDialogComponent_Cancel(ConfirmationDialogComponent obj)
        {
            this.mAreQueensLocked = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!this.mAreQueensLocked && (gameTime.TotalGameTime > this.mNextQueenRefreshTime))
            {
                var lNow = DateTime.Now;
                var lCurrentHour = lNow.Date.AddHours(lNow.Hour);

                var lCurrentPeriodIndex = lNow.Minute/sMinutesPerRefresh;
                var lNextPeriodIndex = lCurrentPeriodIndex + 1;

                var lNextRefreshDateTime = lCurrentHour.AddMinutes(lNextPeriodIndex*sMinutesPerRefresh);
                this.mNextQueenRefreshTime = lNextRefreshDateTime - lNow;

                var lRandom = new Random((int) lNextRefreshDateTime.Ticks);

                var lTopQueenIndex = lRandom.Next(this.mQueenBeeRepository.Count);
                var lTopQueen = this.mQueenBeeRepository.GetMetaObject(lTopQueenIndex);
                this.PopulateItemComponent(this.mTopItemComponent, lTopQueen);

                var lMiddleQueenIndex = lRandom.Next(this.mQueenBeeRepository.Count);
                var lMiddleQueen = this.mQueenBeeRepository.GetMetaObject(lMiddleQueenIndex);
                this.PopulateItemComponent(this.mMiddleItemComponent, lMiddleQueen);

                var lBottomQueenIndex = lRandom.Next(this.mQueenBeeRepository.Count);
                var lBottomQueen = this.mQueenBeeRepository.GetMetaObject(lBottomQueenIndex);
                this.PopulateItemComponent(this.mBottomItemComponent, lBottomQueen);
            }

            this.mTopItemComponent.Update(gameTime);
            this.mMiddleItemComponent.Update(gameTime);
            this.mBottomItemComponent.Update(gameTime);

            this.mButtonMenuComponent.Update(gameTime);
            this.mHudComponent.Update(gameTime);
            this.mConfirmationDialogComponent.Update(gameTime);
        }

        private void PopulateItemComponent(ShopQueenSelectionItemComponent itemComponent, MetaQueenBee metaQueenBee)
        {
            System.Diagnostics.Debug.Assert(itemComponent != null);
            System.Diagnostics.Debug.Assert(metaQueenBee != null);

            itemComponent.Tag = metaQueenBee;
            itemComponent.NameText = metaQueenBee.Name;
            itemComponent.DescriptionText = metaQueenBee.Description;
            itemComponent.Price = metaQueenBee.PurchasePrice;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);

            spriteBatch.Draw(this.mBlankTexture, Vector2.Zero, null, Color.Black, 0, Vector2.Zero, this.ScreenSize, SpriteEffects.None, 0);

            this.mTopItemComponent.Draw(spriteBatch, gameTime);
            this.mMiddleItemComponent.Draw(spriteBatch, gameTime);
            this.mBottomItemComponent.Draw(spriteBatch, gameTime);

            this.mButtonMenuComponent.Draw(spriteBatch, gameTime);
            this.mHudComponent.Draw(spriteBatch, gameTime);
            this.mConfirmationDialogComponent.Draw(spriteBatch, gameTime);
        }

        public override void HandleInput(InputState inputState)
        {
            base.HandleInput(inputState);

            this.mConfirmationDialogComponent.HandleInput(inputState);
            this.mHudComponent.HandleInput(inputState);
            this.mTopItemComponent.HandleInput(inputState);
            this.mMiddleItemComponent.HandleInput(inputState);
            this.mBottomItemComponent.HandleInput(inputState);
            this.mButtonMenuComponent.HandleInput(inputState);
        }

        private void MenuButtonBack_Click(MenuButton menuButton)
        {
            var lShopScreen = new ShopScreen();
            this.ScreenManager.TransitionTo(lShopScreen);
        }

        private void MenuButtonBuy_Click(MenuButton menuButton)
        {
            this.mAreQueensLocked = true;

            this.mConfirmationDialogComponent.MessageText = string.Concat(
                "Are you sure you want to purchase ",
                this.mSelectedItemComponent.NameText,
                " for ", this.mSelectedItemComponent.Price, " coins?");
            this.mConfirmationDialogComponent.IsVisible = true;
        }

        private void SelectItemComponent(ShopQueenSelectionItemComponent itemComponent)
        {
            System.Diagnostics.Debug.Assert(this.mSelectedItemComponent != null);
            System.Diagnostics.Debug.Assert(itemComponent != null);
            this.mSelectedItemComponent.IsSelected = false;
            this.mSelectedItemComponent = itemComponent;
            this.mSelectedItemComponent.IsSelected = true;
        }

        #endregion

    }
}
