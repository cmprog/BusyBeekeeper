using System;
using System.Collections.Generic;

namespace TccLib.Xna.Framework
{
    /// <summary>
    /// Responsible for dispatching events to interested parties.
    /// </summary>
    public class MessageDispatcher
    {
        /// <summary>
        /// Initializes a new instance of the MessageDispatcher class.
        /// </summary>
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
        public void Register<TMessage>(IMessageHandler<TMessage> handler) where TMessage : IMessage
        {
            System.Diagnostics.Debug.Assert(handler != null, "Must provide a non-null handler.");

            ICollection<object> listeners;
            if (!this.ListenersByType.TryGetValue(typeof(TMessage), out listeners))
            {
                listeners = new LinkedList<object>();
                this.ListenersByType.Add(typeof(TMessage), listeners);
            }

            listeners.Add(handler);
        }

        /// <summary>
        /// Removes the given handler from the list of registered handlers.
        /// </summary>
        /// <typeparam name="TMessage">The type of message this handler handles.</typeparam>
        /// <param name="handler">The message handler.</param>
        public void Unregister<TMessage>(IMessageHandler<TMessage> handler) where TMessage : IMessage
        {
            System.Diagnostics.Debug.Assert(handler != null, "Must provide a non-null handler.");

            ICollection<object> listeners;
            if (!this.ListenersByType.TryGetValue(typeof(TMessage), out listeners))
            {
                throw new ArgumentException("No listeners registered for type [" + typeof(TMessage) + "].");
            }

            listeners.Remove(handler);
        }

        /// <summary>
        /// Posts the given message to be processed by any registed handlers for the message type.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message which should be posted.</typeparam>
        /// <param name="message">The message which should be processed.</param>
        public void Post<TMessage>(TMessage message) where TMessage : IMessage
        {
            System.Diagnostics.Debug.Assert(message != null, "Must provide a non-null message.");

            ICollection<object> listeners;
            if (this.ListenersByType.TryGetValue(typeof(TMessage), out listeners))
            {
                foreach (var rawAction in listeners)
                {
                    var handler = (IMessageHandler<TMessage>)rawAction;
                    handler.Process(message);
                }
            }
        }
    }
}
