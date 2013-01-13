using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BusyBeekeeper.Screens.CommonComponents
{
    internal class Button : Label
    {
        public Button()
        {
            this.IsEnabled = true;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
        }

        public event Action<Button> Click;

        public bool IsEnabled { get; set; }

        public override bool HandleInput(InputState inputState)
        {
            if (!this.IsEnabled) return false;

            if (inputState.MouseLeftClickUp())
            {
                var lCurrentMouseState = inputState.CurrentMouseState;
                if (this.HitTest(lCurrentMouseState.X, lCurrentMouseState.Y))
                {
                    var lClickHandler = this.Click;
                    if (lClickHandler != null)
                    {
                        lClickHandler(this);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
