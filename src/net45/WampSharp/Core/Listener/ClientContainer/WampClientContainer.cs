using System.Collections.Concurrent;
using System.Collections.Generic;

namespace WampSharp.Core.Listener
{
    /// <summary>
    /// An implementation of <see cref="IWampClientContainer{TMessage,TClient}"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <typeparam name="TClient"></typeparam>
    public abstract class WampClientContainer<TMessage, TClient> : IWampClientContainer<TMessage, TClient>
    {
        #region Members

        private readonly IWampClientBuilder<TMessage, TClient> mClientBuilder;

        private readonly ConcurrentDictionary<IWampConnection<TMessage>, TClient> mConnectionToClient =
            new ConcurrentDictionary<IWampConnection<TMessage>, TClient>();

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="WampClientContainer{TMessage,TClient}"/>
        /// using a client builder that is activated using the given 
        /// <paramref name="clientBuilderFactory"/> and this container.
        /// </summary>
        /// <param name="clientBuilderFactory">The given <see cref="IWampClientBuilderFactory{TMessage,TClient}"/>.</param>
        public WampClientContainer(IWampClientBuilderFactory<TMessage, TClient> clientBuilderFactory)
        {
            mClientBuilder = clientBuilderFactory.GetClientBuilder(this);
        }

        #endregion

        #region Public Methods

        public virtual TClient GetClient(IWampConnection<TMessage> connection)
        {
            return mConnectionToClient.GetOrAdd(connection,
                                                key => mClientBuilder.Create(key));
        }

        public virtual IEnumerable<TClient> GetAllClients()
        {
            return mConnectionToClient.Values;
        }

        public virtual void RemoveClient(IWampConnection<TMessage> connection)
        {

            mConnectionToClient.TryRemove(connection, out TClient client);
        }

        public virtual bool TryGetClient(IWampConnection<TMessage> connection, out TClient client)
        {
            return mConnectionToClient.TryGetValue(connection, out client);
        }

        public abstract object GenerateClientId(TClient client);

        #endregion
    }
}