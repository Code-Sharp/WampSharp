using WampSharp.Core.Listener;
using WampSharp.V1.Core.Contracts;

namespace WampSharp.V1.PubSub.Client
{
    /// <summary>
    /// Creates a <see cref="IWampServer"/> proxy that handles
    /// <see cref="IWampPubSubClient{TMessage}"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampServerProxyFactory<TMessage>
    {
        /// <summary>
        /// Creates a <see cref="IWampServer"/> that its callbacks will be
        /// redirecte to a given <see cref="IWampPubSubClient{TMessage}"/>.
        /// </summary>
        /// <param name="client">The given <see cref="IWampPubSubClient{TMessage}"/>.</param>
        /// <param name="connection">The connection to the proxy.</param>
        /// <returns>The created proxy.</returns>
        IWampServer Create(IWampPubSubClient<TMessage> client, IWampConnection<TMessage> connection);
    }
}