using System;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Listener;
using WampSharp.Logging;
using WampSharp.V1.Core.Contracts;

namespace WampSharp.V1.Core.Listener
{
    /// <summary>
    /// A <see cref="WampListener{TMessage,TClient}"/> that is
    /// WAMPv1 specific.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampListener<TMessage> : WampListener<TMessage, IWampClient>
    {
        /// <summary>
        /// Occurs when a WAMP session is created.
        /// </summary>
    	public event EventHandler<WampSessionEventArgs> SessionCreated;

        /// <summary>
        /// Occurs when a WAMP session is closed.
        /// </summary>
        public event EventHandler<WampSessionEventArgs> SessionClosed;

        /// <summary>
        /// Creates a new instance of <see cref="WampListener{TMessage}"/>
        /// </summary>
        /// <param name="listener">The <see cref="IWampConnectionListener{TMessage}"/> used in order to 
        /// accept incoming connections.</param>
        /// <param name="handler">The <see cref="IWampIncomingMessageHandler{TMessage,TClient}"/> used
        /// in order to dispatch incoming messages.</param>
        /// <param name="clientContainer">The <see cref="IWampClientContainer{TMessage,TClient}"/> use
        /// in order to store the connected clients.</param>
        public WampListener(IWampConnectionListener<TMessage> listener,
            IWampIncomingMessageHandler<TMessage, IWampClient> handler,
            IWampClientContainer<TMessage, IWampClient> clientContainer)
            : base(listener, handler, clientContainer)
        {
        }

        protected override object GetSessionId(IWampClient client)
        {
            return client.SessionId;
        }

        protected override void OnConnectionOpen(IWampConnection<TMessage> connection)
        {
            base.OnConnectionOpen(connection);

            IWampClient client = ClientContainer.GetClient(connection);

            mLogger.DebugFormat("Client connected, session id: {SessionId}", client.SessionId);

            client.Welcome(client.SessionId, 1, "WampSharp");
            
            RaiseSessionCreated(client);
        }

        private void RaiseSessionCreated(IWampClient client)
        {
            SessionCreated?.Invoke(this, new WampSessionEventArgs(client.SessionId));
        }

        protected override void OnCloseConnection(IWampConnection<TMessage> connection)
        {
            RaiseSessionClosed(connection);

            if (mLogger.IsDebugEnabled())
            {
                IWampClient client = ClientContainer.GetClient(connection);
                mLogger.DebugFormat("Client disconnected, session id: {SessionId}", client.SessionId);
            }

            base.OnCloseConnection(connection);
        }

        private void RaiseSessionClosed(IWampConnection<TMessage> connection)
        {
            EventHandler<WampSessionEventArgs> sessionClosed = SessionClosed;

            if (sessionClosed != null)
            {
                IWampClient client = ClientContainer.GetClient(connection);
                sessionClosed(this, new WampSessionEventArgs(client.SessionId));
            }
        }
    }
}