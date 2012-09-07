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
    /// This screen allows the  user to manage the various games they have going on. They can create
    /// a new game, continue a previously started game, or delete an existing game.
    /// </summary>
    internal class GameSelectionScreen : GameScreen
    {
        /// <summary>
        /// Initializes a new instance of the GameSelectionScreen class.
        /// </summary>
        public GameSelectionScreen()
        {
            this.GameSlotTop = new GameSlotButtonComponent();
            this.GameSlotMiddle = new GameSlotButtonComponent();
            this.GameSlotBottom = new GameSlotButtonComponent();
            this.TitleButton = new TextButtonComponent();
            this.PlayerManager = new PlayerManager();
        }

        /// <summary>
        /// Gets or sets the button for the top game slot.
        /// </summary>
        private GameSlotButtonComponent GameSlotTop { get; set; }

        /// <summary>
        /// Gets or sets the button for the middle game slot.
        /// </summary>
        private GameSlotButtonComponent GameSlotMiddle { get; set; }

        /// <summary>
        /// Gets or sets the button for the bottom game slot.
        /// </summary>
        private GameSlotButtonComponent GameSlotBottom { get; set; }

        /// <summary>
        /// Gets or sets the button for returning to the title screen.
        /// </summary>
        private TextButtonComponent TitleButton { get; set; }

        /// <summary>
        /// Gets or sets the PlayerManager used to gather player information.
        /// </summary>
        private PlayerManager PlayerManager { get; set; }

        /// <summary>
        /// Loads the screen content.
        /// </summary>
        public override void Load()
        {
            base.Load();

            this.Load(this.GameSlotTop, PlayerManager.TopSlotKey, 150);
            this.Load(this.GameSlotMiddle, PlayerManager.MiddleSlotKey, 250);
            this.Load(this.GameSlotBottom, PlayerManager.BottomSlotKey, 350);

            this.TitleButton.TextProperty.Value = "Title";
            this.TitleButton.PositionProperty.Value = new Vector2(10, 420);
            this.TitleButton.SizeProperty.Value = new Vector2(100, 50);
            this.TitleButton.FontProperty.Value = this.ContentManager.Load<SpriteFont>("Fonts/BasicFont");
            this.TitleButton.Renderer = new CompositeRenderer(
                new AdvancedRenderer(
                    SharedProperty.Create(
                        this.ContentManager.Load<Texture2D>("Sprites/Blank")),
                    this.TitleButton.PositionProperty,
                    SharedProperty.Create(Color.Gold),
                    this.TitleButton.SizeProperty),
                new CenteredTextRenderer(
                    this.TitleButton.TextProperty,
                    this.TitleButton.FontProperty,
                    this.TitleButton.PositionProperty,
                    this.TitleButton.SizeProperty));
            this.TitleButton.Behaviors.Add(
                new OnFocusFontChangeBehavior(
                    this.ScreenManager.InputState,
                    this.ContentManager.Load<SpriteFont>("Fonts/BasicFont"),
                    this.ContentManager.Load<SpriteFont>("Fonts/BasicFontBold"),
                    this.TitleButton.PositionProperty,
                    this.TitleButton.SizeProperty,
                    this.TitleButton.FontProperty));
            this.TitleButton.Behaviors.Add(
                new OnMouseUpBehavior(
                    this.ScreenManager.InputState,
                    this.OnTitleButtonClick,
                    this.TitleButton.PositionProperty,
                    this.TitleButton.SizeProperty));
        }

        /// <summary>
        /// When the user clicks the title button, we return to the title screen.
        /// </summary>
        private void OnTitleButtonClick()
        {
            this.ScreenManager.TransitionToScreen(new TitleScreen());
        }

        /// <summary>
        /// Loads the given GameSlotButtonComponent which will correspond to the given slot key.
        /// </summary>
        /// <param name="component">The component to load.</param>
        /// <param name="slotKey">The slot key to grab the player summary with.</param>
        /// <param name="positionY">The y-position of the button component.</param>
        private void Load(GameSlotButtonComponent component, int slotKey, float positionY)
        {
            var playerSummary = this.PlayerManager.LoadSummary(slotKey);

            component.SizeProperty.Value = new Vector2(500, 80);
            component.PositionProperty.Value = new Vector2(250, positionY);
            component.NameFont.Value = this.ContentManager.Load<SpriteFont>("Fonts/BasicFontBold");
            component.DetailFont.Value = this.ContentManager.Load<SpriteFont>("Fonts/BasicFont");
            component.AvatarPosition.Value = new Vector2(105, positionY + 5);
            component.AvatarSize.Value = new Vector2(70, 70);
            
            component.Behaviors.Add(
                new OnMouseUpBehavior(
                    this.ScreenManager.InputState,
                    () => this.OnGameSlotClick(playerSummary),
                    component.PositionProperty,
                    component.SizeProperty));

            if (playerSummary.PlayerExists)
            {
                // TODO
            }
            else
            {
                component.BeekeeperName.Value = "CREATE NEW BEEKEEPER";
                component.Renderer = new CompositeRenderer(
                    new AdvancedRenderer(
                        SharedProperty.Create(
                            this.ContentManager.Load<Texture2D>("Sprites/Blank")),
                        component.PositionProperty,
                        SharedProperty.Create(Color.Gold),
                        component.SizeProperty),
                    new CenteredTextRenderer(
                        component.BeekeeperName,
                        component.NameFont,
                        component.PositionProperty,
                        component.SizeProperty));

                component.Behaviors.Add(
                    new OnFocusFontChangeBehavior(
                        this.ScreenManager.InputState,
                        this.ContentManager.Load<SpriteFont>("Fonts/BasicFont"),
                        this.ContentManager.Load<SpriteFont>("Fonts/BasicFontBold"),
                        this.TitleButton.PositionProperty,
                        this.TitleButton.SizeProperty,
                        this.TitleButton.FontProperty));
            }
        }

        /// <summary>
        /// When the user clicks a game slot, starts the game if the plays is existing, or
        /// continues to the create player screen.
        /// </summary>
        private void OnGameSlotClick(PlayerSummary playerSummary)
        {
            if (playerSummary.PlayerExists)
            {
                // TODO: Continue game...
            }
            else
            {
                this.ScreenManager.TransitionToScreen(new CreatePlayerScreen(this.PlayerManager));
            }
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
            this.GameSlotTop.Update(gameTime);
            this.GameSlotMiddle.Update(gameTime);
            this.GameSlotBottom.Update(gameTime);
            this.TitleButton.Update(gameTime);
        }

        /// <summary>
        /// Draws all the components on the screen by calling their renderers.
        /// </summary>
        /// <param name="gameTime">The current GameTime.</param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            this.GameSlotTop.Renderer.Render(this.ScreenManager.SpriteBatch, gameTime);
            this.GameSlotMiddle.Renderer.Render(this.ScreenManager.SpriteBatch, gameTime);
            this.GameSlotBottom.Renderer.Render(this.ScreenManager.SpriteBatch, gameTime);
            this.TitleButton.Renderer.Render(this.ScreenManager.SpriteBatch, gameTime);
        }
    }
}
