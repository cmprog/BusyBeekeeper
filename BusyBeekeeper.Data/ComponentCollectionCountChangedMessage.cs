using System;
using TccLib.Xna.Framework;

namespace BusyBeekeeper.Data
{
    /// <summary>
    /// Collection of helper methods to create ComponentCollectionCountChangedMessage
    /// objects using type inference.
    /// </summary>
    public static class ComponentCollectionCountChangedMessage
    {
        /// <summary>
        /// Creates a new instance of the ComponentCollectionCountChangedMessage class.
        /// </summary>
        /// <typeparam name="T">The type of value contained in the ComponentCollection.</typeparam>
        /// <param name="collection">The ComponentCollection originating this message.</param>
        /// <param name="oldCount">The previous item count of the collection.</param>
        /// <returns>The new ComponentCollectionCountChangedMessage instance.</returns>
        public static ComponentCollectionCountChangedMessage<T> Create<T>(ComponentCollection<T> collection, int oldCount)
        {
            return new ComponentCollectionCountChangedMessage<T>(collection, oldCount);
        }
    }

    /// <summary>
    /// This custom message gets posed when a ComponentCollection's Count property changes.
    /// </summary>
    /// <typeparam name="T">The type of value contained in the ComponentCollection.</typeparam>
    public class ComponentCollectionCountChangedMessage<T> : IMessage
    {
        /// <summary>
        /// Initializes a new instance of the ComponentCollectionCountChangedMessage class.
        /// </summary>
        /// <param name="oldCount">The old number of items.</param>
        /// <param name="newCount">The new number of items.</param>
        public ComponentCollectionCountChangedMessage(ComponentCollection<T> collection, int oldCount)
        {
            this.Collection = collection;
            this.OldCount = oldCount;
        }

        /// <summary>
        /// Gets the collection which posted this message.
        /// </summary>
        public ComponentCollection<T> Collection { get; private set; }

        /// <summary>
        /// Gets the name of this collection.
        /// </summary>
        public string CollectionName { get { return this.Collection.Name; } }

        /// <summary>
        /// Gets the old item count for the collection before this message was posted.
        /// </summary>
        public int OldCount { get; private set; }

        /// <summary>
        /// Gets the new item count for the collection.
        /// </summary>
        public int NewCount { get { return this.Collection.Count; } }
    }
}
