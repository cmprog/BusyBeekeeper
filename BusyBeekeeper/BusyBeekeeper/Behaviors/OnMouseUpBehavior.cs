using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TccLib.Xna.Framework;
using BusyBeekeeper.GameStateManagement;

namespace BusyBeekeeper.Behaviors
{
    /// <summary>
    /// This behavior is responsible for performing an action whenever the mouse button
    /// is released within a rectangle defined by the size and position given.
    /// </summary>
    public class OnMouseUpBehavior : IBehavior
    {
        /// <summary>
        /// Initializes a new instance of the OnMouseUpBehavior class.
        /// </summary>
        /// <param name="inputState">The input state for getting the mouse position.</param>
        /// <param name="action">The action to be called when the mouse toggles from down to up within the bounds.</param>
        /// <param name="positionProperty">The property of the position of the focus bounds.</param>
        /// <param name="sizeProperty">The property of the size of the focus bounds.</param>
        public OnMouseUpBehavior(
            InputState inputState,
            Action action,
            ISharedProperty<Vector2> positionProperty,
            ISharedProperty<Vector2> sizeProperty)
        {
            this.InputState = inputState;
            this.Action = action;
            this.PositionProperty = positionProperty;
            this.SizeProperty = sizeProperty;
        }

        private Action Action { get; set; }
        
        /// <summary>
        /// Gets or sets the input state containing the mose position.
        /// </summary>
        private InputState InputState { get; set; }

        /// <summary>
        /// Gets or sets the property containing the positino of the focus bounds.
        /// </summary>
        private ISharedProperty<Vector2> PositionProperty { get; set; }

        /// <summary>
        /// Gets or sets the property containing the size of the focus bounds.
        /// </summary>
        private ISharedProperty<Vector2> SizeProperty { get; set; }

        /// <summary>
        /// When we updated, we check to see if the left-button state of the mouse
        /// changed from down to up and is within the bounds, if so we call the action.
        /// </summary>
        /// <param name="gameTime">The current GameTime.</param>
        public void Update(GameTime gameTime)
        {
            if (this.InputState.IsLeftMouseClick)
            {
                var isInBounds =
                    VectorHelper.RectangleContains(
                        this.PositionProperty.Value,
                        this.SizeProperty.Value,
                        this.InputState.CurrentMouseState.X,
                        this.InputState.CurrentMouseState.Y);

                if (isInBounds)
                {
                    this.Action();
                }
            }
        }
    }
}
