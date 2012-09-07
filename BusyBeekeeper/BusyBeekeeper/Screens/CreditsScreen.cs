using System;
using System.Linq;
using BusyBeekeeper.Components;
using BusyBeekeeper.Behaviors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TccLib.Xna.Framework;
using TccLib.Xna.Framework.Renderers;
using BusyBeekeeper.GameStateManagement;

namespace BusyBeekeeper.Screens
{
    /// <summary>
    /// Displays the credits of the game.
    /// </summary>
    public class CreditsScreen : GameScreen
    {
        /// <summary>
        /// Initializes a new instance of the CreditsScreen class.
        /// </summary>
        public CreditsScreen()
        {
            this.BackButton = new TextButtonComponent();
        }

        /// <summary>
        /// Gets or sets the back button.
        /// </summary>
        private TextButtonComponent BackButton { get; set; }

        /// <summary>
        /// Activates the screen. Called when the screen is added to the screen manager or if the game resumes
        /// from being paused or tombstoned.
        /// </summary>
        public override void Load()
        {
            base.Load();

            this.BackButton.TextProperty.Value = "Back";
            this.BackButton.PositionProperty.Value = new Vector2(50, 350);
            this.BackButton.SizeProperty.Value = new Vector2(200, 75);
            this.BackButton.FontProperty.Value = this.ContentManager.Load<SpriteFont>("Fonts/BasicFont");
            this.BackButton.Renderer = new CompositeRenderer(
                new AdvancedRenderer(
                    SharedProperty.Create(
                        this.ContentManager.Load<Texture2D>("Sprites/Blank")),
                    this.BackButton.PositionProperty,
                    SharedProperty.Create(Color.Gold),
                    this.BackButton.SizeProperty),
                new CenteredTextRenderer(
                    this.BackButton.TextProperty,
                    this.BackButton.FontProperty,
                    this.BackButton.PositionProperty,
                    this.BackButton.SizeProperty));
            this.BackButton.Behaviors.Add(
                new OnFocusFontChangeBehavior(
                    this.ScreenManager.InputState,
                    this.ContentManager.Load<SpriteFont>("Fonts/BasicFont"),
                    this.ContentManager.Load<SpriteFont>("Fonts/BasicFontBold"),
                    this.BackButton.PositionProperty,
                    this.BackButton.SizeProperty,
                    this.BackButton.FontProperty));
            this.BackButton.Behaviors.Add(
                new OnMouseUpBehavior(
                    this.ScreenManager.InputState,
                    this.OnBackButtonClick,
                    this.BackButton.PositionProperty,
                    this.BackButton.SizeProperty));
        }

        /// <summary>
        /// When the user clicks the back button, we'll exit this screen
        /// which should return us to the previous screen.
        /// </summary>
        private void OnBackButtonClick()
        {
            this.ScreenManager.TransitionToScreen(new TitleScreen());
        }

        /// <summary>
        /// Updates the screen. We simply need to iterate through all the behaviors and
        /// call update on them respectively.
        /// </summary>
        /// <param name="gameTime">The current GameTime.</param>
        /// <param name="isPaused">Indicates that the game is paused, or at least that the GameScreenManager wants us paused.</param>
        public override void Update(GameTime gameTime, bool isPaused)
        {
            base.Update(gameTime, isPaused);

            var behaviors = this.BackButton.Behaviors;

            foreach (var behavior in behaviors)
            {
                behavior.Update(gameTime);
            }
        }

        /// <summary>
        /// Draws the screen by calling the renders for all our components.
        /// </summary>
        /// <param name="gameTime">The current GameTime.</param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.ScreenManager.SpriteBatch.Begin();
            this.BackButton.Renderer.Render(this.ScreenManager.SpriteBatch, gameTime);
            this.ScreenManager.SpriteBatch.End();
        }
    }
}
