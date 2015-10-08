using System;
using WampSharp.Core.Message;
using WampSharp.V2.MetaApi;

namespace WampSharp.Core.Listener
{
    /// <summary>
    /// Represents a WAMP bi-directional connection with transport details.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IDetailedWampConnection<TMessage> : IWampConnection<TMessage>
    {
        /// <summary>
        /// Gets the transport details associated with this connection.
        /// </summary>
        WampTransportDetails TransportDetails { get; }
    }

    /// <summary>
    /// Represents a WAMP bi-directional connection.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampConnection<TMessage> : IDisposable
    {
        /// <summary>
        /// Sends a given message through the wire.
        /// </summary>
        /// <param name="message">The given message.</param>
        void Send(WampMessage<object> message);
        
        /// <summary>
        /// Occurs when this connection opens.
        /// </summary>
        event EventHandler ConnectionOpen;

        /// <summary>
        /// Occurs when a message arrives.
        /// </summary>
        event EventHandler<WampMessageArrivedEventArgs<TMessage>> MessageArrived;
        
        /// <summary>
        /// Occurs when this connection closes.
        /// </summary>
        event EventHandler ConnectionClosed;
        
        /// <summary>
        /// Occurs when this connection occurs an error.
        /// </summary>
        event EventHandler<WampConnectionErrorEventArgs> ConnectionError;
    }
}