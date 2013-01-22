using System;
using BusyBeekeeper.Data;
using BusyBeekeeper.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BusyBeekeeper.Screens
{
    internal class HudComponent : ScreenComponent
    {
        protected const int sItemMargin = 10;

        private readonly BeeWorldManager mWorldManager;
        private readonly Vector2 mScreenSize;
        private readonly Vector2 mPosition;
        private readonly Vector2 mSize;

        private readonly float mDayCountWidth = 50;
        private readonly Vector2 mDayCountPosition;
        private readonly Vector2 mDayProgressPosition;
        private readonly Vector2 mDayProgressSize;
        private Vector2 mDayProgressIndicatorSize;

        private Texture2D mBlankTexture;
        private SpriteFont mFont;

        public HudComponent(BeeWorldManager worldManager, Vector2 screenSize)
        {
            this.mWorldManager = worldManager;
            this.mScreenSize = screenSize;
            
            const float lcHudHeight = 100f;
            const float lcHudWidth = 800f;

            this.mSize = new Vector2(lcHudWidth, lcHudHeight);
            this.mPosition = screenSize - this.mSize;

            const int lcProgressHeight = 10;
            this.mDayCountPosition = this.mPosition + new Vector2(sItemMargin);
            this.mDayProgressPosition = new Vector2(
                this.mDayCountPosition.X + this.mDayCountWidth + sItemMargin,
                this.mDayCountPosition.Y);
            this.mDayProgressSize = new Vector2(
                this.mSize.X - (3 * sItemMargin) - this.mDayCountWidth,
                lcProgressHeight);
            this.mDayProgressIndicatorSize = new Vector2(0, this.mDayProgressSize.Y);
        }

        protected Texture2D BlankTexture { get { return this.mBlankTexture; } }
        protected Vector2 ScreenSize { get { return this.mScreenSize; } }

        protected SpriteFont Font
        {
            get { return this.mFont; }
        }

        public Vector2 Size { get { return this.mSize; } }
        public Vector2 Position { get { return this.mPosition; } }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var lCurrentTime = this.mWorldManager.Time;
            var lCurrentMinutesIntoDay = lCurrentTime.Minute + (BeeTime.MinutesInHour * lCurrentTime.Hour);
            this.mDayProgressIndicatorSize.X = MathHelper.Lerp(
                0, this.mDayProgressSize.X, (float)lCurrentMinutesIntoDay / BeeTime.MinutesInDay);
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);

            this.mBlankTexture = contentManager.Load<Texture2D>("Sprites/Blank");
            this.mFont = contentManager.Load<SpriteFont>("Fonts/DefaultTiny");
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
            
            spriteBatch.Draw(this.BlankTexture, this.mPosition, null, Color.DarkGreen, 0, Vector2.Zero, this.mSize, SpriteEffects.None, 0);
            spriteBatch.Draw(this.mBlankTexture, this.mDayProgressPosition, null, Color.White, 0, Vector2.Zero, this.mDayProgressSize, SpriteEffects.None, 0);
            spriteBatch.Draw(this.mBlankTexture, this.mDayProgressPosition, null, Color.Blue, 0, Vector2.Zero, this.mDayProgressIndicatorSize, SpriteEffects.None, 0);

            var lDayText = this.mWorldManager.Time.Day.ToString();

            spriteBatch.DrawString(this.mFont, lDayText, this.mDayCountPosition, Color.Black);
        }
    }
}
