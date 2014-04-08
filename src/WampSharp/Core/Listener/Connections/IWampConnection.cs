using System;
using WampSharp.Core.Message;
using WampSharp.V1.Auxiliary.Client;

namespace WampSharp.Core.Listener
{
    /// <summary>
    /// Represents a WAMP bi-directional connection.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampConnection<TMessage> : IDisposable
    {
        void Send(WampMessage<TMessage> message);
        
        event EventHandler ConnectionOpen;
        event EventHandler<WampMessageArrivedEventArgs<TMessage>> MessageArrived;
        event EventHandler ConnectionClosed;
        event EventHandler<WampConnectionErrorEventArgs> ConnectionError;
    }
}