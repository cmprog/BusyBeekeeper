using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BusyBeekeeper.Screens.CommonComponents
{
    /// <summary>
    /// Displays a list of inventory items.
    /// </summary>
    internal partial class InventoryItemListComponent : ScreenComponent
    {
        #region Instance Fields --------------------------------------------------------
        
        private const int sItemSideLength = 128;
        private const int sItemPadding = 10;
        private const int sInfoWidth = 200;
        private const int sItemsPerRow = 3;
        private const int sItemsPerColumn = 3;
        private const int sItemsPerPage = sItemsPerRow * sItemsPerColumn;
        private const int sNavigationHeight = 64;

        private readonly Vector2 mItemSize = new Vector2(sItemSideLength, sItemSideLength);
        private readonly ItemCollection mItems;
        private readonly Vector2[] mItemPositions = new Vector2[sItemsPerPage];

        private bool mInfoRecalculationNeeded;
        private readonly Vector2 mNameTextPosition;
        private string mAdjustedNameText;
        private Vector2 mDescriptionTextPosition;
        private string mAdjustedDescriptionText;

        private int mCurrentPageIndex = 0;
        private int mSelectedIndexOffset = -1;

        private readonly Vector2 mScreenSize;
        private readonly Vector2 mSize = new Vector2(
            (sItemsPerRow * sItemSideLength) + ((sItemsPerRow + 2) * sItemPadding) + sInfoWidth,
            (sItemsPerColumn * sItemSideLength) + ((sItemsPerColumn + 2) * sItemPadding) + sNavigationHeight);
        private readonly Vector2 mPosition;
        private readonly Color mFadeColor = new Color(0, 0, 0, 192);

        private SpriteFont mFontName;
        private SpriteFont mFontDescription;

        private Texture2D mBlankTexture;

        private Button mButtonClose = new Button();
        private Button mButtonPrevious = new Button();
        private Button mButtonNext = new Button();
        private Label mLabelPage = new Label();

        #endregion

        #region Constructors -----------------------------------------------------------
        
        /// <summary>
        /// Initializes a new instance of the InventoryItemListComponent class.
        /// </summary>
        /// <param name="screenSize"></param>
        public InventoryItemListComponent(Vector2 screenSize)
        {
            this.mScreenSize = screenSize;
            this.mPosition = (screenSize - this.mSize) / 2f;
            this.mItems = new ItemCollection(this);

            for (int lIndex = 0; lIndex < sItemsPerPage; lIndex++)
            {
                int lRowIndex = lIndex / 3;
                int lColumnIndex = lIndex % 3;

                var lItemOffset = new Vector2(
                    sItemPadding + (this.mItemSize.X + sItemPadding) * lColumnIndex,
                    sItemPadding + (this.mItemSize.Y + sItemPadding) * lRowIndex);
                this.mItemPositions[lIndex] = this.mPosition + lItemOffset;
            }

            this.mNameTextPosition = new Vector2(
                this.mPosition.X + this.mSize.X - sItemPadding - sInfoWidth,
                this.mPosition.Y + sItemPadding);
        }

        #endregion

        #region Instance Properties ----------------------------------------------------

        protected Vector2 ScreenSize { get { return this.mScreenSize; } }
        public Vector2 Size { get { return this.mSize; } }
        public Vector2 Position { get { return this.mPosition; } }

        protected Vector2 CloseButtonPosition { get { return this.mButtonClose.Position; } }
        protected Vector2 CloseButtonSize { get { return this.mButtonClose.Size; } }

        public bool Visible { get; set; }

        public int SelectedIndex
        {
            get
            {
                return
                    (sItemsPerPage * this.mCurrentPageIndex) +
                    this.mSelectedIndexOffset;
            }
        }

        public ItemCollection Items
        {
            get { return this.mItems; }
        }

        #endregion

        #region Instance Methods -------------------------------------------------------

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);

            this.mBlankTexture = contentManager.Load<Texture2D>("Sprites/Blank");

            this.mFontName = contentManager.Load<SpriteFont>("Fonts/DefaultBig");
            this.mFontDescription = contentManager.Load<SpriteFont>("Fonts/DefaultNormal");

            this.mButtonClose.Text = "Close";
            this.mButtonClose.TextColor = Color.White;
            this.mButtonClose.BackgroundRenderer = new SolidBackgroundRenderer(this.mBlankTexture, Color.DarkRed);
            this.mButtonClose.Size = new Vector2(sInfoWidth, 50);
            this.mButtonClose.Position = this.Position + this.Size - this.mButtonClose.Size - new Vector2(sItemPadding, sItemPadding);
            this.mButtonClose.Font = contentManager.Load<SpriteFont>("Fonts/DefaultNormal");
            this.mButtonClose.Click += x => this.Close();

            this.mButtonPrevious.Text = "<";
            this.mButtonPrevious.TextColor = Color.White;
            this.mButtonPrevious.BackgroundRenderer = new SolidBackgroundRenderer(this.mBlankTexture, Color.Orange);
            this.mButtonPrevious.Size = new Vector2(sNavigationHeight, sNavigationHeight);
            this.mButtonPrevious.Position = new Vector2(
                this.mPosition.X + sItemPadding,
                this.mPosition.Y + this.mSize.Y - sItemPadding - this.mButtonPrevious.Size.Y);
            this.mButtonPrevious.Font = contentManager.Load<SpriteFont>("Fonts/DefaultBig");
            this.mButtonPrevious.Click += x => this.ChangePage(-1);

            this.mButtonNext.Text = ">";
            this.mButtonNext.TextColor = Color.White;
            this.mButtonNext.BackgroundRenderer = new SolidBackgroundRenderer(this.mBlankTexture, Color.Orange);
            this.mButtonNext.Size = this.mButtonPrevious.Size;
            this.mButtonNext.Position = new Vector2(
                this.mPosition.X + this.mSize.X - sItemPadding - sInfoWidth - sItemPadding - this.mButtonNext.Size.X,
                this.mButtonPrevious.Position.Y);
            this.mButtonNext.Font = contentManager.Load<SpriteFont>("Fonts/DefaultBig");
            this.mButtonNext.Click += x => this.ChangePage(1);

            this.mLabelPage.TextColor = Color.Black;
            this.mLabelPage.BackgroundRenderer = new EmptyBackgroundRenderer();
            this.mLabelPage.Size = new Vector2(
                this.mButtonNext.Position.X - this.mButtonPrevious.Position.X - this.mButtonPrevious.Size.X - sItemPadding - sItemPadding,
                this.mButtonNext.Size.Y);
            this.mLabelPage.Position = new Vector2(
                this.mButtonPrevious.Position.X + this.mButtonPrevious.Size.X + sItemPadding,
                this.mButtonPrevious.Position.Y);
            this.mLabelPage.Font = contentManager.Load<SpriteFont>("Fonts/DefaultSmall");

            this.UpdateNavigationInformation();
        }

        public override bool HandleInput(InputState inputState)
        {
            if (!this.Visible) return false;

            if (inputState.MouseLeftClickUp())
            {
                var lCurrentMouseState = inputState.CurrentMouseState;
                if (VectorUtilities.HitTest(Vector2.Zero, this.ScreenSize, lCurrentMouseState.X, lCurrentMouseState.Y)
                    && !VectorUtilities.HitTest(this.mPosition, this.mSize, lCurrentMouseState.X, lCurrentMouseState.Y))
                {
                    this.Close();
                    return true;
                }

                for (int lIndex = 0; lIndex < sItemsPerPage; lIndex++)
                {
                    if (VectorUtilities.HitTest(this.mItemPositions[lIndex], this.mItemSize, lCurrentMouseState.X, lCurrentMouseState.Y))
                    {
                        this.mSelectedIndexOffset = lIndex;
                        this.mInfoRecalculationNeeded = true;
                        return true;
                    }
                }
            }

            if (this.mButtonClose.HandleInput(inputState)) return true;
            if (this.mButtonPrevious.HandleInput(inputState)) return true;
            if (this.mButtonNext.HandleInput(inputState)) return true;

            return false;
        }

        private string FitTextToWidth(
            SpriteFont font,
            string originalText,
            float maxWidth)
        {
            var lTextSize = font.MeasureString(originalText);
            if (lTextSize.X < maxWidth)
            {
                return originalText;
            }

            var lTextBuilder = new StringBuilder();
            var lLineBuilder = new StringBuilder();

            var lSpaceSize = font.MeasureString(" ");
            var lWords = originalText.Split(' ');
            var lLineSize = Vector2.Zero;

            foreach (var lWord in lWords)
            {
                var lWordSize = font.MeasureString(lWord);

                if (lLineSize.X + lSpaceSize.X + lWordSize.X > maxWidth)
                {
                    lTextBuilder.AppendLine(lLineBuilder.ToString());
                    lLineBuilder.Clear();
                }

                lLineBuilder.Append(lWord).Append(' ');
                lLineSize = font.MeasureString(lLineBuilder);
            }

            if (lLineBuilder.Length > 0)
            {
                lTextBuilder.Append(lLineBuilder.ToString());
            }

            return lTextBuilder.ToString();
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (!this.Visible) return;

            spriteBatch.Draw(this.mBlankTexture, Vector2.Zero, null, this.mFadeColor, 0, Vector2.Zero, this.mScreenSize, SpriteEffects.None, 0);
            spriteBatch.Draw(this.mBlankTexture, this.mPosition, null, Color.Gold, 0, Vector2.Zero, this.mSize, SpriteEffects.None, 0);

            this.mButtonClose.Draw(spriteBatch, gameTime);
            this.mButtonNext.Draw(spriteBatch, gameTime);
            this.mButtonPrevious.Draw(spriteBatch, gameTime);
            this.mLabelPage.Draw(spriteBatch, gameTime);

            if (this.mSelectedIndexOffset >= 0)
            {
                if (this.mInfoRecalculationNeeded)
                {
                    var lSelectedItemIndex = (this.mCurrentPageIndex * sItemsPerPage) + this.mSelectedIndexOffset;
                    var lSelectedItem = this.mItems[lSelectedItemIndex];
                    
                    this.mAdjustedNameText = this.FitTextToWidth(this.mFontName, lSelectedItem.Name, sInfoWidth);
                    var lNameTextSize = this.mFontName.MeasureString(this.mAdjustedNameText);

                    this.mAdjustedDescriptionText = this.FitTextToWidth(this.mFontDescription, lSelectedItem.Description, sInfoWidth);
                    this.mDescriptionTextPosition = new Vector2(this.mNameTextPosition.X, this.mNameTextPosition.Y + lNameTextSize.Y + sItemPadding);

                    this.mInfoRecalculationNeeded = false;
                }

                spriteBatch.DrawString(this.mFontName, this.mAdjustedNameText, this.mNameTextPosition, Color.Black);
                spriteBatch.DrawString(this.mFontDescription, this.mAdjustedDescriptionText, this.mDescriptionTextPosition, Color.Black);
            }

            for (int lItemOffset = 0; lItemOffset < sItemsPerPage; lItemOffset++)
            {
                var lItemPosition = this.mItemPositions[lItemOffset];
                spriteBatch.Draw(this.mBlankTexture, lItemPosition, null, Color.White, 0, Vector2.Zero, this.mItemSize, SpriteEffects.None, 0);

                var lItemIndex = (this.mCurrentPageIndex * sItemsPerPage) + lItemOffset;
                if (lItemIndex < this.Items.Count)
                {
                    var lItem = this.mItems[lItemIndex];
                    spriteBatch.Draw(lItem.Texture, lItemPosition, Color.White);
                }

                if (lItemOffset == this.mSelectedIndexOffset)
                {
                    var lTopLeft = this.mItemPositions[lItemOffset] - new Vector2(1, 1);
                    var lTopRight = new Vector2(lTopLeft.X + this.mItemSize.X + 2, lTopLeft.Y);
                    var lBottomLeft = new Vector2(lTopLeft.X, lTopLeft.Y + this.mItemSize.Y + 2);

                    var lWidthSize = new Vector2(this.mItemSize.X + 4, 3);
                    var lHeightSize = new Vector2(3, this.mItemSize.Y + 4);

                    spriteBatch.Draw(this.mBlankTexture, lTopLeft, null, Color.Black, 0, Vector2.Zero, lWidthSize, SpriteEffects.None, 0);
                    spriteBatch.Draw(this.mBlankTexture, lTopLeft, null, Color.Black, 0, Vector2.Zero, lHeightSize, SpriteEffects.None, 0);
                    spriteBatch.Draw(this.mBlankTexture, lTopRight, null, Color.Black, 0, Vector2.Zero, lHeightSize, SpriteEffects.None, 0);
                    spriteBatch.Draw(this.mBlankTexture, lBottomLeft, null, Color.Black, 0, Vector2.Zero, lWidthSize, SpriteEffects.None, 0);
                }
            }
        }

        private void UpdateNavigationInformation()
        {
            if (this.Items.Count == 0)
            {
                this.mLabelPage.Text = "No Items";
                this.mButtonNext.IsEnabled = false;
                this.mButtonPrevious.IsEnabled = false;
            }
            else
            {
                var lPageCount = ((this.Items.Count - 1) / sItemsPerPage) + 1;
                this.mLabelPage.Text = string.Concat(this.mCurrentPageIndex + 1, " / ", lPageCount);

                this.mButtonPrevious.IsEnabled = this.mCurrentPageIndex > 0;
                this.mButtonNext.IsEnabled = this.mCurrentPageIndex + 1 < lPageCount;
            }
        }

        private void ChangePage(int pageOffset)
        {
            this.mCurrentPageIndex += pageOffset;
            this.UpdateNavigationInformation();
            this.mInfoRecalculationNeeded = true;
        }

        protected virtual void Close()
        {
            this.Visible = false;
        }

        #endregion
    }
}
