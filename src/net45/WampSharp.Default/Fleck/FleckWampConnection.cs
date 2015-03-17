using System;
using Fleck;
using WampSharp.Core.Listener;
using WampSharp.V2.Reflection;

namespace WampSharp.Fleck
{
    internal abstract class FleckWampConnection<TMessage> : AsyncWampConnection<TMessage>,
        IDetailedWampConnection<TMessage>
    {
        protected IWebSocketConnection mWebSocketConnection;
        private readonly FleckTransportDetails mTransportDetails;

        public FleckWampConnection(IWebSocketConnection webSocketConnection)
        {
            mWebSocketConnection = webSocketConnection;
            mWebSocketConnection.OnOpen = OnConnectionOpen;
            mWebSocketConnection.OnError = OnConnectionError;
            mWebSocketConnection.OnClose = OnConnectionClose;
            mTransportDetails = new FleckTransportDetails(mWebSocketConnection.ConnectionInfo);
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

        public WampTransportDetails TransportDetails
        {
            get
            {
                return mTransportDetails;
            }
        }
    }
}