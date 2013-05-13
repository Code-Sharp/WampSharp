using System.Collections.Generic;

namespace WampSharp.Core.Listener
{
    /// <summary>
    /// An implementation of <see cref="IWampClientContainer{TMessage,TClient}"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <typeparam name="TClient"></typeparam>
    public class WampClientContainer<TMessage, TClient> : IWampClientContainer<TMessage, TClient>
    {
        #region Members

        private readonly IWampClientBuilder<TMessage, TClient> mClientBuilder;

        private readonly IDictionary<IWampConnection<TMessage>, TClient> mConnectionToClient =
            new Dictionary<IWampConnection<TMessage>, TClient>();

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="WampClientContainer{TMessage,TClient}"/>
        /// using a client builder that is activated using the given <
        /// see cref="clientBuilderFactory"/> and this container.
        /// </summary>
        /// <param name="clientBuilderFactory">The given <see cref="IWampClientBuilderFactory{TMessage,TClient}"/>.</param>
        public WampClientContainer(IWampClientBuilderFactory<TMessage, TClient> clientBuilderFactory)
        {
            mClientBuilder = clientBuilderFactory.GetClientBuilder(this);
        }

        #endregion

        #region Public Methods

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

        #endregion
    }
}