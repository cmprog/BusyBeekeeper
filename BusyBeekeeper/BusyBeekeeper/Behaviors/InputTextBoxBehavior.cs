using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TccLib.Xna.Framework;

namespace BusyBeekeeper.Behaviors
{
    /// <summary>
    /// This behavior monitors an InputState and populates a text property based on the user's
    /// input patterns.
    /// </summary>
    public class InputTextBoxBehavior : IBehavior
    {
        /// <summary>
        /// Initializes a new instance of the InputTextBoxBehavior class.
        /// </summary>
        public InputTextBoxBehavior(
            InputState inputState,
            ISharedProperty<string> textProperty)
        {
            this.InputState = inputState;
            this.TextProperty = textProperty;
        }

        /// <summary>
        /// Gets or sets the InputState monitoring keyboard input.
        /// </summary>
        private InputState InputState { get; set; }

        /// <summary>
        /// Gets or sets the property containing the text to update.
        /// </summary>
        private ISharedProperty<string> TextProperty { get; set; }

        /// <summary>
        /// Checks to see if certain keys were have been typed and updates
        /// the text property accordingly.
        /// </summary>
        /// <param name="gameTime">The current GameTime.</param>
        public void Update(GameTime gameTime)
        {
            if (this.InputState.IsNewKeyPress(Keys.Space))
            {
                this.TextProperty.Value += ' ';
            }
            else if (this.InputState.IsNewKeyPress(Keys.Back))
            {
                if (this.TextProperty.Value.Length > 0)
                {
                    this.TextProperty.Value = 
                        this.TextProperty.Value.Substring(0, this.TextProperty.Value.Length - 1);
                }
            }
            else
            {
                for (var character = (char)Keys.A; character <= (char)Keys.Z; character++)
                {
                    if (this.InputState.IsNewKeyPress((Keys)character))
                    {
                        this.TextProperty.Value += character;
                        break;
                    }
                }
            }
        }
    }
}
