using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BusyBeekeeper.Screens.CommonComponents
{
    internal sealed partial class ButtonMenuComponent : ScreenComponent
    {
        #region Instance Fields --------------------------------------------------------

        private readonly Color mMenuButtonTextColor = Color.Black;
        private readonly Vector2 mScreenSize;
        private readonly Vector2 mMenuButtonSize = new Vector2(125, 40);
        private readonly Collection mMenuButtons;
        private IBackgroundRenderer mBackgroundRenderer;

        private Texture2D mBlankTexture;
        private SpriteFont mFontMenuButton;

        private bool mIsInvalid;

        #endregion

        #region Constructors -----------------------------------------------------------
        
        public ButtonMenuComponent(Vector2 screenSize)
        {
            this.mScreenSize = screenSize;
            this.mMenuButtons = new Collection(this);
            this.IsVisible = true;
        }

        #endregion

        #region Instance Properties ----------------------------------------------------

        public Collection MenuButtons
        {
            get { return this.mMenuButtons; }
        }

        public float MenuButtonMargin { get { return 5; } }
        public IBackgroundRenderer MenuButtonBackgroundRender { get { return this.mBackgroundRenderer; } }
        public Color MenuButtonTextColor { get { return this.mMenuButtonTextColor; } }
        public Vector2 MenuButtonSize { get { return this.mMenuButtonSize; } }
        public SpriteFont MenuButtonFont { get { return this.mFontMenuButton; } }

        public bool IsVisible { get; set; }

        #endregion

        #region Instance Methods -------------------------------------------------------

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.mIsInvalid)
            {
                var lMenuButtonPosition = new Vector2(
                    this.mScreenSize.X - this.MenuButtonMargin - this.mMenuButtonSize.X,
                    this.MenuButtonMargin);

                for (int lIndex = 0; lIndex < this.MenuButtons.Count; lIndex++)
                {
                    var lChildMenuButton = this.MenuButtons[lIndex];
                    if (!lChildMenuButton.IsVisible) continue;

                    lChildMenuButton.Position = lMenuButtonPosition;
                    lMenuButtonPosition.Y += this.MenuButtonSize.Y + this.MenuButtonMargin;
                }

                this.mIsInvalid = false;
            }

            foreach (var lMenuButton in this.MenuButtons)
            {
                lMenuButton.Update(gameTime);
            }
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);

            this.mFontMenuButton = contentManager.Load<SpriteFont>("Fonts/DefaultSmall");
            this.mBlankTexture = contentManager.Load<Texture2D>("Sprites/Blank");
            this.mBackgroundRenderer = new SolidBackgroundRenderer(this.mBlankTexture, Color.Gold);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);

            if (this.IsVisible)
            {
                foreach (var lMenuButton in this.MenuButtons)
                {
                    lMenuButton.Draw(spriteBatch, gameTime);
                }
            }
        }

        public override bool HandleInput(InputState inputState)
        {
            if (!this.IsVisible) return false;

            foreach (var lMenuButton in this.MenuButtons)
            {
                if (lMenuButton.HandleInput(inputState)) return true;
            }

            if (inputState.MouseLeftClickUp())
            {
                this.CloseAllMenuButtons();
            }

            return false;
        }

        public void Invalidate()
        {
            this.mIsInvalid = true;
        }

        public void CloseAllMenuButtons()
        {
            foreach (var lMenuButton in this.MenuButtons)
            {
                lMenuButton.CloseAllChildMenuButtons();
            }
        }

        #endregion
        
    }
}
