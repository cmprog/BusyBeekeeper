using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TccLib.Xna.Framework
{
    /// <summary>
    /// Utility class so we can use to create a SharedProperty using type inference.
    /// </summary>
    public static class SharedProperty
    {
        /// <summary>
        /// Creates a new SharedProperty using type inference.
        /// </summary>
        /// <typeparam name="T">The type of the value contained by this property.</typeparam>
        /// <param name="value">The value to initialize the property with.</param>
        /// <returns>A newly created SharedProperty with the given value.</returns>
        public static SharedProperty<T> Create<T>(T value)
        {
            return new SharedProperty<T>(value);
        }
    }

    /// <summary>
    /// Represents a basic property which can be shared between various
    /// entities and components. This class provides only simple access
    /// and setter behavior with no notification.
    /// </summary>
    /// <typeparam name="T">The type of the value contained by this property.</typeparam>
    public class SharedProperty<T> : ISharedProperty<T>
    {
        /// <summary>
        /// Initializes a new instance of the SharedProperty class.
        /// </summary>
        public SharedProperty()
            : this(default(T))
        {
        }

        /// <summary>
        /// Initializes a new instance of the SharedProperty class.
        /// </summary>
        /// <param name="value">The value to initialize the value with.</param>
        public SharedProperty(T value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the value of the property.
        /// </summary>
        public virtual T Value { get; set; }
    }
}
