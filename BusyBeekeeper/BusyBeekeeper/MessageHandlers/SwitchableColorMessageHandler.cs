using System;
using System.Linq;
using Microsoft.Xna.Framework;
using TccLib.Xna.Framework;

namespace BusyBeekeeper.MessageHandlers
{
    /// <summary>
    /// This message handler handles a change a boolean property by changing the value
    /// of the color property based on the boolean value.
    /// </summary>
    public class SwitchableColorMessageHandler : IMessageHandler<PropertyChangedMessage<bool>>
    {
        /// <summary>
        /// Initializes a new instance of the SwitchableColorMessageHandler class.
        /// </summary>
        /// <param name="propertyName">The name of the property we're handing the property changed message for.</param>
        /// <param name="colorProperty">The color property to change.</param>
        /// <param name="colorIfTrue">The color to set if true.</param>
        /// <param name="colorIfFalse">The color to set if false.</param>
        public SwitchableColorMessageHandler(
            string propertyName,
            ISharedProperty<Color> colorProperty,
            Color colorIfTrue,
            Color colorIfFalse)
        {
            this.PropertyName = propertyName;
            this.ColorProperty = colorProperty;
            this.ColorIfTrue = colorIfTrue;
            this.ColorIfFalse = colorIfFalse;
        }

        /// <summary>
        /// Gets or sets the name of the property we're monitoring.
        /// </summary>
        private string PropertyName { get; set; }
        
        /// <summary>
        /// Gets or sets the property containing the color we are to change
        /// when the property we're monitoring changes to false.
        /// </summary>
        private ISharedProperty<Color> ColorProperty { get; set; }

        /// <summary>
        /// Gets or sets the color which will be set to the color property
        /// when the property we're monitoring changes to true.
        /// </summary>
        private Color ColorIfTrue { get; set; }

        /// <summary>
        /// Gets or sets the color which will be set to the color property
        /// when the property we're monitoring changes to false.
        /// </summary>
        private Color ColorIfFalse { get; set; }

        /// <summary>
        /// Processes a boolean property changed message and sets the contained
        /// color property based on the boolean property's new value.
        /// </summary>
        /// <param name="message">The property changed message to process.</param>
        public void Process(PropertyChangedMessage<bool> message)
        {
            if (message.PropertyName == this.PropertyName)
            {
                this.ColorProperty.Value = message.Property.Value ? this.ColorIfTrue : this.ColorIfFalse;
            }
        }
    }
}
