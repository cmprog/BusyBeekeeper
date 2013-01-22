using System;
using BusyBeekeeper.Core;
using BusyBeekeeper.Data;
using BusyBeekeeper.DataRepositories;
using BusyBeekeeper.Screens.CommonComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BusyBeekeeper.Screens
{
    internal sealed class ShopSuperSectionScreen : GameScreenBase
    {
        #region Instance Fields --------------------------------------------------------

        private Player mPlayer;
        private PlayerManager mPlayerManager;

        private SuperRepository mSuperRepository;
        private SuperPaintRepository mSuperPaintRepository;
        private bool mAreSuperPaintsLocked;
        private TimeSpan mNextSuperPaintRefreshTime;

        private Texture2D mBlankTexture;
        private ButtonMenuComponent mButtonMenuComponent;
        private ConfirmationDialogComponent mConfirmationDialogComponent;
        private ShopScreenHudComponent mHudComponent;

        private readonly Vector2 mSuperItemSize = new Vector2(500, sSuperItemHeight);
        private readonly Vector2 mSuperPaintItemSize = new Vector2(300, sSuperPaintItemHeight);
        private ShopSuperSectionItemComponent mSelectedItemComponent;
        private readonly ShopSuperSectionItemComponent mTopSuperItemComponent = new ShopSuperSectionItemComponent();
        private readonly ShopSuperSectionItemComponent mMiddleSuperItemComponent = new ShopSuperSectionItemComponent();
        private readonly ShopSuperSectionItemComponent mBottomSuperItemComponent = new ShopSuperSectionItemComponent();
        private readonly ShopSuperSectionItemComponent mTopSuperPaintItemComponent = new ShopSuperSectionItemComponent();
        private readonly ShopSuperSectionItemComponent mMiddleTopSuperPaintItemComponent = new ShopSuperSectionItemComponent();
        private readonly ShopSuperSectionItemComponent mMiddleBottomSuperPaintItemComponent = new ShopSuperSectionItemComponent();
        private readonly ShopSuperSectionItemComponent mBottomSuperPaintItemComponent = new ShopSuperSectionItemComponent();

        private readonly MenuButton mMenuButtonBuy = new MenuButton();
        private readonly MenuButton mMenuButtonBack = new MenuButton();

        #endregion

        #region Static Fields ----------------------------------------------------------

        private const int sItemPadding = 10;
        private const int sSuperItemCount = 3;
        private const int sSuperPaintItemCount = 4;
        private const int sMinutesPerRefresh = 1;

        private const int sSuperItemHeight = 150;
        private const int sTotalSuperItemHeight = (sSuperItemHeight*sSuperItemCount)+(sItemPadding*(sSuperItemCount - 1));
        private const int sSuperPaintItemHeight = (sTotalSuperItemHeight - ((sSuperPaintItemCount - 1)*sItemPadding))/sSuperPaintItemCount;

        #endregion

        #region Constructors -----------------------------------------------------------

        public ShopSuperSectionScreen()
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
            // mMenuButtonBack
            //
            this.mSelectedItemComponent = this.mTopSuperItemComponent;
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

            this.mSuperPaintRepository = new SuperPaintRepository(this.ContentManager);
            this.mSuperRepository = new SuperRepository(this.ContentManager);

            this.mBlankTexture = this.ContentManager.Load<Texture2D>("Sprites/Blank");

            this.mConfirmationDialogComponent = new ConfirmationDialogComponent(this.ScreenSize);
            this.mConfirmationDialogComponent.LoadContent(this.ContentManager);
            this.mConfirmationDialogComponent.CancelText = "No";
            this.mConfirmationDialogComponent.ConfirmText = "Yes";
            this.mConfirmationDialogComponent.Confirm += this.ConfirmationDialogComponent_Confirm;
            this.mConfirmationDialogComponent.Cancel += this.ConfirmationDialogComponent_Cancel;

            this.mButtonMenuComponent = new ButtonMenuComponent(this.ScreenSize);
            this.mButtonMenuComponent.LoadContent(this.ContentManager);
            this.mButtonMenuComponent.MenuButtons.Add(this.mMenuButtonBuy);
            this.mButtonMenuComponent.MenuButtons.Add(this.mMenuButtonBack);
            //
            // mHudComponent
            //
            this.mHudComponent = new ShopScreenHudComponent(this.ScreenManager.BeeWorldManager, this.ScreenSize);
            this.mHudComponent.LoadContent(this.ContentManager);
            //
            // mMiddleSuperItemComponent
            //
            this.mMiddleSuperItemComponent.LoadContent(this.ContentManager);
            this.mMiddleSuperItemComponent.IconTexture = this.mBlankTexture;
            this.mMiddleSuperItemComponent.Size = this.mSuperItemSize;
            this.mMiddleSuperItemComponent.Position = new Vector2(
                ((this.ScreenSize.X - this.mSuperItemSize.X - this.mSuperPaintItemSize.X)/2f),
                ((this.ScreenSize.Y - this.mSuperItemSize.Y - this.mHudComponent.Size.Y)/2f));
            this.mMiddleSuperItemComponent.NameText = "Middle";
            this.mMiddleSuperItemComponent.DescriptionText = "This is the middle item.";
            this.mMiddleSuperItemComponent.Price = 2;
            this.mMiddleSuperItemComponent.Click += this.SelectItemComponent;
            this.PopulateItemComponent(
                this.mMiddleSuperItemComponent, 
                this.mSuperRepository.GetMetaObject(1));

            this.mTopSuperItemComponent.LoadContent(this.ContentManager);
            this.mTopSuperItemComponent.IconTexture = this.mBlankTexture;
            this.mTopSuperItemComponent.Size = this.mSuperItemSize;
            this.mTopSuperItemComponent.Position = new Vector2(
                this.mMiddleSuperItemComponent.Position.X,
                this.mMiddleSuperItemComponent.Position.Y - sItemPadding - this.mSuperItemSize.Y);
            this.mTopSuperItemComponent.NameText = "Top";
            this.mTopSuperItemComponent.DescriptionText = "This is the top item.";
            this.mTopSuperItemComponent.Price = 1;
            this.mTopSuperItemComponent.Click += this.SelectItemComponent;
            this.PopulateItemComponent(
                this.mTopSuperItemComponent, 
                this.mSuperRepository.GetMetaObject(0));

            this.mBottomSuperItemComponent.LoadContent(this.ContentManager);
            this.mBottomSuperItemComponent.IconTexture = this.mBlankTexture;
            this.mBottomSuperItemComponent.Size = this.mSuperItemSize;
            this.mBottomSuperItemComponent.Position =
            this.mBottomSuperItemComponent.Position = new Vector2(
                this.mMiddleSuperItemComponent.Position.X,
                this.mMiddleSuperItemComponent.Position.Y + this.mSuperItemSize.Y + sItemPadding);
            this.mBottomSuperItemComponent.NameText = "Right";
            this.mBottomSuperItemComponent.DescriptionText = "This is the bottom item.";
            this.mBottomSuperItemComponent.Price = 3;
            this.mBottomSuperItemComponent.Click += this.SelectItemComponent;
            this.PopulateItemComponent(
                this.mBottomSuperItemComponent, 
                this.mSuperRepository.GetMetaObject(2));

            this.mTopSuperPaintItemComponent.LoadContent(this.ContentManager);
            this.mTopSuperPaintItemComponent.IconTexture = this.mBlankTexture;
            this.mTopSuperPaintItemComponent.Size = this.mSuperPaintItemSize;
            this.mTopSuperPaintItemComponent.Position = new Vector2(
                this.mTopSuperItemComponent.Position.X + this.mSuperItemSize.X + sItemPadding,
                this.mTopSuperItemComponent.Position.Y);
            this.mTopSuperPaintItemComponent.NameText = "Top";
            this.mTopSuperPaintItemComponent.DescriptionText = "This is the top item.";
            this.mTopSuperPaintItemComponent.Price = 4;
            this.mTopSuperPaintItemComponent.Click += this.SelectItemComponent;

            this.mMiddleTopSuperPaintItemComponent.LoadContent(this.ContentManager);
            this.mMiddleTopSuperPaintItemComponent.IconTexture = this.mBlankTexture;
            this.mMiddleTopSuperPaintItemComponent.Size = this.mSuperPaintItemSize;
            this.mMiddleTopSuperPaintItemComponent.Position = new Vector2(
                this.mTopSuperPaintItemComponent.Position.X,
                this.mTopSuperPaintItemComponent.Position.Y + this.mSuperPaintItemSize.Y + sItemPadding);
            this.mMiddleTopSuperPaintItemComponent.NameText = "Middle Top";
            this.mMiddleTopSuperPaintItemComponent.DescriptionText = "This is the middle-top item.";
            this.mMiddleTopSuperPaintItemComponent.Price = 5;
            this.mMiddleTopSuperPaintItemComponent.Click += this.SelectItemComponent;

            this.mMiddleBottomSuperPaintItemComponent.LoadContent(this.ContentManager);
            this.mMiddleBottomSuperPaintItemComponent.IconTexture = this.mBlankTexture;
            this.mMiddleBottomSuperPaintItemComponent.Size = this.mSuperPaintItemSize;
            this.mMiddleBottomSuperPaintItemComponent.Position = new Vector2(
                this.mTopSuperPaintItemComponent.Position.X,
                this.mMiddleTopSuperPaintItemComponent.Position.Y + this.mSuperPaintItemSize.Y + sItemPadding);
            this.mMiddleBottomSuperPaintItemComponent.NameText = "Middle Bottom";
            this.mMiddleBottomSuperPaintItemComponent.DescriptionText = "This is the middle-bottom item.";
            this.mMiddleBottomSuperPaintItemComponent.Price = 6;
            this.mMiddleBottomSuperPaintItemComponent.Click += this.SelectItemComponent;

            this.mBottomSuperPaintItemComponent.LoadContent(this.ContentManager);
            this.mBottomSuperPaintItemComponent.IconTexture = this.mBlankTexture;
            this.mBottomSuperPaintItemComponent.Size = this.mSuperPaintItemSize;
            this.mBottomSuperPaintItemComponent.Position = new Vector2(
                this.mTopSuperPaintItemComponent.Position.X,
                this.mMiddleBottomSuperPaintItemComponent.Position.Y + this.mSuperPaintItemSize.Y + sItemPadding);
            this.mBottomSuperPaintItemComponent.NameText = "Bottom";
            this.mBottomSuperPaintItemComponent.DescriptionText = "This is the bottom item.";
            this.mBottomSuperPaintItemComponent.Price = 7;
            this.mBottomSuperPaintItemComponent.Click += this.SelectItemComponent;
        }

        private void SelectItemComponent(ShopSuperSectionItemComponent itemComponent)
        {
            this.mSelectedItemComponent.IsSelected = false;
            this.mSelectedItemComponent = itemComponent;
            this.mSelectedItemComponent.IsSelected = true;
        }

        private void ConfirmationDialogComponent_Confirm(ConfirmationDialogComponent obj)
        {
            var lMetaSuper = this.mSelectedItemComponent.Tag as MetaSuper;
            if (lMetaSuper != null)
            {
                var lSuper = this.mSuperRepository.CreateObject(lMetaSuper);
                this.mPlayerManager.ProcessCoinExchange(-lMetaSuper.PurchasePrice);
                this.mPlayer.Supers.Add(lSuper);
            }
            else
            {
                var lMetaSuperPaint = this.mSelectedItemComponent.Tag as MetaSuperPaint;
                System.Diagnostics.Debug.Assert(lMetaSuperPaint != null);

                var lSuperPaint = this.mSuperPaintRepository.CreateObject(lMetaSuperPaint);
                this.mPlayerManager.ProcessCoinExchange(-lMetaSuperPaint.PurchasePrice);
                this.mPlayer.SuperPaints.Add(lSuperPaint);
            }

            this.mAreSuperPaintsLocked = false;
        }

        private void ConfirmationDialogComponent_Cancel(ConfirmationDialogComponent obj)
        {
            this.mAreSuperPaintsLocked = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!this.mAreSuperPaintsLocked && (gameTime.TotalGameTime > this.mNextSuperPaintRefreshTime))
            {
                var lNow = DateTime.Now;
                var lCurrentHour = lNow.Date.AddHours(lNow.Hour);

                var lCurrentPeriodIndex = lNow.Minute/sMinutesPerRefresh;
                var lNextPeriodIndex = lCurrentPeriodIndex + 1;

                var lNextRefreshDateTime = lCurrentHour.AddMinutes(lNextPeriodIndex * sMinutesPerRefresh);
                this.mNextSuperPaintRefreshTime = lNextRefreshDateTime - lNow;

                var lRandom = new Random((int)lNextRefreshDateTime.Ticks);

                var lTopSuperPaintIndex = lRandom.Next(this.mSuperPaintRepository.Count);
                var lTopSuperPaint = this.mSuperPaintRepository.GetMetaObject(lTopSuperPaintIndex);
                this.PopulateItemComponent(this.mTopSuperPaintItemComponent, lTopSuperPaint);

                var lMiddleTopSuperPaintIndex = lRandom.Next(this.mSuperPaintRepository.Count);
                var lMiddleTopSuperPaint = this.mSuperPaintRepository.GetMetaObject(lMiddleTopSuperPaintIndex);
                this.PopulateItemComponent(this.mMiddleTopSuperPaintItemComponent, lMiddleTopSuperPaint);

                var lMiddleBottomSuperPaintIndex = lRandom.Next(this.mSuperPaintRepository.Count);
                var lMiddleBottomSuperPaint = this.mSuperPaintRepository.GetMetaObject(lMiddleBottomSuperPaintIndex);
                this.PopulateItemComponent(this.mMiddleBottomSuperPaintItemComponent, lMiddleBottomSuperPaint);

                var lBottomSuperPaintIndex = lRandom.Next(this.mSuperPaintRepository.Count);
                var lBottomSuperPaint = this.mSuperPaintRepository.GetMetaObject(lBottomSuperPaintIndex);
                this.PopulateItemComponent(this.mBottomSuperPaintItemComponent, lBottomSuperPaint);
            }

            this.mButtonMenuComponent.Update(gameTime);
            this.mConfirmationDialogComponent.Update(gameTime);
            this.mHudComponent.Update(gameTime);

            this.mTopSuperItemComponent.Update(gameTime);
            this.mMiddleSuperItemComponent.Update(gameTime);
            this.mBottomSuperItemComponent.Update(gameTime);
            this.mTopSuperPaintItemComponent.Update(gameTime);
            this.mMiddleTopSuperPaintItemComponent.Update(gameTime);
            this.mMiddleBottomSuperPaintItemComponent.Update(gameTime);
            this.mBottomSuperPaintItemComponent.Update(gameTime);
        }

        private void PopulateItemComponent(ShopSuperSectionItemComponent itemComponent, MetaSuperPaint superPaint)
        {
            System.Diagnostics.Debug.Assert(itemComponent != null);
            System.Diagnostics.Debug.Assert(superPaint != null);

            itemComponent.Tag = superPaint;
            itemComponent.NameText = superPaint.Name;
            itemComponent.DescriptionText = superPaint.Description;
            itemComponent.IconTint = new Color(
                superPaint.ColorValueR, superPaint.ColorValueG,
                superPaint.ColorValueB, superPaint.ColorValueA);
            itemComponent.Price = superPaint.PurchasePrice;
        }

        private void PopulateItemComponent(ShopSuperSectionItemComponent itemComponent, MetaSuper super)
        {
            System.Diagnostics.Debug.Assert(itemComponent != null);
            System.Diagnostics.Debug.Assert(super != null);

            itemComponent.Tag = super;
            itemComponent.NameText = super.Name;
            itemComponent.DescriptionText = super.Description;
            itemComponent.IconTint = Color.White;
            itemComponent.Price = super.PurchasePrice;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);

            spriteBatch.Draw(this.mBlankTexture, Vector2.Zero, null, Color.Black, 0, Vector2.Zero, this.ScreenSize, SpriteEffects.None, 0);

            this.mTopSuperItemComponent.Draw(spriteBatch, gameTime);
            this.mMiddleSuperItemComponent.Draw(spriteBatch, gameTime);
            this.mBottomSuperItemComponent.Draw(spriteBatch, gameTime);
            this.mTopSuperPaintItemComponent.Draw(spriteBatch, gameTime);
            this.mMiddleTopSuperPaintItemComponent.Draw(spriteBatch, gameTime);
            this.mMiddleBottomSuperPaintItemComponent.Draw(spriteBatch, gameTime);
            this.mBottomSuperPaintItemComponent.Draw(spriteBatch, gameTime);

            this.mButtonMenuComponent.Draw(spriteBatch, gameTime);
            this.mHudComponent.Draw(spriteBatch, gameTime);
            this.mConfirmationDialogComponent.Draw(spriteBatch, gameTime);
        }

        public override void HandleInput(InputState inputState)
        {
            base.HandleInput(inputState);

            this.mConfirmationDialogComponent.HandleInput(inputState);
            this.mHudComponent.HandleInput(inputState);
            this.mButtonMenuComponent.HandleInput(inputState);

            this.mTopSuperItemComponent.HandleInput(inputState);
            this.mMiddleSuperItemComponent.HandleInput(inputState);
            this.mBottomSuperItemComponent.HandleInput(inputState);
            this.mTopSuperPaintItemComponent.HandleInput(inputState);
            this.mMiddleTopSuperPaintItemComponent.HandleInput(inputState);
            this.mMiddleBottomSuperPaintItemComponent.HandleInput(inputState);
            this.mBottomSuperPaintItemComponent.HandleInput(inputState);
        }

        private void MenuButtonBack_Click(MenuButton menuButton)
        {
            var lShopScreen = new ShopScreen();
            this.ScreenManager.TransitionTo(lShopScreen);
        }

        private void MenuButtonBuy_Click(MenuButton menuButton)
        {
            this.mAreSuperPaintsLocked = true;

            this.mConfirmationDialogComponent.MessageText = string.Concat(
                "Are you sure you want to purchase ",
                this.mSelectedItemComponent.NameText,
                " for ", this.mSelectedItemComponent.Price, " coins?");
            this.mConfirmationDialogComponent.IsVisible = true;
        }

        #endregion
        
    }
}
