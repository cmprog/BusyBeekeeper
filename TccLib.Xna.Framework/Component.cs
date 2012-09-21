using System;
using System.Xml.Serialization;
using System.Collections.Generic;

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
        private static long idCounter = 0;

        /// <summary>
        /// Initializes a new instance of the Component class.
        /// </summary>
        public Component()
        {
            this.Id = idCounter++;
            this.MessageDispatcher = new MessageDispatcher();
            this.Behaviors = new HashSet<IBehavior>();
        }

        /// <summary>
        /// Gets an identification value associated with the component. This is globally unique.
        /// </summary>
        [XmlIgnore]
        public long Id { get; private set; }

        /// <summary>
        /// Gets the message dispatcher associated with the component. Used for dispatching
        /// local Component-scoped messages.
        /// </summary>
        [XmlIgnore]
        public MessageDispatcher MessageDispatcher { get; private set; }

        /// <summary>
        /// Gets a collection of behaviors associated with the component.
        /// </summary>
        [XmlIgnore]
        public ICollection<IBehavior> Behaviors { get; private set; }

        /// <summary>
        /// Deturmines whether this component is equal to another object (which may or may not be
        /// another component).
        /// </summary>
        /// <param name="obj">The object to compare this component to.</param>
        /// <returns>True if they are equal, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var asComponent = obj as Component;

            if (asComponent == null)
            {
                return false;
            }
            
            return this.Id == asComponent.Id;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>An appropriate hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        /// <summary>
        /// Returns a System.String that represents the current Component.
        /// </summary>
        /// <returns>A string representation of the Component.</returns>
        public override string ToString()
        {
            return "Component[" + this.Id + ']';
        }
    }
}
