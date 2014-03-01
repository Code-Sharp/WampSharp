using System;
using WampSharp.Core.Message;

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
        event EventHandler ConnectionOpening;
        event EventHandler<WampMessageArrivedEventArgs<TMessage>> MessageArrived;
        event EventHandler ConnectionClosing;
        event EventHandler ConnectionClosed;
    }

    public class WampMessageArrivedEventArgs<TMessage> : EventArgs
    {
        private readonly WampMessage<TMessage> mMessage;

        public WampMessageArrivedEventArgs(WampMessage<TMessage> message)
        {
            mMessage = message;
        }

        public WampMessage<TMessage> Message
        {
            get
            {
                return mMessage;
            }
        }
    }
}