using System.Collections.Generic;
using WampSharp.Core.Contracts;

namespace WampSharp.Core.Listener
{
    public class WampClientContainer<TMessage> : IWampClientContainer<TMessage>
    {
        private readonly IWampClientBuilder<TMessage> mClientBuilder;

        private readonly IDictionary<IWampConnection<TMessage>, IWampClient> mConnectionToClient =
            new Dictionary<IWampConnection<TMessage>, IWampClient>();

        public WampClientContainer(IWampClientBuilderFactory<TMessage> clientBuilder)
        {
            mClientBuilder = clientBuilder.GetClientBuilder(this);
        }

        public IWampClient GetClient(IWampConnection<TMessage> connection)
        {
            IWampClient client;

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

        public IEnumerable<IWampClient> GetAllClients()
        {
            return mConnectionToClient.Values;
        }

        public void RemoveClient(IWampConnection<TMessage> connection)
        {
            mConnectionToClient.Remove(connection);
        }
    }
}