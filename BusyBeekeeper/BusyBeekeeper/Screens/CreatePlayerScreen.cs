using System;
using System.Linq;
using BusyBeekeeper.Components;
using BusyBeekeeper.Behaviors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TccLib.Xna.Framework;
using TccLib.Xna.Framework.Renderers;
using BusyBeekeeper.Extensions;
using BusyBeekeeper.GameStateManagement;

namespace BusyBeekeeper.Screens
{
    /// <summary>
    /// The create player screen allows the player to, well, create their player.
    /// </summary>
    public class CreatePlayerScreen : GameScreen
    {
        /// <summary>
        /// Creates a new instance of the CreatePlayerScreen class.
        /// </summary>
        /// <param name="playerManager"></param>
        public CreatePlayerScreen(PlayerManager playerManager)
        {
            this.PlayerManager = playerManager;

            this.BackButton = new TextButtonComponent();
        }

        /// <summary>
        /// Gets or sets the PlayerManager used to manage the player.
        /// </summary>
        private PlayerManager PlayerManager { get; set; }

        /// <summary>
        /// Gets or sets the button to go back to the GameSelectionScreen.
        /// </summary>
        private TextButtonComponent BackButton { get; set; }

        /// <summary>
        /// Loads the screen content.
        /// </summary>
        public override void Load()
        {
            base.Load();

            this.BackButton.TextProperty.Value = "Back";
            this.BackButton.PositionProperty.Value = new Vector2(10, 420);
            this.BackButton.SizeProperty.Value = new Vector2(100, 50);
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
        /// When the user clicks the back button, we'll transition back to the
        /// GameSelectionScreen.
        /// </summary>
        private void OnBackButtonClick()
        {
            this.ScreenManager.TransitionToScreen(new GameSelectionScreen());
        }

        /// <summary>
        /// Updates the screen, this is as simple as updating the components.
        /// </summary>
        /// <param name="gameTime">The current GameTime.</param>
        /// <param name="isPaused">Flag indicating whether the game is paused or not.</param>
        public override void Update(GameTime gameTime, bool isPaused)
        {
            base.Update(gameTime, isPaused);

            if (isPaused) return;
            this.BackButton.Update(gameTime);
        }

        /// <summary>
        /// Draws all the components on the screen by calling their renderers.
        /// </summary>
        /// <param name="gameTime">The current GameTime.</param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            this.BackButton.Renderer.Render(this.ScreenManager.SpriteBatch, gameTime);
        }
    }
}
