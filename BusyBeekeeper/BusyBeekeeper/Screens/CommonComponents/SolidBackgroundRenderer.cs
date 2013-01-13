using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BusyBeekeeper.Screens.CommonComponents
{
    internal sealed class SolidBackgroundRenderer : IBackgroundRenderer
    {
        private readonly Texture2D mBlankTexture;
        private readonly Color mTintColor;

        public SolidBackgroundRenderer(Texture2D blankTexture, Color tintColor)
        {
            if (blankTexture == null) throw new ArgumentNullException("blankTexture");
            this.mBlankTexture = blankTexture;
            this.mTintColor = tintColor;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Vector2 position, Vector2 size)
        {
            spriteBatch.Draw(this.mBlankTexture, position, null, this.mTintColor, 0f, Vector2.Zero, size, SpriteEffects.None, 0);
        }
    }
}
