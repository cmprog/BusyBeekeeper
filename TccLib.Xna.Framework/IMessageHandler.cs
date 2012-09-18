using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TccLib.Xna.Framework
{
    /// <summary>
    /// Defines an interface for objects which handle certain types of messages.
    /// </summary>
    /// <typeparam name="TMessage">The message that the handler processes.</typeparam>
    public interface IMessageHandler<TMessage> where TMessage : IMessage
    {
        /// <summary>
        /// Processes the given message.
        /// </summary>
        /// <param name="message">The message to process.</param>
        void Process(TMessage message);
    }
}
