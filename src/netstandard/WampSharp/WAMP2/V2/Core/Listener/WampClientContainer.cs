using WampSharp.Core.Listener;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Core.Listener
{
    public class WampClientContainer<TMessage> : WampClientContainer<TMessage, IWampClientProxy<TMessage>>
    {
        private IWampSessionMapper mSessionIdMap;

        public WampClientContainer(IWampClientBuilderFactory<TMessage, IWampClientProxy<TMessage>> clientBuilderFactory) :
            this(clientBuilderFactory, new WampSessionMapper())
        {
        }

        public WampClientContainer(IWampClientBuilderFactory<TMessage, IWampClientProxy<TMessage>> clientBuilderFactory, IWampSessionMapper sessionIdMap) :
            base(clientBuilderFactory)
        {
            mSessionIdMap = sessionIdMap;
        }

        public override void RemoveClient(IWampConnection<TMessage> connection)
        {
            bool clientProxyFound = TryGetClient(connection, out IWampClientProxy<TMessage> clientProxy);

            base.RemoveClient(connection);

            if (clientProxyFound)
            {
                mSessionIdMap.ReleaseSession(clientProxy.Session);
            }
        }

        public override object GenerateClientId(IWampClientProxy<TMessage> client)
        {
            return mSessionIdMap.AllocateSession(client);
        }
    }
}