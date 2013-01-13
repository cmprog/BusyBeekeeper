using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BusyBeekeeper
{
    /// <summary>
    /// This is the main Game class - we delegate basically everything off to the ScreenManager.
    /// </summary>
    public class BusyBeekeeperGame : Microsoft.Xna.Framework.Game
    {
        private BeeWorldManagerComponent mBeeWorldManagerComponent;
        private ScreenManagerComponent mScreenManagerComponent;

        /// <summary>
        /// Initializes a new instance of the <see cref="BusyBeekeeperGame"/> class.
        /// </summary>
        public BusyBeekeeperGame()
        {
            this.IsMouseVisible = true;
            this.TargetElapsedTime = TimeSpan.FromTicks(333333);
            this.Content.RootDirectory = "Content";

            this.Graphics = new GraphicsDeviceManager(this);
            this.Graphics.IsFullScreen = false;
            this.Graphics.PreferredBackBufferWidth = 1136;
            this.Graphics.PreferredBackBufferHeight = 640;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            
            this.mBeeWorldManagerComponent = new BeeWorldManagerComponent(this);
            this.mBeeWorldManagerComponent.Initialize();
            this.Components.Add(this.mBeeWorldManagerComponent);

            var lInitialGameScreen = new Screens.BeeWorldScreen();
            this.mScreenManagerComponent = new ScreenManagerComponent(this, lInitialGameScreen, this.mBeeWorldManagerComponent.BeeWorldManager);
            this.mScreenManagerComponent.Initialize();
            this.Components.Add(this.mScreenManagerComponent);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);
            this.mBeeWorldManagerComponent.BeeWorldManager.PlayerManager.Save();
        }

        /// <summary>
        /// Gets or sets the GraphicsDeviceManager responsible for this game.
        /// </summary>
        private GraphicsDeviceManager Graphics { get; set; }
    }
}
