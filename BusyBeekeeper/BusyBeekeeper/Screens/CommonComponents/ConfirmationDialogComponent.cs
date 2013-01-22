using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BusyBeekeeper.Screens.CommonComponents
{
    internal sealed class ConfirmationDialogComponent : ScreenComponent
    {
        #region Instance Fields --------------------------------------------------------

        private readonly Vector2 mScreenSize;

        private Vector2 mPosition;
        private Vector2 mSize;
        private readonly Color mFadeColor = new Color(0, 0, 0, 192);

        private bool mIsInvalid;

        private readonly Button mButtonCancel = new Button();
        private readonly Button mButtonConfirm = new Button();

        private Texture2D mBlankTexture;
        private SpriteFont mFont;

        private string mMessageText;
        private string mAdjustedMessageText;
        private Vector2 mMessageTextPosition;
        private bool mIsVisible;

        #endregion

        #region Static Fields ----------------------------------------------------------

        private const int sMinimumWidth = 300;
        private const int sItemMargin = 10;

        #endregion

        #region Constructors -----------------------------------------------------------

        public ConfirmationDialogComponent(Vector2 screenSize)
        {
            this.mButtonCancel.Text = "Cancel";
            this.mButtonConfirm.Text = "Okay";
            this.mScreenSize = screenSize;
        }

        #endregion

        #region Events -----------------------------------------------------------------

        public event Action<ConfirmationDialogComponent> Confirm;
        public event Action<ConfirmationDialogComponent> Cancel;

        #endregion

        #region Instance Properties ----------------------------------------------------

        public string ConfirmText { get; set; }
        public string CancelText { get; set; }

        public string MessageText
        {
            get { return this.mMessageText; }
            set
            {
                if (this.mMessageText == value) return;
                this.mMessageText = value;
                this.Invalidate();
            }
        }

        public bool IsVisible
        {
            get { return this.mIsVisible; }
            set
            {
                this.mIsVisible = value;
                this.mButtonCancel.IsVisible = value;
                this.mButtonConfirm.IsVisible = value;
            }
        }

        #endregion

        #region Instance Methods -------------------------------------------------------

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);

            this.mBlankTexture = contentManager.Load<Texture2D>("Sprites/Blank");
            this.mFont = contentManager.Load<SpriteFont>("Fonts/DefaultNormal");

            this.mButtonCancel.LoadContent(contentManager);
            this.mButtonCancel.TextColor = Color.Black;
            this.mButtonCancel.Font = this.mFont;
            this.mButtonCancel.BackgroundRenderer = new SolidBackgroundRenderer(this.mBlankTexture, Color.Red);
            this.mButtonCancel.Click += this.ButtonCancel_Click;
            this.mButtonCancel.Size = new Vector2((sMinimumWidth - (3*sItemMargin))/2, 40);

            this.mButtonConfirm.LoadContent(contentManager);
            this.mButtonConfirm.TextColor = Color.Black;
            this.mButtonConfirm.Font = this.mFont;
            this.mButtonConfirm.BackgroundRenderer = new SolidBackgroundRenderer(this.mBlankTexture, Color.Green);
            this.mButtonConfirm.Click += this.ButtonConfirm_Click;
            this.mButtonConfirm.Size = this.mButtonCancel.Size;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.mIsInvalid)
            {
                var lMessageTextSize = this.mFont.MeasureString(this.mMessageText);
                var lWidth = (lMessageTextSize.X > sMinimumWidth) ? this.mScreenSize.X/2 : sMinimumWidth;
                this.mAdjustedMessageText = this.mFont.FitTextToWidth(this.mMessageText, lWidth);
                var lAdjustedMessageTextSize = this.mFont.MeasureString(this.mAdjustedMessageText);

                this.mSize = new Vector2(
                    sItemMargin + lWidth + sItemMargin,
                    sItemMargin + lAdjustedMessageTextSize.Y + sItemMargin + this.mButtonCancel.Size.Y + sItemMargin);
                this.mPosition = (this.mScreenSize - this.mSize)/2f;

                this.mMessageTextPosition = this.mPosition + new Vector2(sItemMargin);

                this.mButtonCancel.Position = new Vector2(
                    this.mPosition.X + sItemMargin,
                    this.mPosition.Y + this.mSize.Y - sItemMargin - this.mButtonCancel.Size.Y);
                this.mButtonConfirm.Position = new Vector2(
                    this.mPosition.X + this.mSize.X - sItemMargin - this.mButtonConfirm.Size.X,
                    this.mButtonCancel.Position.Y);

                this.mIsInvalid = false;
            }

            this.mButtonCancel.Update(gameTime);
            this.mButtonConfirm.Update(gameTime);
        }

        public override void HandleInput(InputState inputState)
        {
            base.HandleInput(inputState);

            this.mButtonCancel.HandleInput(inputState);
            this.mButtonConfirm.HandleInput(inputState);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (!this.IsVisible) return;

            spriteBatch.Draw(this.mBlankTexture, Vector2.Zero, null, this.mFadeColor, 0, Vector2.Zero, this.mScreenSize, SpriteEffects.None, 0);
            spriteBatch.Draw(this.mBlankTexture, this.mPosition, null, Color.LightBlue, 0, Vector2.Zero, this.mSize, SpriteEffects.None, 0);
            spriteBatch.DrawString(this.mFont, this.mAdjustedMessageText, this.mMessageTextPosition, Color.Black);

            this.mButtonCancel.Draw(spriteBatch, gameTime);
            this.mButtonConfirm.Draw(spriteBatch, gameTime);
        }

        private void ButtonCancel_Click(Button button)
        {
            var lCancelHandler = this.Cancel;
            if (lCancelHandler != null) lCancelHandler(this);
            this.IsVisible = false;
        }

        private void ButtonConfirm_Click(Button button)
        {
            var lConfirmHandler = this.Confirm;
            if (lConfirmHandler != null) lConfirmHandler(this);
            this.IsVisible = false;
        }

        private void Invalidate()
        {
            this.mIsInvalid = true;
        }

        #endregion

    }
}
