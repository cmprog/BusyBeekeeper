﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TccLib.Xna.Framework
{
    /// <summary>
    /// Utility class so we can create instances of PropertyChangedMessage with type inference.
    /// </summary>
    public static class PropertyChangedMessage
    {
        /// <summary>
        /// Creates a new PropertyChangedMessage based on the given property and old value.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <typeparam name="TValue">The type of value the property held.</typeparam>
        /// <param name="property">The property that changed.</param>
        /// <param name="oldValue">The old value of the property.</param>
        /// <returns>The newly created PropertyChangedMessage.</returns>
        public static PropertyChangedMessage<TProperty, TValue> Create<TProperty, TValue>(TProperty property, TValue oldValue) where TProperty : ISharedProperty<TValue>
        {
            return new PropertyChangedMessage<TProperty, TValue>(property, oldValue);
        }
    }

    /// <summary>
    /// Defines a simple message for notifying when a property changed.
    /// </summary>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <typeparam name="TValue">The type of value the property held.</typeparam>
    public class PropertyChangedMessage<TProperty, TValue> : IMessage where TProperty : ISharedProperty<TValue>
    {
        /// <summary>
        /// Initializes a new instance of the PropertyChangedMessage class.
        /// </summary>
        /// <param name="property">The property that changed.</param>
        /// <param name="oldValue">The old value of the property.</param>
        public PropertyChangedMessage(TProperty property, TValue oldValue)
        {
            System.Diagnostics.Debug.Assert(property != null, "Must provide a non-null property.");

            this.Property = property;
            this.OldValue = oldValue;
        }

        /// <summary>
        /// Gets the new value asigned to the property.
        /// </summary>
        public TValue NewValue 
        {
            get { return this.Property.Value; } 
        }

        /// <summary>
        /// Gets the old value which was assiged to the property.
        /// </summary>
        public TValue OldValue { get; private set; }

        /// <summary>
        /// Gets the property.
        /// </summary>
        public TProperty Property { get; private set; }
    }
}
