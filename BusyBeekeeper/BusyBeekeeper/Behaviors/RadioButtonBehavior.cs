using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TccLib.Xna.Framework;
using BusyBeekeeper.GameStateManagement;

namespace BusyBeekeeper.Behaviors
{
    /// <summary>
    /// This behavior tracks mouse button clicks and selects a selectable property
    /// when the mouse is clicked within the given bounds. It then marks all other
    /// selectable properties as not selected.
    /// </summary>
    public class RadioButtonBehavior : IBehavior
    {
        /// <summary>
        /// Initializes a new instance of the RadioButtonBehavior class.
        /// </summary>
        public RadioButtonBehavior(
            InputState inputState,
            ISharedProperty<Vector2> positionProperty,
            ISharedProperty<Vector2> sizeProperty,
            ISharedProperty<bool> mainSelectedProperty,
            params ISharedProperty<bool>[] secondarySelectedProperties)
        {
            this.InputState = inputState;
            this.PositionProperty = positionProperty;
            this.SizeProperty = sizeProperty;
            this.MainSelectedProperty = mainSelectedProperty;
            this.SecondarySelectedProperties = secondarySelectedProperties;
        }

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
        /// Gets or sets the main boolean property to toggle to true
        /// when the mouse clicks within our defined bounds.
        /// </summary>
        private ISharedProperty<bool> MainSelectedProperty { get; set; }

        /// <summary>
        /// Gets or sets an enumerable collection of boolean properties
        /// which should be toggled off when the the mouse clicks within
        /// our defined bounds.
        /// </summary>
        private IEnumerable<ISharedProperty<bool>> SecondarySelectedProperties { get; set; }

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
                    this.MainSelectedProperty.Value = true;

                    foreach (var lProperty in this.SecondarySelectedProperties)
                    {
                        lProperty.Value = false;
                    }
                }
            }
        }
    }
}
