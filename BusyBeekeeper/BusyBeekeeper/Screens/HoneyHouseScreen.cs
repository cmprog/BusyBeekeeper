using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusyBeekeeper.Screens.CommonComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BusyBeekeeper.Screens
{
    internal sealed class HoneyHouseScreen : GameScreenBase
    {
        #region Instance Fields --------------------------------------------------------

        private Texture2D mBlankTexture;
        private ButtonMenuComponent mButtonMenuComponent;

        private readonly MenuButton mMenuButtonTravel = new MenuButton();

        #endregion

        #region Constructors -----------------------------------------------------------

        public HoneyHouseScreen()
        {
            //
            // mMenuButtonTravel
            //
            this.mMenuButtonTravel.Text = "Travel";
            this.mMenuButtonTravel.Click += this.MenuButtonTravel_Click;
        }

        #endregion

        #region Instance Properties ----------------------------------------------------

        #endregion

        #region Instance Methods -------------------------------------------------------

        public override void LoadContent()
        {
            base.LoadContent();

            this.mBlankTexture = this.ContentManager.Load<Texture2D>("Sprites/Blank");

            this.mButtonMenuComponent = new ButtonMenuComponent(this.ScreenSize);
            this.mButtonMenuComponent.LoadContent(this.ContentManager);
            this.mButtonMenuComponent.MenuButtons.Add(this.mMenuButtonTravel);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.mButtonMenuComponent.Update(gameTime);
        }

        public override void HandleInput(InputState inputState)
        {
            base.HandleInput(inputState);
            this.mButtonMenuComponent.HandleInput(inputState);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(this.mBlankTexture, Vector2.Zero, null, Color.Black, 0, Vector2.Zero, this.ScreenSize, SpriteEffects.None, 0);
            this.mButtonMenuComponent.Draw(spriteBatch, gameTime);
        }

        private void MenuButtonTravel_Click(MenuButton obj)
        {
            var lWorldScreen = new BeeWorldScreen();
            this.ScreenManager.TransitionTo(lWorldScreen);
        }

        #endregion
        
    }
}
