using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BusyBeekeeper.Screens
{
    internal sealed class BeeHiveSuperComponent : ScreenComponent
    {
        #region Instance Fields --------------------------------------------------------
        
        #endregion

        #region Constructors -----------------------------------------------------------
        
        #endregion

        #region Instance Properties ----------------------------------------------------

        public Texture2D BlankTexture { get; set; }

        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }

        public bool IsSelected { get; set; }

        #endregion

        #region Instance Methods -------------------------------------------------------

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);

            spriteBatch.Draw(this.BlankTexture, this.Position, null, Color.White, 0, Vector2.Zero, this.Size, SpriteEffects.None, 0);

            if (this.IsSelected)
            {
                var lTopLeft = this.Position;
                var lTopRight = new Vector2(lTopLeft.X + this.Size.X - 2, lTopLeft.Y);
                var lBottomLeft = new Vector2(lTopLeft.X, lTopLeft.Y + this.Size.Y - 2);

                var lWidthSize = new Vector2(this.Size.X, 2);
                var lHeightSize = new Vector2(2, this.Size.Y);

                spriteBatch.Draw(this.BlankTexture, lTopLeft, null, Color.Red, 0, Vector2.Zero, lWidthSize, SpriteEffects.None, 0);
                spriteBatch.Draw(this.BlankTexture, lTopLeft, null, Color.Red, 0, Vector2.Zero, lHeightSize, SpriteEffects.None, 0);
                spriteBatch.Draw(this.BlankTexture, lTopRight, null, Color.Red, 0, Vector2.Zero, lHeightSize, SpriteEffects.None, 0);
                spriteBatch.Draw(this.BlankTexture, lBottomLeft, null, Color.Red, 0, Vector2.Zero, lWidthSize, SpriteEffects.None, 0);
            }
        }

        #endregion
    }
}
