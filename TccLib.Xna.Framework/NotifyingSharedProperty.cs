using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TccLib.Xna.Framework
{
    /// <summary>
    /// Utility class so we can use to create a NotifyingSharedProperty using type inference.
    /// </summary>
    public static class NotifyingSharedProperty
    {
        /// <summary>
        /// Creates a new NotifyingSharedProperty using type inference.
        /// </summary>
        /// <typeparam name="T">The type of the value contained by this property.</typeparam>
        /// <param name="messageDispatcher">The message dispatcher which is responsible for dispatching events.</param>
        /// <param name="propertyName">The name of this property.</param>
        /// <param name="value">The value to initialize the property with.</param>
        /// <returns>A newly created SharedProperty with the given value.</returns>
        public static NotifyingSharedProperty<T> Create<T>(MessageDispatcher messageDispatcher, string propertyName, T value)
        {
            return new NotifyingSharedProperty<T>(messageDispatcher, propertyName, value);
        }
    }

    /// <summary>
    /// Represents a basic property which can be shared between various
    /// entities and components. When the property is changed, 
    /// </summary>
    /// <typeparam name="T">The type of the value contained by this property.</typeparam>
    public class NotifyingSharedProperty<T> : SharedProperty<T>
    {
        /// <summary>
        /// Initializes a new instance of the NotifyingSharedProperty class.
        /// </summary>
        /// <param name="messageDispatcher">The message dispatcher in which to post message to.</param>
        public NotifyingSharedProperty(MessageDispatcher messageDispatcher, string propertyName, T value)
            : base(value)
        {
            System.Diagnostics.Debug.Assert(messageDispatcher != null, "Must provide a non-null MessageDispatcher.");
            System.Diagnostics.Debug.Assert(!string.IsNullOrWhiteSpace(propertyName), "Property name cannot be null or whitespace.");

            this.MessageDispatcher = messageDispatcher;
            this.Name = propertyName;
        }

        /// <summary>
        /// Gets or sets the value of the property.
        /// </summary>
        public override T Value
        {
            get 
            { 
                return base.Value; 
            }

            set
            {
                if (EqualityComparer<T>.Default.Equals(base.Value, value))
                {
                    return;
                }

                var oldValue = base.Value;
                base.Value = value;

                var message = PropertyChangedMessage.Create(this.Name, this, oldValue);
                this.MessageDispatcher.Post(message);
            }
        }

        /// <summary>
        /// Gets the name of this shared property.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the MessageDispatcher associated with this property.
        /// </summary>
        protected MessageDispatcher MessageDispatcher { get; private set; }
    }
}
