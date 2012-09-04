using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TccLib.Xna.Framework
{
    /// <summary>
    /// Represents a basic property which can be shared between various
    /// entities and components. When the property is changed, 
    /// </summary>
    /// <typeparam name="T">The type of the value contained by this property.</typeparam>
    public class NotifyingSharedProperty<T> : SharedProperty<T>, ISharedProperty<T>
    {
        /// <summary>
        /// Initializes a new instance of the NotifyingSharedProperty class.
        /// </summary>
        /// <param name="messageDispatcher">The message dispatcher in which to post message to.</param>
        public NotifyingSharedProperty(MessageDispatcher messageDispatcher)
        {
            System.Diagnostics.Debug.Assert(messageDispatcher != null, "Must provide a non-null MessageDispatcher.");

            this.MessageDispatcher = messageDispatcher;
        }

        /// <summary>
        /// Gets or sets the value of the property.
        /// </summary>
        public T Value
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

                var message = PropertyChangedMessage.Create(this, oldValue);
                this.MessageDispatcher.Post(message);
            }
        }

        /// <summary>
        /// Gets the MessageDispatcher associated with this property.
        /// </summary>
        protected MessageDispatcher MessageDispatcher { get; private set; }
    }
}
