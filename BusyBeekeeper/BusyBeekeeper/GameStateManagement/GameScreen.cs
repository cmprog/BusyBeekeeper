#region File Description
//-----------------------------------------------------------------------------
// GameScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace BusyBeekeeper.GameStateManagement
{
    /// <summary>
    /// A screen is simply a layer of which can be updated and drawn.
    /// </summary>
    public abstract class GameScreen
    {
        /// <summary>
        /// Gets the manager that this screen belongs to.
        /// </summary>
        public ScreenManager ScreenManager { get; internal set; }

        /// <summary>
        /// Gets the ContentManager which lives for the lifetime of the screen.
        /// </summary>
        public ContentManager ContentManager { get; private set; }

        /// <summary>
        /// Gives the screen a chance to load any content it needs before entering the
        /// normal update and draw loop message loop.
        /// </summary>
        public virtual void Load()
        {
            this.ContentManager = new ContentManager(this.ScreenManager.Game.Content.ServiceProvider);
            this.ContentManager.RootDirectory = "Content";
        }

        /// <summary>
        /// Unload content for the screen. Called when the screen is removed from the screen manager.
        /// </summary>
        public virtual void Unload() 
        {
            this.ContentManager.Unload();
        }

        /// <summary>
        /// Allows the screen to run logic, such as updating the transition position.
        /// Unlike HandleInput, this method is called regardless of whether the screen
        /// is active, hidden, or in the middle of a transition.
        /// </summary>
        /// <param name="gameTime">The current GameTime.</param>
        /// <param name="isPaused">Flag indicating that the game is paused.</param>
        public virtual void Update(GameTime gameTime, bool isPaused)
        {
        }

        /// <summary>
        /// This is called when the screen should draw itself.
        /// </summary>
        public virtual void Draw(GameTime gameTime) { }
    }
}
