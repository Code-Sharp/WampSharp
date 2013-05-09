using System.Collections.Generic;

namespace WampSharp.Core.Listener
{
    public class WampClientContainer<TMessage, TClient> : IWampClientContainer<TMessage, TClient>
    {
        private readonly IWampClientBuilder<TMessage, TClient> mClientBuilder;

        private readonly IDictionary<IWampConnection<TMessage>, TClient> mConnectionToClient =
            new Dictionary<IWampConnection<TMessage>, TClient>();

        public WampClientContainer(IWampClientBuilderFactory<TMessage, TClient> clientBuilder)
        {
            mClientBuilder = clientBuilder.GetClientBuilder(this);
        }

        public TClient GetClient(IWampConnection<TMessage> connection)
        {
            TClient client;

            if (mConnectionToClient.TryGetValue(connection, out client))
            {
                return client;
            }
            else
            {
                client = mClientBuilder.Create(connection);
                mConnectionToClient[connection] = client;
                return client;
            }
        }

        public IEnumerable<TClient> GetAllClients()
        {
            return mConnectionToClient.Values;
        }

        public void RemoveClient(IWampConnection<TMessage> connection)
        {
            mConnectionToClient.Remove(connection);
        }
    }
}