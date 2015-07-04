using WampSharp.Core.Listener;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Core.Listener
{
    public class WampClientContainer<TMessage> : WampClientContainer<TMessage, IWampClientProxy<TMessage>>
    {
        private readonly WampIdMapper<IWampClientProxy<TMessage>> mSessionIdToClientProxy = new WampIdMapper<IWampClientProxy<TMessage>>();

        public WampClientContainer(IWampClientBuilderFactory<TMessage, IWampClientProxy<TMessage>> clientBuilderFactory) :
            base(clientBuilderFactory)
        {
        }

        public override void RemoveClient(IWampConnection<TMessage> connection)
        {
            IWampClientProxy<TMessage> clientProxy;

            bool clientProxyFound = TryGetClient(connection, out clientProxy);

            base.RemoveClient(connection);

            if (clientProxyFound)
            {
                mSessionIdToClientProxy.TryRemove(clientProxy.Session, out clientProxy);
            }
        }

        public override object GenerateClientId(IWampClientProxy<TMessage> client)
        {
            return mSessionIdToClientProxy.Add(client);
        }
    }
}