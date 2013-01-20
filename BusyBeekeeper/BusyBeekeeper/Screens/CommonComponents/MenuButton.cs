using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BusyBeekeeper.Screens.CommonComponents
{
    internal sealed class MenuButton : ScreenComponent
    {
        #region Instance Fields --------------------------------------------------------

        private readonly MenuButtonCollection mMenuButtons;

        private bool mIsVisible;
        private bool mIsInvalid;
        private Vector2 mPosition;
        private ButtonMenuComponent mButtonMenuComponent;
        private bool mAreChildMenuButtonsVisible;

        #endregion

        #region Constructors -----------------------------------------------------------

        public MenuButton()
        {
            this.mMenuButtons = new MenuButtonCollection(this);
            this.mIsVisible = true;
        }

        #endregion

        #region Events -----------------------------------------------------------------

        public event Action<MenuButton> Click;

        #endregion

        #region Instance Properties ----------------------------------------------------

        public ButtonMenuComponent ButtonMenuComponent
        {
            get { return this.mButtonMenuComponent; }
            internal set
            {
                if (this.mButtonMenuComponent == value) return;
                this.mButtonMenuComponent = value;
                this.ChildMenuButtons.ButtonMenuComponent = value;
            }
        }

        public MenuButton ParentMenuButton { get; internal set; }

        public string Text { get; set; }

        public Vector2 Position
        {
            get { return this.mPosition; }
            set
            {
                if (this.mPosition == value) return;
                this.mPosition = value;
                this.Invalidate();
            }
        }

        public bool IsVisible
        {
            get
            {
                return
                    (((this.ParentMenuButton != null) &&
                      this.ParentMenuButton.IsVisible &&
                      this.ParentMenuButton.AreChildMenuButtonsVisible &&
                      this.mIsVisible)) ||
                    ((this.ParentMenuButton == null) &&
                     this.mIsVisible);
            }
            set
            {
                if (this.mIsVisible == value) return;
                this.mIsVisible = value;

                if (this.ParentMenuButton != null)
                {
                    this.ParentMenuButton.Invalidate();
                }
                else if (this.ButtonMenuComponent != null)
                {
                    this.ButtonMenuComponent.Invalidate();
                }
            }
        }

        public bool AreChildMenuButtonsVisible
        {
            get { return this.mAreChildMenuButtonsVisible; }
            set
            {
                if (this.mAreChildMenuButtonsVisible == value) return;
                this.mAreChildMenuButtonsVisible = value;
                this.Invalidate();
            }
        }

        #endregion

        #region Instance Methods -------------------------------------------------------

        /// <summary>
        /// Gets a collection of the child buttons for this button.
        /// </summary>
        public MenuButtonCollection ChildMenuButtons
        {
            get { return this.mMenuButtons; }
        }

        public override bool HandleInput(InputState inputState)
        {
            if (!this.IsVisible) return false;

            if (inputState.MouseLeftClickUp())
            {
                var lCurrentMouseState = inputState.CurrentMouseState;
                if (this.HitTest(lCurrentMouseState.X, lCurrentMouseState.Y))
                {
                    if (this.ParentMenuButton != null)
                    {
                        foreach (var lMenuButton in this.ParentMenuButton.ChildMenuButtons)
                        {
                            lMenuButton.CloseAllChildMenuButtons();
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.Assert(this.ButtonMenuComponent != null);
                        this.ButtonMenuComponent.CloseAllMenuButtons();
                    }

                    if (this.ChildMenuButtons.Count == 0)
                    {
                        System.Diagnostics.Debug.Assert(this.ButtonMenuComponent != null);
                        this.ButtonMenuComponent.CloseAllMenuButtons();
                    }
                    else
                    {
                        this.AreChildMenuButtonsVisible = true;
                    }

                    var lClickHandler = this.Click;
                    if (lClickHandler != null) lClickHandler(this);
                    return true;
                }
            }

            foreach (var lMenuButton in this.ChildMenuButtons)
            {
                if (lMenuButton.HandleInput(inputState)) return true;
            }

            return false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.mIsInvalid)
            {
                var lChildMenuButtonPosition = new Vector2(
                    this.mPosition.X - this.ButtonMenuComponent.MenuButtonMargin - this.ButtonMenuComponent.MenuButtonSize.X,
                    this.mPosition.Y);

                for (int lIndex = 0; lIndex < this.ChildMenuButtons.Count; lIndex++)
                {
                    var lChildMenuButton = this.ChildMenuButtons[lIndex];
                    if (!lChildMenuButton.IsVisible) continue;

                    lChildMenuButton.Position = lChildMenuButtonPosition;
                    lChildMenuButtonPosition.Y +=
                        this.ButtonMenuComponent.MenuButtonSize.Y +
                        this.ButtonMenuComponent.MenuButtonMargin;
                }

                this.mIsInvalid = false;
            }

            foreach (var lChildMenuButton in this.ChildMenuButtons)
            {
                lChildMenuButton.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (!this.IsVisible) return;

            var lBackgroundRenderer = this.ButtonMenuComponent.MenuButtonBackgroundRender;
            lBackgroundRenderer.Draw(spriteBatch, gameTime, this.mPosition, this.ButtonMenuComponent.MenuButtonSize);

            var lFont = this.ButtonMenuComponent.MenuButtonFont;
            var lText = this.Text;
            var lTextSize = lFont.MeasureString(lText);
            var lTextPosition = this.mPosition + ((this.ButtonMenuComponent.MenuButtonSize - lTextSize) / 2f);
            var lTextColor = this.ButtonMenuComponent.MenuButtonTextColor;

            spriteBatch.DrawString(lFont, lText, lTextPosition, lTextColor);

            foreach (var lChildMenuButton in this.ChildMenuButtons)
            {
                lChildMenuButton.Draw(spriteBatch, gameTime);
            }
        }

        public bool HitTest(int x, int y)
        {
            return VectorUtilities.HitTest(this.mPosition, this.ButtonMenuComponent.MenuButtonSize, x, y);
        }

        internal void Invalidate()
        {
            this.mIsInvalid = true;
        }

        #endregion

        public void CloseAllChildMenuButtons()
        {
            this.AreChildMenuButtonsVisible = false;
            foreach (var lMenuButton in this.ChildMenuButtons)
            {
                lMenuButton.CloseAllChildMenuButtons();
            }
        }
    }
}
