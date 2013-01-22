using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BusyBeekeeper.Data;
using BusyBeekeeper.Core;
using BusyBeekeeper.Screens.CommonComponents;
using BusyBeekeeper.DataRepositories;

namespace BusyBeekeeper.Screens
{
    internal sealed class BeeHiveScreen : GameScreenBase
    {
        #region Instance Fields --------------------------------------------------------
        
        private Texture2D mBlankTexture;

        private Texture2D mSuperTexture;

        // Selection Mode
        private bool mIsSelectionModeActive;
        private BeeHiveSuperComponent mSelectedSuper;
        private Action<BeeHiveSuperComponent> mSuperSelectedCallback;
        // -- Selection Mode

        // MenuButtons
        private readonly MenuButton mMenuButtonSupers = new MenuButton();
        private readonly MenuButton mMenuButtonSupersAdd = new MenuButton();
        private readonly MenuButton mMenuButtonSupersAddBrood = new MenuButton();
        private readonly MenuButton mMenuButtonSupersAddHoney = new MenuButton();
        private readonly MenuButton mMenuButtonSupersRemove = new MenuButton();
        private readonly MenuButton mMenuButtonQueen = new MenuButton();
        private readonly MenuButton mMenuButtonQueenChange = new MenuButton();
        private readonly MenuButton mMenuButtonQueenRemove = new MenuButton();
        private readonly MenuButton mMenuButtonUseSmoker = new MenuButton();
        private readonly MenuButton mMenuButtonToYard = new MenuButton();
        private readonly MenuButton mMenuButtonToWorld = new MenuButton();
        private readonly MenuButton mMenuButtonSelectionCommit = new MenuButton();
        private readonly MenuButton mMenuButtonSelectionCancel = new MenuButton();
        // -- MenuButtons

        private ButtonMenuComponent mButtonMenuComponent;

        private readonly List<BeeHiveSuperComponent> mSuperComponents = new List<BeeHiveSuperComponent>();
        private BeeHiveHudComponent mHudComponent;
        private InventoryItemSelectorComponent mInventorySelectorComponent;
        private readonly IList<ScreenComponent> mComponents = new List<ScreenComponent>();

        private Player mPlayer;
        private BeeHive mBeeHive;
        private BeeHiveManager mBeeHiveManager;

        private SpriteFont mMetaInfoFont;
        private SuperRepository mSuperRepository;

        private readonly int mScrollBoundsHeight = 50;
        private Rectangle mVerticalScrollUpBounds;
        private Rectangle mVerticalScrollDownBounds;

        private readonly float mVerticalScrollAmount = 10;
        private float mVerticalScrollHeight;
        private float mVerticalScrollPosition;

        private readonly int mSuperWidth = 500;
        private readonly int mSuperHeightPerDepth = 150;

        private BeeHiveSuperComponent mSuperComponentBeingRemoved;

        #endregion

        #region Constructors -----------------------------------------------------------

        public BeeHiveScreen()
        {
            //
            // mMenuButtonSupers
            //
            this.mMenuButtonSupers.Text = "Supers";
            this.mMenuButtonSupers.ChildMenuButtons.Add(this.mMenuButtonSupersAdd);
            this.mMenuButtonSupers.ChildMenuButtons.Add(this.mMenuButtonSupersRemove);
            //
            // mMenuButtonSupersAdd
            //
            this.mMenuButtonSupersAdd.Text = "Add";
            this.mMenuButtonSupersAdd.ChildMenuButtons.Add(this.mMenuButtonSupersAddBrood);
            this.mMenuButtonSupersAdd.ChildMenuButtons.Add(this.mMenuButtonSupersAddHoney);
            //
            // mMenuButtonSupersAdd
            //
            this.mMenuButtonSupersAddBrood.Text = "Brood";
            this.mMenuButtonSupersAddBrood.Click += this.MenuButtonSupersAddBrood_Click;
            //
            // mMenuButtonSupersAdd
            //
            this.mMenuButtonSupersAddHoney.Text = "Honey";
            this.mMenuButtonSupersAddHoney.Click += this.MenuButtonSupersAddHoney_Click;
            //
            // mMenuButtonSupersAdd
            //
            this.mMenuButtonSupersRemove.Text = "Remove";
            this.mMenuButtonSupersRemove.Click += this.MenuButtonSupersRemove_Click;
            //
            // mMenuButtonSupersAdd
            //
            this.mMenuButtonQueen.Text = "Queen";
            this.mMenuButtonQueen.ChildMenuButtons.Add(this.mMenuButtonQueenChange);
            this.mMenuButtonQueen.ChildMenuButtons.Add(this.mMenuButtonQueenRemove);
            //
            // mMenuButtonQueenChange
            //
            this.mMenuButtonQueenChange.Text = "Change";
            this.mMenuButtonQueenChange.Click += this.MenuButtonQueenChange_Click;
            //
            // mMenuButtonQueenRemove
            //
            this.mMenuButtonQueenRemove.Text = "Remove";
            this.mMenuButtonQueenRemove.Click += this.MenuButtonQueenRemove_Click;
            //
            // mMenuButtonUserSmoker
            //
            this.mMenuButtonUseSmoker.Text = "Use Smoker";
            this.mMenuButtonUseSmoker.Click += this.MenuButtonUseSmoker_Click;
            //
            // mMenuButtonSelectionCommit
            //
            this.mMenuButtonSelectionCommit.Text = "Select";
            this.mMenuButtonSelectionCommit.Click += this.MenuButtonSelectionCommit_Click;
            this.mMenuButtonSelectionCommit.IsVisible = false;
            //
            // mMenuButtonSelectionCancel
            //
            this.mMenuButtonSelectionCancel.Text = "Cancel";
            this.mMenuButtonSelectionCancel.Click += this.MenuButtonSelectionCancel_Click;
            this.mMenuButtonSelectionCancel.IsVisible = false;
            //
            // mMenuButtonSelectionCancel
            //
            this.mMenuButtonSelectionCancel.Text = "Cancel";
            this.mMenuButtonSelectionCancel.Click += this.MenuButtonSelectionCancel_Click;
            //
            // mMenuButtonToYard
            //
            this.mMenuButtonToYard.Text = "To Yard";
            this.mMenuButtonToYard.Click += this.MenuButtonToYard_Click;
            //
            // mMenuButtonToYard
            //
            this.mMenuButtonToWorld.Text = "Travel";
            this.mMenuButtonToWorld.Click += this.MenuButtonToWorld_Click;
        }

        #endregion

        #region Instance Properties ----------------------------------------------------
        
        #endregion

        #region Instance Methods -------------------------------------------------------

        private void AddSuper(InventoryItem item, SuperType type)
        {
            var lSuper = (Super) item.Tag;

            this.mBeeHive.Supers.Add(lSuper, type);
            this.mPlayer.Supers.Remove(lSuper);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);

            spriteBatch.Draw(this.mBlankTexture, Vector2.Zero, null, Color.Black, 0, Vector2.Zero, this.ScreenSize, SpriteEffects.None, 0);

            for (int lIndex = this.mComponents.Count - 1; lIndex >= 0; lIndex--)
            {
                this.mComponents[lIndex].Draw(spriteBatch, gameTime);
            }

            foreach (var lSuperComponent in this.mSuperComponents)
            {
                var lSuperIndex = (int) lSuperComponent.Tag;
                var lSuper = this.mBeeHive.Supers[lSuperIndex];
                if (lSuper.Type == SuperType.HoneyCollection)
                {
                    var lHoneyCollectionText = string.Concat("Honey Collected : ", lSuper.HoneyCollected);
                    var lHoneyCollectionTextLocation = lSuperComponent.Position + new Vector2(5);
                    spriteBatch.DrawString(this.mMetaInfoFont, lHoneyCollectionText, lHoneyCollectionTextLocation, Color.Green);
                }
            }

            const float lcTextMarginBottom = 1f;

            var lPopulationText = string.Concat("Population : ", this.mBeeHive.Population);
            var lPopulationTextSize = this.mMetaInfoFont.MeasureString(lPopulationText);
            var lPopulationTextPosition = new Vector2(10, 10);

            var lColonyStrengthText = string.Concat("Colony Strength : ", this.mBeeHive.ColonyStrength);
            var lColonyStrengthTextSize = this.mMetaInfoFont.MeasureString(lColonyStrengthText);
            var lColonyStrengthTextPosition = new Vector2(
                lPopulationTextPosition.X,
                lPopulationTextPosition.Y + lPopulationTextSize.Y + lcTextMarginBottom);

            var lColonyAggressivenessText = string.Concat("Colony Agressiveness : ", this.mBeeHive.ColonyAgressiveness);
            var lColonyAggressivenessTextSize = this.mMetaInfoFont.MeasureString(lColonyAggressivenessText);
            var lColonyAggressivenessTextPosition = new Vector2(
                lColonyStrengthTextPosition.X,
                lColonyStrengthTextPosition.Y + lColonyStrengthTextSize.Y + lcTextMarginBottom);

            var lColonySwarmLiklinessText = string.Concat("Colony Swarm Likliness : ", this.mBeeHive.ColonySwarmLikeliness);
            var lColonySwarmLiklinessTextPosition = new Vector2(
                lColonyAggressivenessTextPosition.X,
                lColonyAggressivenessTextPosition.Y + lColonyAggressivenessTextSize.Y + lcTextMarginBottom);

            spriteBatch.DrawString(this.mMetaInfoFont, lPopulationText, lPopulationTextPosition, Color.Green);
            spriteBatch.DrawString(this.mMetaInfoFont, lColonyStrengthText, lColonyStrengthTextPosition, Color.Green);
            spriteBatch.DrawString(this.mMetaInfoFont, lColonyAggressivenessText, lColonyAggressivenessTextPosition, Color.Green);
            spriteBatch.DrawString(this.mMetaInfoFont, lColonySwarmLiklinessText, lColonySwarmLiklinessTextPosition, Color.Green);
        }

        public override void HandleInput(InputState inputState)
        {
            base.HandleInput(inputState);

            this.mInventorySelectorComponent.HandleInput(inputState);

            // Must handle input backwards to make the topmost component
            // the first to have a change to handle the input.
            for (int lIndex = this.mComponents.Count - 1; lIndex >= 0; lIndex--)
            {
                var lComponent = this.mComponents[lIndex];
                lComponent.HandleInput(inputState);
            }

            if (this.mIsSelectionModeActive && inputState.MouseLeftClickUp())
            {
                var lCurrentMouseState = inputState.CurrentMouseState;
                for (int lIndex = 0; lIndex < this.mSuperComponents.Count; lIndex++)
                {
                    var lSuperComponent = this.mSuperComponents[lIndex];
                    if (VectorUtilities.HitTest(lSuperComponent.Position, lSuperComponent.Size, lCurrentMouseState.X, lCurrentMouseState.Y))
                    {
                        lSuperComponent.IsSelected = true;
                        if (this.mSelectedSuper != null) this.mSelectedSuper.IsSelected = false;
                        this.mSelectedSuper = lSuperComponent;
                        break;
                    }
                }
            }

            if (!this.mInventorySelectorComponent.Visible)
            {
                var lCurrentMouseState = inputState.CurrentMouseState;
                if (this.mVerticalScrollUpBounds.Contains(lCurrentMouseState.X, lCurrentMouseState.Y))
                {
                    this.mVerticalScrollPosition = Math.Min(
                        this.mVerticalScrollPosition + this.mVerticalScrollAmount,
                        this.mVerticalScrollHeight);
                }
                else if (this.mVerticalScrollDownBounds.Contains(lCurrentMouseState.X, lCurrentMouseState.Y))
                {
                    this.mVerticalScrollPosition = Math.Max(
                        this.mVerticalScrollPosition - this.mVerticalScrollAmount, 0);
                }
            }
        }

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

            this.mButtonMenuComponent = new ButtonMenuComponent(this.ScreenSize);
            this.mButtonMenuComponent.LoadContent(this.ContentManager);
            this.mButtonMenuComponent.MenuButtons.Add(this.mMenuButtonSupers);
            this.mButtonMenuComponent.MenuButtons.Add(this.mMenuButtonQueen);
            this.mButtonMenuComponent.MenuButtons.Add(this.mMenuButtonUseSmoker);
            this.mButtonMenuComponent.MenuButtons.Add(this.mMenuButtonToYard);
            this.mButtonMenuComponent.MenuButtons.Add(this.mMenuButtonToWorld);
            this.mButtonMenuComponent.MenuButtons.Add(this.mMenuButtonSelectionCommit);
            this.mButtonMenuComponent.MenuButtons.Add(this.mMenuButtonSelectionCancel);

            this.mHudComponent = new BeeHiveHudComponent(this.ScreenManager.BeeWorldManager, this.ScreenSize);
            this.mHudComponent.LoadContent(this.ContentManager);

            this.mVerticalScrollUpBounds = new Rectangle(
                (int)((this.ScreenSize.X - this.mSuperWidth) / 2f), 0,
                this.mSuperWidth, this.mScrollBoundsHeight);
            this.mVerticalScrollDownBounds = new Rectangle(
                this.mVerticalScrollUpBounds.X,
                (int)this.mHudComponent.Position.Y - this.mVerticalScrollUpBounds.Height,
                this.mVerticalScrollUpBounds.Width,
                this.mVerticalScrollUpBounds.Height + (int)this.mHudComponent.Size.Y);

            this.mInventorySelectorComponent = new InventoryItemSelectorComponent(this.ScreenSize);
            this.mInventorySelectorComponent.LoadContent(this.ContentManager);

            this.mSuperTexture = new Texture2D(this.ScreenManager.Game.GraphicsDevice, 128, 128);
            this.mSuperTexture.SetData(Enumerable.Repeat(Color.Pink, 128 * 128).ToArray());

            this.mComponents.Add(this.mInventorySelectorComponent);
            this.mComponents.Add(this.mButtonMenuComponent);
            this.mComponents.Add(this.mHudComponent);
        }

        private void MenuButtonQueenChange_Click(MenuButton obj)
        {
            this.ScreenManager.BeeWorldManager.IsPaused = true;

            this.mInventorySelectorComponent.Items.Clear();
            this.mInventorySelectorComponent.Items.AddRange(
                this.mPlayer.QueenBees.Select(ToInventoryItem));

            this.mInventorySelectorComponent.SelectActionText = "Place Queen";
            this.mInventorySelectorComponent.Show(
                this.ChangeQueen, () => this.ScreenManager.BeeWorldManager.IsPaused = false);
        }

        private void ChangeQueen(InventoryItem inventoryItem)
        {
            this.mButtonMenuComponent.IsVisible = false;
            var lQueenBee = (QueenBee) inventoryItem.Tag;
            this.mBeeHiveManager.RemoveQueen(() => this.AddQueen(lQueenBee));
        }

        private void AddQueen(QueenBee queenBee)
        {
            this.mBeeHiveManager.AddQueen(queenBee, this.QueenAddComplete);
        }

        private void QueenAddComplete()
        {
            this.mButtonMenuComponent.IsVisible = true;   
        }

        private void MenuButtonUseSmoker_Click(MenuButton button)
        {
            this.mButtonMenuComponent.IsVisible = false;
            this.mBeeHiveManager.SmokeHive(this.mPlayer.Smoker, this.SmokingFinished);
        }

        private void MenuButtonQueenRemove_Click(MenuButton obj)
        {
            this.mButtonMenuComponent.IsVisible = false;
            this.mBeeHiveManager.RemoveQueen(this.QueenRemovalFinished);
        }

        private void QueenRemovalFinished()
        {
            this.mButtonMenuComponent.IsVisible = true;
        }

        private void MenuButtonToWorld_Click(MenuButton button)
        {
            var lWorldScreen = new BeeWorldScreen();
            this.ScreenManager.TransitionTo(lWorldScreen);
        }

        private void MenuButtonToYard_Click(MenuButton button)
        {
            var lYardScreen = new BeeYardScreen();
            var lPlayerManager = this.ScreenManager.BeeWorldManager.PlayerManager;
            lPlayerManager.TravelToBeeYard();
            this.ScreenManager.TransitionTo(lYardScreen);
        }

        private void MenuButtonSupersAddHoney_Click(MenuButton obj)
        {
            this.ScreenManager.BeeWorldManager.IsPaused = true;

            this.mInventorySelectorComponent.Items.Clear();
            this.mInventorySelectorComponent.Items.AddRange(
                this.mPlayer.Supers
                    .Where(x => x.Type != SuperType.BroodChamber)
                    .Select(ToInventoryItem));

            this.mInventorySelectorComponent.SelectActionText = "Add Super";
            this.mInventorySelectorComponent.Show(
                x => this.AddSuper(x, SuperType.HoneyCollection),
                () => this.ScreenManager.BeeWorldManager.IsPaused = false);
        }

        private void MenuButtonSupersAddBrood_Click(MenuButton obj)
        {
            this.ScreenManager.BeeWorldManager.IsPaused = true;

            this.mInventorySelectorComponent.Items.Clear();
            this.mInventorySelectorComponent.Items.AddRange(
                this.mPlayer.Supers
                    .Where(x => x.Type != SuperType.HoneyCollection)
                    .Select(ToInventoryItem));

            this.mInventorySelectorComponent.SelectActionText = "Add Super";
            this.mInventorySelectorComponent.Show(
                x => this.AddSuper(x, SuperType.BroodChamber),
                () => this.ScreenManager.BeeWorldManager.IsPaused = false);
        }

        private void MenuButtonSupersRemove_Click(MenuButton button)
        {
            this.BeginSelectionMode(this.RemoveSuper);
        }

        private void MenuButtonSelectionCommit_Click(MenuButton obj)
        {
            System.Diagnostics.Debug.Assert(this.mSuperSelectedCallback != null);
            this.mSuperSelectedCallback(this.mSelectedSuper);
        }

        private void MenuButtonSelectionCancel_Click(MenuButton obj)
        {
            this.EndSelectionMode();
        }

        private void RemoveSuper(BeeHiveSuperComponent superComponet)
        {
            var lIndex = (int)superComponet.Tag;
            var lSuper = this.mBeeHive.Supers[lIndex];

            this.mButtonMenuComponent.IsVisible = false;

            this.mSuperComponentBeingRemoved = superComponet;
            this.mBeeHiveManager.RemoveSuper(lSuper, this.SuperRemovalComplete);
        }

        public void BeginSelectionMode(Action<BeeHiveSuperComponent> superSelectedCallback)
        {
            System.Diagnostics.Debug.Assert(superSelectedCallback != null);
            this.mIsSelectionModeActive = true;
            this.mSuperSelectedCallback = superSelectedCallback;
            this.mSelectedSuper = null;

            this.mMenuButtonSelectionCancel.IsVisible = true;
            this.mMenuButtonSelectionCommit.IsVisible = true;

            this.mMenuButtonSupers.IsVisible = false;
            this.mMenuButtonQueen.IsVisible = false;
            this.mMenuButtonUseSmoker.IsVisible = false;
            this.mMenuButtonToYard.IsVisible = false;
            this.mMenuButtonToWorld.IsVisible = false;
        }

        public void EndSelectionMode()
        {
            if (this.mSelectedSuper != null) this.mSelectedSuper.IsSelected = false;
            this.mIsSelectionModeActive = false;
            this.mSuperSelectedCallback = null;
            this.mSelectedSuper = null;

            this.mMenuButtonSelectionCancel.IsVisible = false;
            this.mMenuButtonSelectionCommit.IsVisible = false;

            this.mMenuButtonSupers.IsVisible = true;
            this.mMenuButtonQueen.IsVisible = true;
            this.mMenuButtonUseSmoker.IsVisible = true;
            this.mMenuButtonToYard.IsVisible = true;
            this.mMenuButtonToWorld.IsVisible = true;
        }

        public void SuperRemovalComplete()
        {
            this.mButtonMenuComponent.IsVisible = true;
            this.EndSelectionMode();
        }

        private void SmokingFinished()
        {
            this.mButtonMenuComponent.IsVisible = true;
        }

        #region Inventory Item Convertion Methods

        private InventoryItem ToInventoryItem(Super super)
        {
            var lInventoryItem = new InventoryItem();
            lInventoryItem.Tag = super;
            lInventoryItem.Name = super.Name;
            lInventoryItem.Description = super.Description;
            lInventoryItem.Quantity = 1;
            lInventoryItem.Texture = this.mSuperTexture;
            return lInventoryItem;
        }

        private InventoryItem ToInventoryItem(QueenBee queenBee)
        {
            var lInventoryItem = new InventoryItem();
            lInventoryItem.Tag = queenBee;
            lInventoryItem.Name = queenBee.Name;
            lInventoryItem.Description = queenBee.Description;
            lInventoryItem.Quantity = 1;
            lInventoryItem.Texture = this.mSuperTexture;
            return lInventoryItem;
        }

        #endregion

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            for (int lIndex = this.mComponents.Count - 1; lIndex >= 0; lIndex--)
            {
                this.mComponents[lIndex].Update(gameTime);
            }

            while (this.mSuperComponents.Count > this.mBeeHive.Supers.Count)
            {
                var lLastIndex = this.mSuperComponents.Count - 1;
                var lSuperComponent = this.mSuperComponents[lLastIndex];
                this.mSuperComponents.RemoveAt(lLastIndex);
                this.mComponents.Remove(lSuperComponent);
            }

            while (this.mSuperComponents.Count < this.mBeeHive.Supers.Count)
            {
                var lSuperComponent = new BeeHiveSuperComponent();
                lSuperComponent.IsSelected = false;
                lSuperComponent.Tag = this.mSuperComponents.Count;
                lSuperComponent.BlankTexture = this.mBlankTexture;
                this.mSuperComponents.Add(lSuperComponent);
                this.mComponents.Add(lSuperComponent);
            }

            var lSuperBottomLeftLocation = new Vector2(
                (this.ScreenSize.X - mSuperWidth) / 2,
                this.mHudComponent.Position.Y - 10f + this.mVerticalScrollPosition);

            this.mVerticalScrollHeight = -(this.mHudComponent.Position.Y / 2f);
            for (int lIndex = 0; lIndex < this.mBeeHive.Supers.Count; lIndex++)
            {
                var lSuper = this.mBeeHive.Supers[lIndex];
                var lSuperComponent = this.mSuperComponents[lIndex];

                var lSuperComponentSize = new Vector2(mSuperWidth, mSuperHeightPerDepth * lSuper.Depth);
                var lSuperComponentPosition = new Vector2(
                    lSuperBottomLeftLocation.X,
                    lSuperBottomLeftLocation.Y - lSuperComponentSize.Y);

                lSuperComponent.Size = lSuperComponentSize;
                lSuperComponent.Position = lSuperComponentPosition;

                this.mVerticalScrollHeight += lSuperComponentSize.Y;

                lSuperBottomLeftLocation.Y = lSuperComponentPosition.Y - 1;
            }
            this.mVerticalScrollHeight = Math.Max(this.mVerticalScrollHeight, 0);
        }

        #endregion

        #region Static Methods ---------------------------------------------------------
        
        #endregion
    }
}
