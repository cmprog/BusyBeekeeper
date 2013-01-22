using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BusyBeekeeper.Screens
{
    internal sealed class ShopSuperSectionItemComponent : ScreenComponent
    {
        #region Instance Fields --------------------------------------------------------

        private Texture2D mBlankTexture;

        private bool mAreCalculationsValid;
        private Vector2 mSize;
        private Vector2 mPosition;

        private Vector2 mIconPosition;
        private Vector2 mIconSize = new Vector2(128);

        private Vector2 mNameTextPosition;
        private Vector2 mDescriptionTextPosition;
        private Vector2 mPriceTextPosition;

        private string mNameText;
        private string mAdjustedNameText;
        private string mDescriptionText;
        private string mAdjustedDescriptionText;
        private int mPrice;
        private string mPriceText;

        private SpriteFont mFontName;
        private SpriteFont mFontDescription;
        private SpriteFont mFontPrice;

        #endregion

        #region Static Fields ----------------------------------------------------------

        private const int sItemMargin = 10;

        #endregion

        #region Events -----------------------------------------------------------------

        public event Action<ShopSuperSectionItemComponent> Click;

        #endregion

        #region Constructors -----------------------------------------------------------

        #endregion

        #region Instance Properties ----------------------------------------------------

        public Vector2 Position
        {
            get { return this.mPosition; }
            set
            {
                if (this.mPosition == value) return;
                this.mPosition = value;
                this.Invalidate();
            }
        }

        public Vector2 Size
        {
            get { return this.mSize; }
            set
            {
                if (this.mSize == value) return;
                this.mSize = value;
                this.Invalidate();
            }
        }

        public Texture2D IconTexture { get; set; }

        public string NameText
        {
            get { return this.mNameText; }
            set
            {
                if (this.mNameText == value) return;
                this.mNameText = value;
                this.Invalidate();
            }
        }

        public string DescriptionText
        {
            get { return this.mDescriptionText; }
            set
            {
                if (this.mDescriptionText == value) return;
                this.mDescriptionText = value;
                this.Invalidate();
            }
        }

        public int Price
        {
            get { return this.mPrice; }
            set
            {
                if (this.mPrice == value) return;
                this.mPrice = value;
                this.Invalidate();
            }
        }

        public bool IsSelected { get; set; }

        public Color IconTint { get; set; }

        #endregion

        #region Instance Methods -------------------------------------------------------

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);

            this.mBlankTexture = contentManager.Load<Texture2D>("Sprites/Blank");
            this.mFontName = contentManager.Load<SpriteFont>("Fonts/DefaultSmall");
            this.mFontDescription = this.mFontName;
            this.mFontPrice = this.mFontDescription;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);

            spriteBatch.Draw(this.mBlankTexture, this.mPosition, null, Color.PaleGoldenrod, 0, Vector2.Zero, this.mSize, SpriteEffects.None, 0);
            spriteBatch.Draw(this.IconTexture, this.mIconPosition, null, this.IconTint, 0, Vector2.Zero, this.mIconSize, SpriteEffects.None, 0);
            spriteBatch.DrawString(this.mFontName, this.mAdjustedNameText, this.mNameTextPosition, Color.Black);
            spriteBatch.DrawString(this.mFontDescription, this.mAdjustedDescriptionText, this.mDescriptionTextPosition, Color.Black);
            spriteBatch.DrawString(this.mFontPrice, this.mPriceText, this.mPriceTextPosition, Color.Black);

            if (this.IsSelected)
            {
                var lTopLeft = this.mIconPosition - new Vector2(1, 1);
                var lTopRight = new Vector2(lTopLeft.X + this.mIconSize.X + 2, lTopLeft.Y);
                var lBottomLeft = new Vector2(lTopLeft.X, lTopLeft.Y + this.mIconSize.Y + 2);

                var lWidthSize = new Vector2(this.mIconSize.X + 4, 3);
                var lHeightSize = new Vector2(3, this.mIconSize.Y + 4);

                spriteBatch.Draw(this.mBlankTexture, lTopLeft, null, Color.Black, 0, Vector2.Zero, lWidthSize, SpriteEffects.None, 0);
                spriteBatch.Draw(this.mBlankTexture, lTopLeft, null, Color.Black, 0, Vector2.Zero, lHeightSize, SpriteEffects.None, 0);
                spriteBatch.Draw(this.mBlankTexture, lTopRight, null, Color.Black, 0, Vector2.Zero, lHeightSize, SpriteEffects.None, 0);
                spriteBatch.Draw(this.mBlankTexture, lBottomLeft, null, Color.Black, 0, Vector2.Zero, lWidthSize, SpriteEffects.None, 0);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!this.mAreCalculationsValid)
            {
                this.mIconSize = new Vector2(Math.Min(this.mSize.X, this.mSize.Y) - (2 * sItemMargin));
                var lIconPositionY = this.mPosition.Y + ((this.mSize.Y - this.mIconSize.Y) / 2f);
                this.mIconPosition = new Vector2(this.mPosition.X + sItemMargin, lIconPositionY);

                var lTextWidth = this.mSize.X - (3*sItemMargin) - this.mIconSize.X;
                this.mNameTextPosition = new Vector2(
                    this.mPosition.X + sItemMargin + this.mIconSize.X + sItemMargin,
                    this.mIconPosition.Y);
                this.mAdjustedNameText = this.mFontName.FitTextToWidth(this.mNameText, lTextWidth);
                var lNameTextSize = this.mFontName.MeasureString(this.mAdjustedNameText);

                this.mDescriptionTextPosition = new Vector2(
                    this.mNameTextPosition.X,
                    this.mNameTextPosition.Y + lNameTextSize.Y);
                this.mAdjustedDescriptionText = this.mFontDescription.FitTextToWidth(this.mDescriptionText, lTextWidth);
                var lDescriptionTextSize = this.mFontDescription.MeasureString(this.mAdjustedDescriptionText);

                this.mPriceTextPosition = new Vector2(
                    this.mDescriptionTextPosition.X,
                    this.mDescriptionTextPosition.Y + lDescriptionTextSize.Y);
                this.mPriceText = string.Concat(this.Price, " coins");

                this.mAreCalculationsValid = true;
            }
        }

        public override void HandleInput(InputState inputState)
        {
            if (inputState.MouseLeftClickUp())
            {
                var lCurrentMouseState = inputState.CurrentMouseState;
                if (this.HitTest(lCurrentMouseState.X, lCurrentMouseState.Y))
                {
                    var lClickHandler = this.Click;
                    if (lClickHandler != null) lClickHandler(this);
                    inputState.MouseLeftClickUpHandled = true;
                }
            }
        }

        private bool HitTest(int x, int y)
        {
            return VectorUtilities.HitTest(
                this.mPosition, this.mSize, x, y);
        }

        private void Invalidate()
        {
            this.mAreCalculationsValid = false;
        }

        #endregion
        
    }
}
