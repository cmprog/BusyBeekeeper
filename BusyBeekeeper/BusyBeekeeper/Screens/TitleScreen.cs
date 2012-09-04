using System;
using System.Linq;
using BusyBeekeeper.Components;
using BusyBeekeeper.Behaviors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TccLib.Xna.Framework;
using TccLib.Xna.Framework.Renderers;
using TccLib.Xna.GameStateManagement;

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
            this.StartButton = new TextButtonComponent();
            this.CreditsButton = new TextButtonComponent();
            this.ExitButton = new TextButtonComponent();
        }

        /// <summary>
        /// Gets or sets the start button.
        /// </summary>
        private TextButtonComponent StartButton { get; set; }

        /// <summary>
        /// Gets or sets the credits button.
        /// </summary>
        private TextButtonComponent CreditsButton { get; set; }

        /// <summary>
        /// Gets or sets the exit button.
        /// </summary>
        private TextButtonComponent ExitButton { get; set; }

        /// <summary>
        /// Activates the screen. Called when the screen is added to the screen manager or if the game resumes
        /// from being paused or tombstoned.
        /// </summary>
        public override void Activate()
        {
            base.Activate();

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
                    SharedProperty.Create(
                        this.ContentManager.Load<Texture2D>("Sprites/Blank")),
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
                    SharedProperty.Create(
                        this.ContentManager.Load<Texture2D>("Sprites/Blank")),
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

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

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
            this.ScreenManager.SpriteBatch.Begin();
            this.StartButton.Renderer.Render(this.ScreenManager.SpriteBatch, gameTime);
            this.CreditsButton.Renderer.Render(this.ScreenManager.SpriteBatch, gameTime);
            this.ExitButton.Renderer.Render(this.ScreenManager.SpriteBatch, gameTime);
            this.ScreenManager.SpriteBatch.End();
        }

        private void OnStartButtonClick()
        {
            Console.WriteLine("OnStartButtonClick");
        }

        private void OnCreditsButtonClick()
        {
            Console.WriteLine("OnCreditsButtonClick");
        }

        private void OnExitButtonClick()
        {
            Console.WriteLine("OnExitButtonClick");
        }
    }
}
