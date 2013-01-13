using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BusyBeekeeper.Screens.CommonComponents
{
    internal class Label : RectangleScreenComponent
    {
        private IBackgroundRenderer mBackgroundRenderer;

        public Label()
        {
            this.mBackgroundRenderer = new EmptyBackgroundRenderer();
        }

        public IBackgroundRenderer BackgroundRenderer
        {
            get { return this.mBackgroundRenderer; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("BackgroundRenderer");
                }
                this.mBackgroundRenderer = value;
            }
        }

        public SpriteFont Font
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public Color TextColor
        {
            get;
            set;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
            this.BackgroundRenderer.Draw(spriteBatch, gameTime, this.Position, this.Size);

            var lTextSize = this.Font.MeasureString(this.Text);
            var lTextPosition = this.Position + ((this.Size - lTextSize) / 2);

            spriteBatch.DrawString(this.Font, this.Text, lTextPosition, this.TextColor);
        }
    }
}
