using System;
using System.Collections;
using System.Collections.Generic;
using TccLib.Xna.Framework;

namespace BusyBeekeeper.Data
{
    /// <summary>
    /// This special collection is just a basic hash-set collection, but we send
    /// a notification whenever the count changes.
    /// </summary>
    public class ComponentCollection<T> : ICollection<T>
    {
        /// <summary>
        /// Initializes a new instance of the ComponentCollection class.
        /// </summary>
        /// <param name="messageDispatcher">A MessageDispatcher we can post messages to.</param>
        /// <param name="collectionName">The name of the collection, used when posting messages.</param>
        public ComponentCollection(MessageDispatcher messageDispatcher, string collectionName)
        {
            this.Items = new HashSet<T>();
            this.MessageDispatcher = messageDispatcher;
            this.Name = collectionName;
        }

        /// <summary>
        /// Gets or sets the inner collection of items.
        /// </summary>
        private HashSet<T> Items { get; set; }

        /// <summary>
        /// Gets or sets the message dispatch we use to dispatch events.
        /// </summary>
        private MessageDispatcher MessageDispatcher { get; set; }

        /// <summary>
        /// Gets or sets the name of this collection, used when posting events.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Adds the given item to the collection.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(T item)
        {
            if (this.Items.Add(item))
            {
                this.PostCountChange(this.Count - 1);
            }
        }

        /// <summary>
        /// Clears all the items.
        /// </summary>
        public void Clear()
        {
            if (this.Count == 0) return;

            var oldCount = this.Count;
            this.Items.Clear();
            this.PostCountChange(oldCount);
        }

        /// <summary>
        /// Checks if the given item is in this collection.
        /// </summary>
        /// <param name="item">The item to look for.</param>
        /// <returns>True if the item is in this collection - false otherwise.</returns>
        public bool Contains(T item)
        {
            return this.Items.Contains(item);
        }

        /// <summary>
        /// Copies the contents of this collection to the given array.
        /// </summary>
        /// <param name="array">The array to copy to.</param>
        /// <param name="arrayIndex">The starting index to copy.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.Items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of items in the collection.
        /// </summary>
        public int Count
        {
            get { return this.Items.Count; }
        }

        /// <summary>
        /// Gets a flag indicating whether or not this collection
        /// is read only (its not).
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Attempts to remove the given item from this collection.
        /// </summary>
        /// <param name="item">The item to try to remove.</param>
        /// <returns>True if the item was removed - false otherwise.</returns>
        public bool Remove(T item)
        {
            if (this.Items.Remove(item))
            {
                this.PostCountChange(this.Count + 1);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets an enumerator to enumerate the items in this collection.
        /// </summary>
        /// <returns>An enumerator which enumerates items in this collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        /// <summary>
        /// This method gets a non-typed enumerator.
        /// </summary>
        /// <returns>An enumerator for the contents of this collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Posts a ComponentCollectionCountChangedMessage.
        /// </summary>
        /// <param name="oldCount">The old item count of the collection.</param>
        private void PostCountChange(int oldCount)
        {
            var lMessage = ComponentCollectionCountChangedMessage.Create(this, oldCount);
            this.MessageDispatcher.Post(lMessage);
        }
    }
}
