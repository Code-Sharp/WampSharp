using System;
using WampSharp.Core.Message;

namespace WampSharp.Core.Listener
{
    /// <summary>
    /// Represents <see cref="EventArgs"/> for an incoming message.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <remarks>
    /// Used for <see cref="IWampConnection{TMessage}.MessageArrived"/>.
    /// </remarks>
    public class WampMessageArrivedEventArgs<TMessage> : EventArgs
    {
        private readonly WampMessage<TMessage> mMessage;

        /// <summary>
        /// Initializes a new instance of <see cref="WampMessageArrivedEventArgs{TMessage}"/>.
        /// </summary>
        /// <param name="message">The message </param>
        public WampMessageArrivedEventArgs(WampMessage<TMessage> message)
        {
            mMessage = message;
        }

        /// <summary>
        /// Gets the arrived message.
        /// </summary>
        public WampMessage<TMessage> Message
        {
            get
            {
                return mMessage;
            }
        }
    }
}