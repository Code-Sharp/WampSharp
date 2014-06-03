using System;
using WampSharp.Core.Client;
using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;

namespace WampSharp.Auxiliary.Client
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
            connection.Subscribe(x => OnNext(x), x => OnError(x), OnConnectionLost);
        }

        #endregion

        #region Private Methods

        private void OnConnectionLost()
        {
            RaiseConnectionLost();

            // TODO: Reconnection logic.
        }

        private void OnNext(WampMessage<TMessage> wampMessage)
        {
            // Not sure what to do.
        }

        private void OnError(Exception exception)
        {
            RaiseConnectionError(exception);
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
            EventHandler<WampConnectionEstablishedEventArgs> connectionEstablished = ConnectionEstablished;

            if (connectionEstablished != null)
            {
                connectionEstablished(this,
                                      new WampConnectionEstablishedEventArgs(sessionId, serverIdent));
            }
        }

        private void RaiseConnectionLost()
        {
            EventHandler connectionLost = ConnectionLost;

            if (connectionLost != null)
            {
                connectionLost(this, EventArgs.Empty);
            }
        }


        private void RaiseConnectionError(Exception exception)
        {
            EventHandler<WampConnectionErrorEventArgs> connectionError = ConnectionError;

            if (connectionError != null)
            {
                connectionError(this, new WampConnectionErrorEventArgs(exception));
            }
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

            public string SessionId
            {
                get
                {
                    return mParent.SessionId;
                }
            }
        }

        #endregion
    }
}