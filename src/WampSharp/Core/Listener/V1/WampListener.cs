using System;
using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Dispatch;

namespace WampSharp.Core.Listener.V1
{
    public class WampListener<TMessage> : WampListener<TMessage, IWampClient>
    {
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

            client.Welcome(client.SessionId, 1, "WampSharp");
        }

    }
}