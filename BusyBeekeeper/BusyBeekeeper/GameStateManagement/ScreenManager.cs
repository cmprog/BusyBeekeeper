using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Linq;
using TccLib.Extensions;

namespace BusyBeekeeper.GameStateManagement
{
    /// <summary>
    /// The screen manage is responsible for managing the screens within the game and the transactions
    /// from one screen to another. A screen is allowed to have a single PopupScreen layed over it. When
    /// a PopupScreen is displayed over a normal GameScreen, the GameScreen will still get update and draw
    /// requested, though it will be told that the game is in a paused state. The Popup screen will
    /// not be told the game is paused.
    /// </summary>
    public class ScreenManager : DrawableGameComponent
    {
        /// <summary>
        /// Initializes a new instance of the ScreenManager class.
        /// </summary>
        /// <param name="game">The Game which owns the ScreenManager.</param>
        public ScreenManager(Game game, GameScreen initialScreen)
            : base(game)
        {
            if (initialScreen == null)
            {
                throw new ArgumentNullException("initialScreen");
            }

            if (game == null)
            {
                throw new ArgumentNullException("game");
            }

            this.InputState = new InputState();
            this.CurrentScreen = initialScreen;
            this.CurrentScreen.ScreenManager = this;
        }

        /// <summary>
        /// Gets or sets the currently displayed GameScreen.
        /// </summary>
        private GameScreen CurrentScreen { get; set; }

        /// <summary>
        /// Gets or sets the currently active PopupScreen when one exists,
        /// otherwise null.
        /// </summary>
        private PopupScreen CurrentPopupScreen { get; set; }

        /// <summary>
        /// Gets or sets the current TransitionScreen. This screen is only used when
        /// we are transitioning from one screen to another.
        /// </summary>
        private TransitionScreen TransitionScreen { get; set; }

        /// <summary>
        /// Gets a default SpriteBatch which can be shared among all the screens.
        /// </summary>
        public SpriteBatch SpriteBatch { get; private set; }

        /// <summary>
        /// Gets the InputState used during gameplay.
        /// </summary>
        public InputState InputState { get; private set; }

        /// <summary>
        /// Gets or sets a flag indicating whether or not the screen manager
        /// has been initialized. Once initialized, we must ensure to activate
        /// and deactivate screens when they are added and removed.
        /// </summary>
        private bool IsInitialized { get; set; }

        /// <summary>
        /// Initializes the screen manager component.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this.IsInitialized = true;
        }


        /// <summary>
        /// Loads the game content. All we need to do is create the shared SpriteBatch and
        /// activate the current screen if there is one.
        /// </summary>
        protected override void LoadContent()
        {
            this.SpriteBatch = new SpriteBatch(this.GraphicsDevice);

            if (this.CurrentScreen != null)
            {
                this.CurrentScreen.Load();
            }

            if (this.CurrentPopupScreen != null)
            {
                this.CurrentPopupScreen.Load();
            }

            if (this.TransitionScreen != null)
            {
                this.TransitionScreen.Load();
            }
        }


        /// <summary>
        /// We don't have anything directly to unload, but here we tell the screen's
        /// we're managing to unload anything if they need to.
        /// </summary>
        protected override void UnloadContent()
        {
            if (this.CurrentScreen != null)
            {
                this.CurrentScreen.Unload();
            }

            if (this.CurrentPopupScreen != null)
            {
                this.CurrentPopupScreen.Unload();
            }

            if (this.TransitionScreen != null)
            {
                this.TransitionScreen.Unload();
            }
        }


        /// <summary>
        /// Updating is pretty easy, just update the screens if needed.
        /// </summary>
        /// <param name="gameTime">The current GameTime.</param>
        public override void Update(GameTime gameTime)
        {
            this.InputState.Update();

            if (this.TransitionScreen != null)
            {
                // Tansaction screen will only pause when the game itself is inactive.
                this.TransitionScreen.Update(gameTime, !Game.IsActive);
                // When we have a transition screen, nothing else will get an update.
            }
            else
            {
                if (this.CurrentPopupScreen != null)
                {
                    // Popup screen will only pause when the game itself is inactive.
                    this.CurrentPopupScreen.Update(gameTime, !Game.IsActive);
                }

                // The current screen will still update, but paused whenever there is an
                // active popup screen or if the game itself is not active.
                this.CurrentScreen.Update(
                    gameTime,
                    !Game.IsActive || (this.CurrentScreen != null));
            }
        }


        /// <summary>
        /// Has the current screen and pop up screen draw themselves (in that order).
        /// </summary>
        /// <param name="gameTime">The current GameTime.</param>
        public override void Draw(GameTime gameTime)
        {
            System.Diagnostics.Debug.Assert(
                this.CurrentScreen != null,
                "There should always be a GameScreen to draw.");

            this.CurrentScreen.Draw(gameTime);

            if (this.CurrentPopupScreen != null)
            {
                this.CurrentPopupScreen.Draw(gameTime);
            }

            if (this.TransitionScreen != null)
            {
                this.TransitionScreen.Draw(gameTime);
            }
        }

        /// <summary>
        /// Presents the given PopupScreen.
        /// </summary>
        /// <param name="popupScreen"></param>
        public void DisplayPopup(PopupScreen popupScreen)
        {
            System.Diagnostics.Debug.Assert(
                popupScreen != null,
                "PopupScreen cannot be null, use DismissPopup to remove the current PopupScreen.");

            this.CurrentPopupScreen = popupScreen;
            this.CurrentPopupScreen.ScreenManager = this;
            this.CurrentPopupScreen.Load();
        }

        /// <summary>
        /// Dismisses the current PopupScreen.
        /// </summary>
        public void DismissPopup()
        {
            System.Diagnostics.Debug.Assert(
                this.CurrentPopupScreen != null,
                "Cannot dismiss a PopupScreen that doesn't exist!");

            this.CurrentPopupScreen = null;
        }

        /// <summary>
        /// Immediately swaps the current game screen with the given game screen.
        /// </summary>
        /// <param name="gameScreen">The GameScreen to replace with the current GameScreen.</param>
        public void SwapScreen(GameScreen gameScreen)
        {
            System.Diagnostics.Debug.Assert(
                gameScreen != null,
                "Cannot swap to a null GameScreen.");

            this.CurrentScreen = gameScreen;
            this.CurrentPopupScreen = null;
            this.CurrentScreen.ScreenManager = this;
            this.CurrentScreen.Load();
        }

        /// <summary>
        /// Transitions from the current GameScreen to the given GameScreen. This is done
        /// with style and animation, use for more dramatic screen changes.
        /// </summary>
        /// <param name="gameScreen">The GameScreen to replace with the current GameScreen.</param>
        public void TransitionToScreen(GameScreen gameScreen)
        {
            this.TransitionToScreen(gameScreen, Color.Black);
        }

        /// <summary>
        /// Transitions from the current GameScreen to the given GameScreen. This is done
        /// with style and animation, use for more dramatic screen changes.
        /// </summary>
        /// <param name="gameScreen">The GameScreen to replace with the current GameScreen.</param>
        /// <param name="transitionColor">The color of the transition.</param>
        public void TransitionToScreen(GameScreen gameScreen, Color transitionColor)
        {
            this.TransitionScreen = new TransitionScreen(
                gameScreen.ThrowIfNull("gameScreen"),
                transitionColor,
                () => this.TransitionScreen = null);
            this.TransitionScreen.ScreenManager = this;
            this.TransitionScreen.Load();
        }
    }
}
