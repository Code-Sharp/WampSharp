using WampSharp.Core.Listener;
using WampSharp.V2.Core;
using WampSharp.V2.Realm;

namespace WampSharp.V2.Binding
{
    /// <summary>
    /// Represents a router-side binding.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampRouterBinding<TMessage> : IWampBinding<TMessage>
    {
        /// <summary>
        /// Creates a <see cref="IWampBindingHost"/> hosting this binding.
        /// </summary>
        /// <param name="realmContainer"></param>
        /// <param name="connectionListener"></param>
        /// <param name="sessionIdMap"></param>
        /// <returns></returns>
        IWampBindingHost CreateHost(IWampHostedRealmContainer realmContainer,
                                    IWampConnectionListener<TMessage> connectionListener,
                                    IWampSessionMapper sessionIdMap);
    }
}