using System;

namespace TccLib.Xna.Framework
{
    /// <summary>
    /// A Component is a simple base class for any game component which is an aggregation of properties and behaviors.
    /// </summary>
    public abstract class Component
    {
        /// <summary>
        /// Used to keep track of what ID we need to use when new components are created.
        /// </summary>
        private static long sIdCounter = 0;

        /// <summary>
        /// Creates a new component.
        /// </summary>
        public Component()
        {
            this.Id = sIdCounter++;
            this.MessageDispatcher = new MessageDispatcher();
        }

        /// <summary>
        /// Gets an identification value associated with the component. This is globally unique.
        /// </summary>
        public long Id { get; private set; }

        /// <summary>
        /// Gets the message dispatcher associated with the component. Used for dispatching
        /// local Component-scoped messages.
        /// </summary>
        public MessageDispatcher MessageDispatcher { get; private set; }
    }
}
