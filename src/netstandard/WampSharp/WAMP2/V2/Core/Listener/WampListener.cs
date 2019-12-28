using WampSharp.Core.Dispatch;
using WampSharp.Core.Listener;
using WampSharp.Logging;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm.Binded;

namespace WampSharp.V2.Core.Listener
{
    /// <summary>
    /// A <see cref="WampListener{TMessage}"/> that is
    /// WAMPv2 specific.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampListener<TMessage> : WampListener<TMessage, IWampClientProxy<TMessage>>
    {
        private readonly IWampSessionServer<TMessage> mSessionHandler;

        /// <summary>
        /// Creates a new instance of <see cref="WampListener{TMessage}"/>
        /// </summary>
        /// <param name="listener">The <see cref="IWampConnectionListener{TMessage}"/> used in order to 
        ///     accept incoming connections.</param>
        /// <param name="handler">The <see cref="IWampIncomingMessageHandler{TMessage}"/> used
        ///     in order to dispatch incoming messages.</param>
        /// <param name="clientContainer">The <see cref="IWampClientContainer{TMessage,TClient}"/> use
        ///     in order to store the connected clients.</param>
        /// <param name="sessionHandler">A session handler that handles new clients.</param>
        public WampListener(IWampConnectionListener<TMessage> listener,
                            IWampIncomingMessageHandler<TMessage, IWampClientProxy<TMessage>> handler,
                            IWampClientContainer<TMessage, IWampClientProxy<TMessage>> clientContainer,
                            IWampSessionServer<TMessage> sessionHandler)
            : base(listener, handler, clientContainer)
        {
            mSessionHandler = sessionHandler;
        }

        protected override object GetSessionId(IWampClientProxy<TMessage> client)
        {
            return client.Session;
        }

        protected override void OnNewConnection(IWampConnection<TMessage> connection)
        {
            base.OnNewConnection(connection);

            IWampClientProxy<TMessage> client = ClientContainer.GetClient(connection);

            mLogger.DebugFormat("Client connected, session id: {SessionId}", client.Session);

            mSessionHandler.OnNewClient(client);
        }

        public override void Stop()
        {
            base.Stop();

            GoodbyeDetails details = new GoodbyeDetails();

            foreach (IWampClientProxy<TMessage> clientProxy in this.ClientContainer.GetAllClients())
            {
                clientProxy.SendGoodbye(details, WampErrors.SystemShutdown);
            }
        }

        protected override void OnCloseConnection(IWampConnection<TMessage> connection)
        {

            if (ClientContainer.TryGetClient(connection, out IWampClientProxy<TMessage> client))
            {
                mLogger.DebugFormat("Client disconnected, session id: {SessionId}", client.Session);

                mSessionHandler.OnClientDisconnect(client);
            }

            base.OnCloseConnection(connection);
        }
    }
}