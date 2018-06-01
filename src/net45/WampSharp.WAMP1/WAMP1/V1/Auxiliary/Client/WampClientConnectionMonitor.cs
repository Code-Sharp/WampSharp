using System;
using WampSharp.Core.Client;
using WampSharp.Core.Listener;
using WampSharp.V1.Core.Contracts;

namespace WampSharp.V1.Auxiliary.Client
{
    public class WampClientConnectionMonitor<TMessage> : IWampClientConnectionMonitor
    {
        #region Fields

        private readonly IWampAuxiliaryServer mProxy;

        #endregion

        #region Constructor

        public WampClientConnectionMonitor
            (IWampServerProxyBuilder<TMessage, IWampAuxiliaryClient, IWampServer> serverProxyBuilder,
             IWampConnection<TMessage> connection)
        {
            mProxy = serverProxyBuilder.Create(new WampAuxiliaryClient(this), connection);
            connection.ConnectionClosed += OnConnectionLost;
            connection.ConnectionError += OnConnectionError;
        }

        #endregion

        #region Private Methods

        private void OnConnectionLost(object sender, EventArgs e)
        {
            RaiseConnectionLost();

            // TODO: Reconnection logic.
        }

        private void OnConnectionError(object sender, WampConnectionErrorEventArgs e)
        {
            RaiseConnectionError(e.Exception);
        }

        private void OnWelcome(string sessionId, int protocolVersion, string serverIdent)
        {
            this.SessionId = sessionId;

            VerifyProtoclVersion(protocolVersion);

            RaiseConnectionEstablished(sessionId, serverIdent);
        }

        private void VerifyProtoclVersion(int protocolVersion)
        {
            if (protocolVersion != 1)
            {
                // TODO :)
                throw new Exception();
            }
        }

        private void RaiseConnectionEstablished(string sessionId, string serverIdent)
        {
            ConnectionEstablished?.Invoke(this,
                                      new WampConnectionEstablishedEventArgs(sessionId, serverIdent));
        }

        private void RaiseConnectionLost()
        {
            ConnectionLost?.Invoke(this, EventArgs.Empty);
        }


        private void RaiseConnectionError(Exception exception)
        {
            ConnectionError?.Invoke(this, new WampConnectionErrorEventArgs(exception));
        }

        public void MapPrefix(string prefix, string uri)
        {
            mProxy.Prefix(null, prefix, uri);
        }

        #endregion

        #region Properties

        public string SessionId
        {
            get; 
            private set;
        }

        #endregion

        #region Events

        public event EventHandler<WampConnectionEstablishedEventArgs> ConnectionEstablished;

        public event EventHandler ConnectionLost;

        public event EventHandler<WampConnectionErrorEventArgs> ConnectionError;
        
        #endregion

        #region Nested Classes

        private class WampAuxiliaryClient : IWampAuxiliaryClient
        {
            private readonly WampClientConnectionMonitor<TMessage> mParent;

            public WampAuxiliaryClient(WampClientConnectionMonitor<TMessage> parent)
            {
                mParent = parent;
            }

            public void Welcome(string sessionId, int protocolVersion, string serverIdent)
            {
                mParent.OnWelcome(sessionId, protocolVersion, serverIdent);
            }

            public string SessionId => mParent.SessionId;
        }

        #endregion
    }
}