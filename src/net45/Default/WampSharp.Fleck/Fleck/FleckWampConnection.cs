using System;
using Fleck;
using WampSharp.Core.Listener;
using WampSharp.V2.Transports;

namespace WampSharp.Fleck
{
    internal abstract class FleckWampConnection<TMessage> : AsyncWampConnection<TMessage>
    {
        protected IWebSocketConnection mWebSocketConnection;
        private readonly PingPongHandler mPingPongHandler;

        public FleckWampConnection(IWebSocketConnection webSocketConnection, 
            TimeSpan? autoSendPingInterval = null)
        {
            mWebSocketConnection = webSocketConnection;

            mPingPongHandler =
                new PingPongHandler(mLogger,
                                    new FleckPinger(webSocketConnection),
                                    autoSendPingInterval ?? TimeSpan.FromSeconds(45));

            mWebSocketConnection.OnOpen = OnConnectionOpen;
            mWebSocketConnection.OnError = OnConnectionError;
            mWebSocketConnection.OnClose = OnConnectionClose;
        }

        private void OnConnectionOpen()
        {
            RaiseConnectionOpen();

            mPingPongHandler.Start();
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