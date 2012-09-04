using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TccLib.Xna.Framework;
using TccLib.Xna.GameStateManagement;

namespace BusyBeekeeper.Behaviors
{
    /// <summary>
    /// This behavior changes the sprite font based on whether or not the input state
    /// places the mouse over the rectangle defined by the size and position given.
    /// </summary>
    public class OnFocusFontChangeBehavior : IBehavior
    {
        /// <summary>
        /// Initializes a new instance of the OnFocutFontChangeBehavior class.
        /// </summary>
        /// <param name="inputState">The input state for getting the mouse position.</param>
        /// <param name="outFocusFont">The font to set when the mouse is out of the focus bounds.</param>
        /// <param name="inFocusFont">The font to set when the mouse if in the focus bounds.</param>
        /// <param name="positionProperty">The property of the position of the focus bounds.</param>
        /// <param name="sizeProperty">The property of the size of the focus bounds.</param>
        /// <param name="fontProperty">The property of the font to set.</param>
        public OnFocusFontChangeBehavior(
            InputState inputState,
            SpriteFont outFocusFont,
            SpriteFont inFocusFont,
            ISharedProperty<Vector2> positionProperty,
            ISharedProperty<Vector2> sizeProperty,
            ISharedProperty<SpriteFont> fontProperty)
        {
            this.InputState = inputState;
            this.OutFocusFont = outFocusFont;
            this.InFocusFont = inFocusFont;
            this.PositionProperty = positionProperty;
            this.SizeProperty = sizeProperty;
            this.FontProperty = fontProperty;
        }

        /// <summary>
        /// Gets or sets the input state containing the mose position.
        /// </summary>
        private InputState InputState { get; set; }
        
        /// <summary>
        /// Gets or sets the font to set when the mouse is in the focus bounds.
        /// </summary>
        private SpriteFont InFocusFont { get; set; }

        /// <summary>
        /// Gets or sets the font to set when the mouse is out of the focus bounds.
        /// </summary>
        private SpriteFont OutFocusFont { get; set; }

        /// <summary>
        /// Gets or sets the property containing the positino of the focus bounds.
        /// </summary>
        private ISharedProperty<Vector2> PositionProperty { get; set; }

        /// <summary>
        /// Gets or sets the property containing the size of the focus bounds.
        /// </summary>
        private ISharedProperty<Vector2> SizeProperty { get; set; }

        /// <summary>
        /// Gets or sets the property containing the font to update.
        /// </summary>
        private ISharedProperty<SpriteFont> FontProperty { get; set; }

        /// <summary>
        /// When we update, we just check the input state to see if it is within the bounds
        /// specified by the position and size properties, then update the font property
        /// accordingly.
        /// </summary>
        /// <param name="gameTime">The current GameTime.</param>
        public void Update(GameTime gameTime)
        {
            var isInFocus =
                VectorHelper.RectangleContains(
                    this.PositionProperty.Value, 
                    this.SizeProperty.Value, 
                    this.InputState.CurrentMouseState.X, 
                    this.InputState.CurrentMouseState.Y);

            this.FontProperty.Value = isInFocus ? this.InFocusFont : this.OutFocusFont;
        }
    }
}
