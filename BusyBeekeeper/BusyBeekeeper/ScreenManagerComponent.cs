using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BusyBeekeeper.Core;
using BusyBeekeeper.Data;

namespace BusyBeekeeper
{
    internal sealed class ScreenManagerComponent : DrawableGameComponent, IGameScreenManager
    {
        private SpriteBatch mSpriteBatch;
        private readonly InputState mInputState = new InputState();
        private readonly BeeWorldManager mBeeWorldManager;

        public ScreenManagerComponent(Game game, IGameScreen initalGameScreen, BeeWorldManager beeWorldManager)
            : base(game)
        {
            if (initalGameScreen == null) throw new ArgumentNullException("initialGameScreen");
            if (beeWorldManager == null) throw new ArgumentNullException("beeWorldManager");
            
            this.mBeeWorldManager = beeWorldManager;

            this.Present(initalGameScreen);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            this.mSpriteBatch = new SpriteBatch(this.GraphicsDevice);
        }

        public IGameScreen CurrentScreen { get; private set; }

        public BeeWorldManager BeeWorldManager
        {
            get { return this.mBeeWorldManager; }
        }

        public Player Player
        {
            get { return this.mBeeWorldManager.PlayerManager.Player; }
        }

        public void TransitionTo(IGameScreen gameScreen)
        {
            if (gameScreen == null) throw new ArgumentNullException("gameScreen");
            if (gameScreen == this.CurrentScreen) throw new ArgumentException("Cannot transition to the current screen.");

            this.Dismiss(this.CurrentScreen);
            this.Present(gameScreen);
        }

        private void Dismiss(IGameScreen gameScreen)
        {
            gameScreen.UnloadContent();
            gameScreen.ScreenManager = null;
        }

        private void Present(IGameScreen gameScreen)
        {
            gameScreen.ScreenManager = this;
            gameScreen.LoadContent();
            this.CurrentScreen = gameScreen;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.mInputState.Update();
            this.CurrentScreen.HandleInput(this.mInputState);
            this.CurrentScreen.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.mSpriteBatch.Begin();
            this.CurrentScreen.Draw(this.mSpriteBatch, gameTime);
            this.mSpriteBatch.End();
        }
    }
}
