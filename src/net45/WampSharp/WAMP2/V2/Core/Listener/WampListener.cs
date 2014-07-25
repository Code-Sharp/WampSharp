using WampSharp.Core.Dispatch;
using WampSharp.Core.Listener;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Core.Listener
{
    /// <summary>
    /// A <see cref="WampListener{TMessage}"/> that is
    /// WAMPv2 specific.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampListener<TMessage> : WampListener<TMessage, IWampClient<TMessage>>
    {
        private readonly IWampSessionServer<TMessage> mSessionHandler;

        /// <summary>
        /// Creates a new instance of <see cref="WampListener{TMessage}"/>
        /// </summary>
        /// <param name="listener">The <see cref="IWampConnectionListener{TMessage}"/> used in order to 
        /// accept incoming connections.</param>
        /// <param name="handler">The <see cref="IWampIncomingMessageHandler{TMessage}"/> used
        /// in order to dispatch incoming messages.</param>
        /// <param name="clientContainer">The <see cref="IWampClientContainer{TMessage,TClient}"/> use
        /// in order to store the connected clients.</param>
        /// <param name="sessionHandler">A session handler that handles new clients.</param>
        public WampListener(IWampConnectionListener<TMessage> listener,
                            IWampIncomingMessageHandler<TMessage, IWampClient<TMessage>> handler,
                            IWampClientContainer<TMessage, IWampClient<TMessage>> clientContainer,
                            IWampSessionServer<TMessage> sessionHandler)
            : base(listener, handler, clientContainer)
        {
            mSessionHandler = sessionHandler;
        }

        protected override void OnNewConnection(IWampConnection<TMessage> connection)
        {
            base.OnNewConnection(connection);

            IWampClient<TMessage> client = ClientContainer.GetClient(connection);

            mSessionHandler.OnNewClient(client);
        }

        protected override void OnCloseConnection(IWampConnection<TMessage> connection)
        {
            IWampClient<TMessage> client;

            if (ClientContainer.TryGetClient(connection, out client))
            {
                mSessionHandler.OnClientDisconnect(client);
            }

            base.OnCloseConnection(connection);
        }
    }
}