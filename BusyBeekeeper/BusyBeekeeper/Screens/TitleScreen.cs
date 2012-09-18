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
    /// The title screen is mostly just a splash screen with a play button.
    /// </summary>
    internal class TitleScreen : GameScreen
    {
        /// <summary>
        /// Initializes a new instance of the TitleScreen class.
        /// </summary>
        public TitleScreen()
        {
            this.StartButton = new TextBoxComponent();
            this.CreditsButton = new TextBoxComponent();
            this.ExitButton = new TextBoxComponent();
        }

        /// <summary>
        /// Gets or sets the start button.
        /// </summary>
        private TextBoxComponent StartButton { get; set; }

        /// <summary>
        /// Gets or sets the credits button.
        /// </summary>
        private TextBoxComponent CreditsButton { get; set; }

        /// <summary>
        /// Gets or sets the exit button.
        /// </summary>
        private TextBoxComponent ExitButton { get; set; }

        /// <summary>
        /// Activates the screen. Called when the screen is added to the screen manager or if the game resumes
        /// from being paused or tombstoned.
        /// </summary>
        public override void Load()
        {
            base.Load();

            this.StartButton.TextProperty.Value = "Start";
            this.StartButton.PositionProperty.Value = new Vector2(50, 100);
            this.StartButton.SizeProperty.Value = new Vector2(200, 75);
            this.StartButton.FontProperty.Value = this.ContentManager.Load<SpriteFont>("Fonts/BasicFont");
            this.StartButton.Renderer = new CompositeRenderer(
                new AdvancedRenderer(                    
                    SharedProperty.Create(
                        this.ContentManager.Load<Texture2D>("Sprites/Blank")),
                    this.StartButton.PositionProperty,
                    SharedProperty.Create(Color.Gold),
                    this.StartButton.SizeProperty),
                new CenteredTextRenderer(
                    this.StartButton.TextProperty,
                    this.StartButton.FontProperty,
                    this.StartButton.PositionProperty,
                    this.StartButton.SizeProperty));
            this.StartButton.Behaviors.Add(
                new OnFocusFontChangeBehavior(
                    this.ScreenManager.InputState,
                    this.ContentManager.Load<SpriteFont>("Fonts/BasicFont"),
                    this.ContentManager.Load<SpriteFont>("Fonts/BasicFontBold"),
                    this.StartButton.PositionProperty,
                    this.StartButton.SizeProperty,
                    this.StartButton.FontProperty));
            this.StartButton.Behaviors.Add(
                new OnMouseUpBehavior(
                    this.ScreenManager.InputState,
                    this.OnStartButtonClick,
                    this.StartButton.PositionProperty,
                    this.StartButton.SizeProperty));

            this.CreditsButton.TextProperty.Value = "Credits";
            this.CreditsButton.PositionProperty.Value = new Vector2(50, 200);
            this.CreditsButton.SizeProperty.Value = new Vector2(200, 75);
            this.CreditsButton.FontProperty.Value = this.ContentManager.Load<SpriteFont>("Fonts/BasicFont");
            this.CreditsButton.Renderer = new CompositeRenderer(
                new AdvancedRenderer(
                    SharedProperty.Create(this.ContentManager.Load<Texture2D>("Sprites/Blank")),
                    this.CreditsButton.PositionProperty,
                    SharedProperty.Create(Color.Gold),
                    this.CreditsButton.SizeProperty),
                new CenteredTextRenderer(
                    this.CreditsButton.TextProperty,
                    this.CreditsButton.FontProperty,
                    this.CreditsButton.PositionProperty,
                    this.CreditsButton.SizeProperty));
            this.CreditsButton.Behaviors.Add(
                new OnFocusFontChangeBehavior(
                    this.ScreenManager.InputState,
                    this.ContentManager.Load<SpriteFont>("Fonts/BasicFont"),
                    this.ContentManager.Load<SpriteFont>("Fonts/BasicFontBold"),
                    this.CreditsButton.PositionProperty,
                    this.CreditsButton.SizeProperty,
                    this.CreditsButton.FontProperty));
            this.CreditsButton.Behaviors.Add(
                new OnMouseUpBehavior(
                    this.ScreenManager.InputState,
                    this.OnCreditsButtonClick,
                    this.CreditsButton.PositionProperty,
                    this.CreditsButton.SizeProperty));

            this.ExitButton.TextProperty.Value = "Exit";
            this.ExitButton.PositionProperty.Value = new Vector2(50, 300);
            this.ExitButton.SizeProperty.Value = new Vector2(200, 75);
            this.ExitButton.FontProperty.Value = this.ContentManager.Load<SpriteFont>("Fonts/BasicFont");
            this.ExitButton.Renderer = new CompositeRenderer(
                new AdvancedRenderer(
                    SharedProperty.Create(this.ContentManager.Load<Texture2D>("Sprites/Blank")),
                    this.ExitButton.PositionProperty,
                    SharedProperty.Create(Color.Gold),
                    this.ExitButton.SizeProperty),
                new CenteredTextRenderer(
                    this.ExitButton.TextProperty,
                    this.ExitButton.FontProperty,
                    this.ExitButton.PositionProperty,
                    this.ExitButton.SizeProperty));
            this.CreditsButton.Behaviors.Add(
                new OnFocusFontChangeBehavior(
                    this.ScreenManager.InputState,
                    this.ContentManager.Load<SpriteFont>("Fonts/BasicFont"),
                    this.ContentManager.Load<SpriteFont>("Fonts/BasicFontBold"),
                    this.ExitButton.PositionProperty,
                    this.ExitButton.SizeProperty,
                    this.ExitButton.FontProperty));
            this.CreditsButton.Behaviors.Add(
                new OnMouseUpBehavior(
                    this.ScreenManager.InputState,
                    this.OnExitButtonClick,
                    this.ExitButton.PositionProperty,
                    this.ExitButton.SizeProperty));
        }

        public override void Update(GameTime gameTime, bool isPaused)
        {
            base.Update(gameTime, isPaused);

            var behaviors =
                this.StartButton.Behaviors
                    .Concat(this.CreditsButton.Behaviors)
                    .Concat(this.ExitButton.Behaviors);

            foreach (var behavior in behaviors)
            {
                behavior.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            this.StartButton.Renderer.Render(this.ScreenManager.SpriteBatch, gameTime);
            this.CreditsButton.Renderer.Render(this.ScreenManager.SpriteBatch, gameTime);
            this.ExitButton.Renderer.Render(this.ScreenManager.SpriteBatch, gameTime);
        }

        /// <summary>
        /// When the start button is clicked, we'll transition to the game selection screen.
        /// </summary>
        private void OnStartButtonClick()
        {
            this.ScreenManager.TransitionToScreen(new GameSelectionScreen());
        }

        /// <summary>
        /// When the credits button is clicked, we go to the credits screen.
        /// </summary>
        private void OnCreditsButtonClick()
        {
            this.ScreenManager.TransitionToScreen(new CreditsScreen());
        }

        /// <summary>
        /// When the exit button is clicked, we must exit the game.
        /// </summary>
        private void OnExitButtonClick()
        {
            this.ScreenManager.Game.Exit();
        }
    }
}
