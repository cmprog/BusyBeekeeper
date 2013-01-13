using System;
using System.Collections.Generic;
using BusyBeekeeper.Screens.CommonComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BusyBeekeeper.Screens
{
    internal abstract class GameScreenBase : IGameScreen
    {
        private ContentManager mContentManager;
        private Vector2 mScreenSize;

        public IGameScreenManager ScreenManager { get; set; }

        public ContentManager ContentManager
        {
            get { return this.mContentManager; }
        }

        public Vector2 ScreenSize
        {
            get { return this.mScreenSize; }
        }

        public virtual void LoadContent() 
        {
            this.mContentManager = new ContentManager(this.ScreenManager.Game.Services);
            this.mContentManager.RootDirectory = "Content";

            this.mScreenSize = new Vector2(
                this.ScreenManager.Game.GraphicsDevice.Viewport.Width,
                this.ScreenManager.Game.GraphicsDevice.Viewport.Height);
        }

        public virtual void UnloadContent()
        {
            this.mContentManager.Unload();
        }

        public virtual void Update(GameTime gameTime) { }
        public virtual void HandleInput(InputState inputState) { }
        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime) { }
    }
}
