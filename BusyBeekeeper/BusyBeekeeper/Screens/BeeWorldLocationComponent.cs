using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BusyBeekeeper.Screens
{
    internal class BeeWorldLocationComponent : ScreenComponent
    {
        #region Instance Fields --------------------------------------------------------

        private Texture2D mBlankTexture;
        private SpriteFont mFont;
        private bool mIsEnabled;
        private Color mNameBackColor;

        #endregion
        
        #region Constructors -----------------------------------------------------------

        public BeeWorldLocationComponent()
        {
            this.mIsEnabled = false;
            this.mNameBackColor = Color.LightGoldenrodYellow;
        }

        #endregion
        
        #region Instance Properties ----------------------------------------------------

        public string NameText { get; set; }
        public Vector2 NamePosition { get; set; }
        public Vector2 NameSize { get; set; }

        public bool IsEnabled
        {
            get { return this.mIsEnabled; }
            set
            {
                if (this.mIsEnabled == value) return;
                this.mIsEnabled = value;
                this.mNameBackColor = value ? Color.Goldenrod : Color.LightGoldenrodYellow;
            }
        }

        public event Action<BeeWorldLocationComponent> Click;

        #endregion
        
        #region Instance Methods -------------------------------------------------------

        public override void LoadContent(ContentManager contentManager)
        {
            this.mBlankTexture = contentManager.Load<Texture2D>("Sprites/Blank");
            this.mFont = contentManager.Load<SpriteFont>("Fonts/DefaultSmall");
        }

        public override void HandleInput(InputState inputState)
        {
            if (!this.IsEnabled) return;

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

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var lNamePosition = this.NamePosition;
            var lNameSize = this.NameSize;

            var lNameText = this.NameText;
            var lNameTextSize = this.mFont.MeasureString(lNameText);
            var lNameTextPosition = lNamePosition + ((lNameSize - lNameTextSize)/2f);

            spriteBatch.Draw(this.mBlankTexture, lNamePosition, null, this.mNameBackColor, 0, Vector2.Zero, lNameSize, SpriteEffects.None, 0);
            spriteBatch.DrawString(this.mFont, lNameText, lNameTextPosition, Color.Black);
        }

        private bool HitTest(int x, int y)
        {
            return VectorUtilities.HitTest(this.NamePosition, this.NameSize, x, y);
        }

        #endregion
    }
}
