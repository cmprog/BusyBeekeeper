using System;
using System.Collections.Generic;

namespace TccLib.Xna.Framework
{
    /// <summary>
    /// Responsible for dispatching events to interested parties.
    /// </summary>
    public class MessageDispatcher
    {
        public MessageDispatcher()
        {
            this.ListenersByType = new Dictionary<Type, ICollection<object>>();
        }

        /// <summary>
        /// Gets or sets a collection of listeners grouped by the type of message they
        /// are responsible for handling messages for.
        /// </summary>
        private Dictionary<Type, ICollection<object>> ListenersByType { get; set; }

        /// <summary>
        /// Registers the handler to the list of registered handlers.
        /// </summary>
        /// <typeparam name="TMessage">The type of message the handler handles.</typeparam>
        /// <param name="handler">The handler for the message.</param>
        public void Register<TMessage>(Action<TMessage> handler) where TMessage : IMessage
        {
            System.Diagnostics.Debug.Assert(handler != null);

            ICollection<object> lListeners;
            if (!this.ListenersByType.TryGetValue(typeof(TMessage), out lListeners))
            {
                lListeners = new LinkedList<object>();
                this.ListenersByType.Add(typeof(TMessage), lListeners);
            }

            lListeners.Add(handler);
        }

        /// <summary>
        /// Removes the given handler from the list of registered handlers.
        /// </summary>
        /// <typeparam name="TMessage">The type of message this handler handles.</typeparam>
        /// <param name="handler">The message handler.</param>
        public void Unregister<TMessage>(Action<TMessage> handler) where TMessage : IMessage
        {
            System.Diagnostics.Debug.Assert(handler != null);

            ICollection<object> lListeners;
            if (!this.ListenersByType.TryGetValue(typeof(TMessage), out lListeners))
            {
                throw new ArgumentException("No listeners registered for type [" + typeof(TMessage) + "].");
            }

            lListeners.Remove(handler);
        }

        /// <summary>
        /// Posts the given message to be processed by any registed handlers for the message type.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message which should be posted.</typeparam>
        /// <param name="message">The message which should be processed.</param>
        public void Post<TMessage>(TMessage message) where TMessage : IMessage
        {
            System.Diagnostics.Debug.Assert(handler != null);

            ICollection<object> lListeners;
            if (this.ListenersByType.TryGetValue(typeof(TMessage), out lListeners))
            {
                foreach (var lRawAction in lListeners)
                {
                    var lAction = (Action<TMessage>)lRawAction;
                    lAction(message);
                }
            }
        }
    }
}
