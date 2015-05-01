using System;
using Fleck;
using WampSharp.Core.Listener;

namespace WampSharp.Fleck
{
    internal abstract class FleckWampConnection<TMessage> : AsyncWampConnection<TMessage>
    {
        protected IWebSocketConnection mWebSocketConnection;

        public FleckWampConnection(IWebSocketConnection webSocketConnection)
        {
            mWebSocketConnection = webSocketConnection;
            mWebSocketConnection.OnOpen = OnConnectionOpen;
            mWebSocketConnection.OnError = OnConnectionError;
            mWebSocketConnection.OnClose = OnConnectionClose;
        }

        private void OnConnectionOpen()
        {
            RaiseConnectionOpen();
        }

        private void OnConnectionError(Exception exception)
        {
            RaiseConnectionError(exception);
        }

        private void OnConnectionClose()
        {
            RaiseConnectionClosed();
        }

        public override void Dispose()
        {
            if (IsConnected)
            {
                mWebSocketConnection.Close();                
            }
        }

        protected override bool IsConnected
        {
            get
            {
                return mWebSocketConnection.IsAvailable;
            }
        }

        public event EventHandler ConnectionOpen;
        public event EventHandler<WampMessageArrivedEventArgs<TMessage>> MessageArrived;
        public event EventHandler ConnectionClosed;
        public event EventHandler<WampConnectionErrorEventArgs> ConnectionError;
    }
}