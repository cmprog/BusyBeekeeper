using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BusyBeekeeper.GameStateManagement
{
    /// <summary>
    /// This is a special GameScreen only used directly by the ScreenManager for transitioning
    /// from one screen to another screen. It performs an animating transition to case the current
    /// screens to be faded, then 
    /// </summary>
    internal sealed class TransitionScreen : GameScreen
    {
        private static readonly TimeSpan TransitionOffTime = TimeSpan.FromSeconds(0.5);
        private static readonly TimeSpan TransitionOnTime = TimeSpan.FromSeconds(0.5);

        /// <summary>
        /// Creates a new instance of the TransitionScreen class.
        /// </summary>
        /// <param name="nextScreen">The next screen to display.</param>
        /// <param name="transitionColor">The Color to use during the transition.</param>
        /// <param name="transitionComplete">A callback action to indicate the transition is complete.</param>
        public TransitionScreen(GameScreen nextScreen, Color transitionColor, Action transitionComplete)
        {
            this.TransitionColor = new Color(transitionColor.R, transitionColor.R, transitionColor.B, 0f);
            this.NextScreen = nextScreen;
            this.TransitionComplete = transitionComplete;
            this.TransitionAmount = -1;
        }

        /// <summary>
        /// Gets or sets the Color used for the transition color.
        /// </summary>
        private Color TransitionColor { get; set; }

        /// <summary>
        /// Gets or sets the GameScreen which will be displayed after the transition
        /// screen has completed its transition.
        /// </summary>
        private GameScreen NextScreen { get; set; }

        /// <summary>
        /// Gets or sets an action to call when the transition has completed transitioning.
        /// </summary>
        private Action TransitionComplete { get; set; }

        /// <summary>
        /// Gets or sets the texture which will be used for the transition. This should
        /// be a blank white texture.
        /// </summary>
        private Texture2D BlankTexture { get; set; }

        /// <summary>
        /// Gets the amount of the transition ranging from -1 to 1. -1 represents the start of the transition,
        /// 0 represents the moment where we will swap screen, and 1 represents the end of the transition.
        /// </summary>
        private float TransitionAmount { get; set; }

        /// <summary>
        /// Loads the required textures.
        /// </summary>
        public override void Load()
        {
            base.Load();
            this.BlankTexture = this.ContentManager.Load<Texture2D>("Sprites/Blank");
        }

        /// <summary>
        /// Updates the state of the transition.
        /// </summary>
        /// <param name="gameTime">The current GameTime.</param>
        /// <param name="isPaused">Whether or not we're paused.</param>
        public override void Update(GameTime gameTime, bool isPaused)
        {
            base.Update(gameTime, isPaused);

            // When the game is paused, we'll do nothing.
            if (isPaused) return;

            if (this.TransitionAmount < 0)
            {
                // Increment the transition amount.
                var increment = (float)(gameTime.ElapsedGameTime.TotalSeconds / TransitionOffTime.TotalSeconds);
                this.TransitionAmount += increment;

                // If we've gone from negative to positive, swap the screens via the screen manager.
                if (this.TransitionAmount >= 0)
                {
                    this.ScreenManager.SwapScreen(this.NextScreen);
                }
            }
            else
            {
                // Increment the transition amount.
                var increment = (float)(gameTime.ElapsedGameTime.TotalSeconds / TransitionOffTime.TotalSeconds);
                this.TransitionAmount += increment;

                if (this.TransitionAmount >= 1)
                {
                    this.TransitionComplete();
                }
            }

            this.TransitionColor = new Color(
                this.TransitionColor.R,
                this.TransitionColor.G,
                this.TransitionColor.B,
                1f - (float)Math.Abs(this.TransitionAmount));
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            var screenSize = new Vector2(
                this.ScreenManager.GraphicsDevice.Viewport.Width,
                this.ScreenManager.GraphicsDevice.Viewport.Height);

            this.ScreenManager.SpriteBatch.Draw(
                this.BlankTexture,
                Vector2.Zero,
                null,
                this.TransitionColor,
                0,
                Vector2.Zero,
                screenSize,
                SpriteEffects.None,
                0);
        }
    }
}
