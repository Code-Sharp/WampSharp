using System.Collections.Generic;
using WampSharp.Core.Contracts;

namespace WampSharp.Core.Listener
{
    public class WampClientContainer<TConnection> : IWampClientContainer<TConnection>
    {
        private readonly IWampClientBuilder<TConnection> mClientBuilder;

        private readonly IDictionary<TConnection, IWampClient> mConnectionToClient = 
            new Dictionary<TConnection, IWampClient>();

        public WampClientContainer(IWampClientBuilder<TConnection> clientBuilder)
        {
            mClientBuilder = clientBuilder;
        }

        public IWampClient GetClient(TConnection connection)
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

        public void RemoveClient(TConnection connection)
        {
            mConnectionToClient.Remove(connection);
        }
    }
}