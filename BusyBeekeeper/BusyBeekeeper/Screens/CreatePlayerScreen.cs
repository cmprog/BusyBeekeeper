using System;
using System.Linq;
using BusyBeekeeper.Components;
using BusyBeekeeper.Behaviors;
using BusyBeekeeper.MessageHandlers;
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
        public CreatePlayerScreen(PlayerManager playerManager, int playerSlotKey)
        {
            this.PlayerManager = playerManager;

            this.PlayerSlotKey = playerSlotKey;

            this.BackButton = new TextBoxComponent();
            this.StartButton = new TextBoxComponent();
            this.BeekeeperNameTextBox = new TextBoxComponent();

            this.MaleAvatarPictureBox = new AvatarSelectionComponent();
            this.FemaleAvatarPictureBox = new AvatarSelectionComponent();
            this.BearAvatarPictureBox = new AvatarSelectionComponent();
        }

        /// <summary>
        /// Gets or sets the player slot the creates player should be created
        /// under. The player won't be created until the user chooses both
        /// an avatar and a name.
        /// </summary>
        private int PlayerSlotKey { get; set; }

        /// <summary>
        /// Gets or sets the PlayerManager used to manage the player.
        /// </summary>
        private PlayerManager PlayerManager { get; set; }

        /// <summary>
        /// Gets or sets the button to go back to the GameSelectionScreen.
        /// </summary>
        private TextBoxComponent BackButton { get; set; }

        /// <summary>
        /// Gets or sets the button to start the game.
        /// </summary>
        private TextBoxComponent StartButton { get; set; }

        /// <summary>
        /// Gets or sets the text box which allows the user to type in a name for their beekeeper.
        /// </summary>
        private TextBoxComponent BeekeeperNameTextBox { get; set; }

        /// <summary>
        /// Gets or sets the avatar selection which displays the male beekeeper avatar.
        /// </summary>
        private AvatarSelectionComponent MaleAvatarPictureBox { get; set; }

        /// <summary>
        /// Gets or sets the avatar selection which displays the female beekeeper avatar.
        /// </summary>
        private AvatarSelectionComponent FemaleAvatarPictureBox { get; set; }

        /// <summary>
        /// Gets or sets the avatar selection which displays the exclusive bear avatar.
        /// </summary>
        private AvatarSelectionComponent BearAvatarPictureBox { get; set; }

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

            this.StartButton.TextProperty.Value = "Start";
            this.StartButton.PositionProperty.Value = new Vector2(690, 420);
            this.StartButton.SizeProperty.Value = new Vector2(100, 50);
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
            this.BackButton.Behaviors.Add(
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

            this.BeekeeperNameTextBox.PositionProperty.Value = new Vector2(100, 300);
            this.BeekeeperNameTextBox.SizeProperty.Value = new Vector2(600, 50);
            this.BeekeeperNameTextBox.FontProperty.Value = this.ContentManager.Load<SpriteFont>("Fonts/BasicFont");
            this.BeekeeperNameTextBox.Renderer = new CompositeRenderer(
                new AdvancedRenderer(
                    SharedProperty.Create(
                        this.ContentManager.Load<Texture2D>("Sprites/Blank")),
                    this.BeekeeperNameTextBox.PositionProperty,
                    SharedProperty.Create(Color.White),
                    this.BeekeeperNameTextBox.SizeProperty),
                new MiddleLeftTextRenderer(
                    this.BeekeeperNameTextBox.TextProperty,
                    this.BeekeeperNameTextBox.FontProperty,
                    this.BeekeeperNameTextBox.PositionProperty,
                    this.BeekeeperNameTextBox.SizeProperty,
                    SharedProperty.Create(Color.Black),
                    SharedProperty.Create(5f)));
            this.BackButton.Behaviors.Add(
                new InputTextBoxBehavior(
                    this.ScreenManager.InputState,
                    this.BeekeeperNameTextBox.TextProperty));

            this.MaleAvatarPictureBox.PositionProperty.Value = new Vector2(50, 50);
            this.MaleAvatarPictureBox.SizeProperty.Value = new Vector2(200, 200);
            this.MaleAvatarPictureBox.IsSelectedProperty.Value = true;
            this.MaleAvatarPictureBox.BorderColorProperty.Value = Color.Gold;
            this.MaleAvatarPictureBox.TextureProperty.Value = this.ContentManager.Load<Texture2D>("Sprites/Blank");
            this.MaleAvatarPictureBox.Renderer = new CompositeRenderer(
                new AdvancedRenderer(
                    this.MaleAvatarPictureBox.TextureProperty,
                    this.MaleAvatarPictureBox.PositionProperty,
                    SharedProperty.Create(Color.Blue),
                    this.MaleAvatarPictureBox.SizeProperty),
                new SquareBorderRenderer(
                    SharedProperty.Create(
                        this.ContentManager.Load<Texture2D>("Sprites/Blank")),
                    this.MaleAvatarPictureBox.PositionProperty,
                    this.MaleAvatarPictureBox.SizeProperty,
                    this.MaleAvatarPictureBox.BorderColorProperty));
            this.MaleAvatarPictureBox.Behaviors.Add(
                new RadioButtonBehavior(
                    this.ScreenManager.InputState,
                    this.MaleAvatarPictureBox.PositionProperty,
                    this.MaleAvatarPictureBox.SizeProperty,
                    this.MaleAvatarPictureBox.IsSelectedProperty,
                    this.FemaleAvatarPictureBox.IsSelectedProperty,
                    this.BearAvatarPictureBox.IsSelectedProperty));
            this.MaleAvatarPictureBox.MessageDispatcher.Register(
                new SwitchableColorMessageHandler(
                    "IsSelectedProperty",
                    this.MaleAvatarPictureBox.BorderColorProperty,
                    Color.Gold,
                    Color.Black));

            this.FemaleAvatarPictureBox.PositionProperty.Value = new Vector2(300, 50);
            this.FemaleAvatarPictureBox.SizeProperty.Value = new Vector2(200, 200);
            this.FemaleAvatarPictureBox.TextureProperty.Value = this.ContentManager.Load<Texture2D>("Sprites/Blank");
            this.FemaleAvatarPictureBox.Renderer = new CompositeRenderer(
                new AdvancedRenderer(
                    this.FemaleAvatarPictureBox.TextureProperty,
                    this.FemaleAvatarPictureBox.PositionProperty,
                    SharedProperty.Create(Color.Blue),
                    this.FemaleAvatarPictureBox.SizeProperty),
                new SquareBorderRenderer(
                    SharedProperty.Create(
                        this.ContentManager.Load<Texture2D>("Sprites/Blank")),
                    this.FemaleAvatarPictureBox.PositionProperty,
                    this.FemaleAvatarPictureBox.SizeProperty,
                    this.FemaleAvatarPictureBox.BorderColorProperty));
            this.FemaleAvatarPictureBox.Behaviors.Add(
                new RadioButtonBehavior(
                    this.ScreenManager.InputState,
                    this.FemaleAvatarPictureBox.PositionProperty,
                    this.FemaleAvatarPictureBox.SizeProperty,
                    this.FemaleAvatarPictureBox.IsSelectedProperty,
                    this.MaleAvatarPictureBox.IsSelectedProperty,
                    this.BearAvatarPictureBox.IsSelectedProperty));
            this.FemaleAvatarPictureBox.MessageDispatcher.Register(
                new SwitchableColorMessageHandler(
                    "IsSelectedProperty",
                    this.FemaleAvatarPictureBox.BorderColorProperty,
                    Color.Gold,
                    Color.Black));

            this.BearAvatarPictureBox.PositionProperty.Value = new Vector2(550, 50);
            this.BearAvatarPictureBox.SizeProperty.Value = new Vector2(200, 200);
            this.BearAvatarPictureBox.TextureProperty.Value = this.ContentManager.Load<Texture2D>("Sprites/Blank");
            this.BearAvatarPictureBox.Renderer = new CompositeRenderer(
                new AdvancedRenderer(
                    this.BearAvatarPictureBox.TextureProperty,
                    this.BearAvatarPictureBox.PositionProperty,
                    SharedProperty.Create(Color.Blue),
                    this.BearAvatarPictureBox.SizeProperty),
                new SquareBorderRenderer(
                    SharedProperty.Create(
                        this.ContentManager.Load<Texture2D>("Sprites/Blank")),
                    this.BearAvatarPictureBox.PositionProperty,
                    this.BearAvatarPictureBox.SizeProperty,
                    this.BearAvatarPictureBox.BorderColorProperty));
            this.BearAvatarPictureBox.Behaviors.Add(
                new RadioButtonBehavior(
                    this.ScreenManager.InputState,
                    this.BearAvatarPictureBox.PositionProperty,
                    this.BearAvatarPictureBox.SizeProperty,
                    this.BearAvatarPictureBox.IsSelectedProperty,
                    this.MaleAvatarPictureBox.IsSelectedProperty,
                    this.FemaleAvatarPictureBox.IsSelectedProperty));
            this.BearAvatarPictureBox.MessageDispatcher.Register(
                new SwitchableColorMessageHandler(
                    "IsSelectedProperty",
                    this.BearAvatarPictureBox.BorderColorProperty,
                    Color.Gold,
                    Color.Black));
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
        /// When the user clicks the start button, we need to create the player
        /// and then transition to the bee yard screen. The player starts at their house.
        /// </summary>
        private void OnStartButtonClick()
        {
            // TODO: Need to actually pass the player avatar here.
            this.PlayerManager.CreatePlayer(
                this.PlayerSlotKey, 
                this.BeekeeperNameTextBox.TextProperty.Value,
                null);

            this.ScreenManager.TransitionToScreen(new BeeYardScreen());
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
            this.StartButton.Update(gameTime);
            this.BeekeeperNameTextBox.Update(gameTime);

            this.MaleAvatarPictureBox.Update(gameTime);
            this.FemaleAvatarPictureBox.Update(gameTime);
            this.BearAvatarPictureBox.Update(gameTime);
        }

        /// <summary>
        /// Draws all the components on the screen by calling their renderers.
        /// </summary>
        /// <param name="gameTime">The current GameTime.</param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            this.BackButton.Renderer.Render(this.ScreenManager.SpriteBatch, gameTime);
            this.StartButton.Renderer.Render(this.ScreenManager.SpriteBatch, gameTime);
            this.BeekeeperNameTextBox.Renderer.Render(this.ScreenManager.SpriteBatch, gameTime);

            this.MaleAvatarPictureBox.Renderer.Render(this.ScreenManager.SpriteBatch, gameTime);
            this.FemaleAvatarPictureBox.Renderer.Render(this.ScreenManager.SpriteBatch, gameTime);
            this.BearAvatarPictureBox.Renderer.Render(this.ScreenManager.SpriteBatch, gameTime);
        }
    }
}
