using WampSharp.Core.Dispatch;
using WampSharp.Core.Listener;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Core.Listener
{
    /// <summary>
    /// A <see cref="WampListener{TMessage}"/> that is
    /// WAMPv1 specific.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampListener<TMessage> : WampListener<TMessage, IWampClient>
    {
        /// <summary>
        /// Creates a new instance of <see cref="WampListener{TMessage}"/>
        /// </summary>
        /// <param name="listener">The <see cref="IWampConnectionListener{TMessage}"/> used in order to 
        /// accept incoming connections.</param>
        /// <param name="handler">The <see cref="IWampIncomingMessageHandler{TMessage}"/> used
        /// in order to dispatch incoming messages.</param>
        /// <param name="clientContainer">The <see cref="IWampClientContainer{TMessage,TClient}"/> use
        /// in order to store the connected clients.</param>
        public WampListener(IWampConnectionListener<TMessage> listener,
                            IWampIncomingMessageHandler<TMessage, IWampClient> handler,
                            IWampClientContainer<TMessage, IWampClient> clientContainer)
            : base(listener, handler, clientContainer)
        {
        }

        protected override void OnNewConnection(IWampConnection<TMessage> connection)
        {
            base.OnNewConnection(connection);

            IWampClient client = ClientContainer.GetClient(connection);

            client.Welcome(client.Session, null);
        }
    }
}