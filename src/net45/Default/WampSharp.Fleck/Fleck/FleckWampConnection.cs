using System;
using Fleck;
using WampSharp.Core.Listener;
using WampSharp.V2.Authentication;
using WampSharp.V2.MetaApi;
using WampSharp.V2.Transports;

namespace WampSharp.Fleck
{
    internal abstract class FleckWampConnection<TMessage> : AsyncWebSocketWampConnection<TMessage>,
        IDetailedWampConnection<TMessage>
    {
        protected IWebSocketConnection mWebSocketConnection;
        private readonly PingPongHandler mPingPongHandler;
        private readonly FleckTransportDetails mTransportDetails;

        public FleckWampConnection(IWebSocketConnection webSocketConnection,
                                   ICookieAuthenticatorFactory cookieAuthenticatorFactory,
                                   TimeSpan? autoSendPingInterval = null) :
                                       base(new FleckCookieProvider(webSocketConnection.ConnectionInfo),
                                            cookieAuthenticatorFactory)
        {
            mWebSocketConnection = webSocketConnection;

            mPingPongHandler =
                new PingPongHandler(mLogger,
                                    new FleckPinger(webSocketConnection),
                                    autoSendPingInterval ?? TimeSpan.FromSeconds(45));

            mWebSocketConnection.OnOpen = OnConnectionOpen;
            mWebSocketConnection.OnError = OnConnectionError;
            mWebSocketConnection.OnClose = OnConnectionClose;
            mTransportDetails = new FleckTransportDetails(mWebSocketConnection.ConnectionInfo);
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

        protected override void Dispose()
        {
            if (IsConnected)
            {
                mWebSocketConnection.Close();                
            }
        }

        protected override bool IsConnected => mWebSocketConnection.IsAvailable;

        public WampTransportDetails TransportDetails => mTransportDetails;
    }
}