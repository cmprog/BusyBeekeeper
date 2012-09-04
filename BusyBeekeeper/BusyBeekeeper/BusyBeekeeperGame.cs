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
using TccLib.Xna.GameStateManagement;

namespace BusyBeekeeper
{
    /// <summary>
    /// This is the main Game class - we delegate basically everything off to the ScreenManager.
    /// </summary>
    public class BusyBeekeeperGame : Microsoft.Xna.Framework.Game
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BusyBeekeeperGame"/> class.
        /// </summary>
        public BusyBeekeeperGame()
        {
            this.IsMouseVisible = true;
            this.TargetElapsedTime = TimeSpan.FromTicks(333333);
            this.Graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.ScreenManager = new ScreenManager(this);
        }

        /// <summary>
        /// Gets or sets the GraphicsDeviceManager responsible for this game.
        /// </summary>
        private GraphicsDeviceManager Graphics { get; set; }

        /// <summary>
        /// Gets or sets a the ScreenManager associated with the game.
        /// </summary>
        private ScreenManager ScreenManager { get; set; }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.ScreenManager.AddScreen(new Screens.TitleScreen());
            base.Initialize();
        }
    }
}
