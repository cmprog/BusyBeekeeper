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
        private readonly BeeWorldManager mWorldManager;
        private readonly Vector2 mScreenSize;
        private readonly Vector2 mPosition;
        private readonly Vector2 mSize;

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
        }

        protected Texture2D BlankTexture { get { return this.mBlankTexture; } }
        protected Vector2 ScreenSize { get { return this.mScreenSize; } }
        public Vector2 Size { get { return this.mSize; } }
        public Vector2 Position { get { return this.mPosition; } }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);

            this.mBlankTexture = contentManager.Load<Texture2D>("Sprites/Blank");
            this.mFont = contentManager.Load<SpriteFont>("Fonts/DefaultSmall");
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
            
            spriteBatch.Draw(this.BlankTexture, this.mPosition, null, Color.DarkGreen, 0, Vector2.Zero, this.mSize, SpriteEffects.None, 0);

            var lBeeTime = this.mWorldManager.Time;
            var lBeeTimeText = string.Concat(lBeeTime.Day, ":", lBeeTime.Hour, ":", lBeeTime.Minute);
            var lBeeTimePosition = this.Position + new Vector2(5, 5);

            spriteBatch.DrawString(this.mFont, lBeeTimeText, lBeeTimePosition, Color.Black);
        }
    }
}
