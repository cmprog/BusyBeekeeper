using System;
using BusyBeekeeper.Data.Graphics.Shop;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BusyBeekeeper.Screens
{
    internal sealed class ShopSectionComponent : ScreenComponent
    {
        #region Instance Fields --------------------------------------------------------

        private readonly ShopSectionInfo mSectionInfo;

        private Texture2D mBlankTexture;
        private SpriteFont mFont;

        private Vector2 mNameTextPosition;

        #endregion

        #region Constructors -----------------------------------------------------------

        public ShopSectionComponent(ShopSectionInfo sectionInfo)
        {
            if (sectionInfo == null) throw new ArgumentNullException("sectionInfo");
            this.mSectionInfo = sectionInfo;
        }

        #endregion

        #region Events -----------------------------------------------------------------

        public event Action<ShopSectionComponent> Click;

        #endregion

        #region Instance Properties ----------------------------------------------------

        public override void LoadContent(ContentManager contentManager)
        {
            this.mBlankTexture = contentManager.Load<Texture2D>("Sprites/Blank");
            this.mFont = contentManager.Load<SpriteFont>("Fonts/DefaultSmall");

            var lNameTextSize = this.mFont.MeasureString(this.mSectionInfo.NameText);
            this.mNameTextPosition = this.mSectionInfo.NamePosition + ((this.mSectionInfo.NameSize - lNameTextSize)/2f);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
            
            spriteBatch.Draw(this.mBlankTexture, this.mSectionInfo.NamePosition, null, Color.LightGoldenrodYellow, 0, Vector2.Zero, this.mSectionInfo.NameSize, SpriteEffects.None, 0);
            spriteBatch.DrawString(this.mFont, this.mSectionInfo.NameText, this.mNameTextPosition, Color.Black);
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
                this.mSectionInfo.NamePosition,
                this.mSectionInfo.NameSize,
                x, y);
        }

        #endregion

        #region Instance Methods -------------------------------------------------------

        #endregion

    }
}
