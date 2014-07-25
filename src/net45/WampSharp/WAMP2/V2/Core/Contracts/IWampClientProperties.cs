using WampSharp.V2.Binding;
using WampSharp.V2.Realm;
using WampSharp.V2.Realm.Binded;

namespace WampSharp.V2.Core.Contracts
{
    /// <summary>
    /// Contains additional properties of a <see cref="IWampClient"/> proxy.
    /// </summary>
    public interface IWampClientProperties
    {
        /// <summary>
        /// Gets the session of the current client. 
        /// </summary>
        long Session { get; }

        // TODO: Maybe get rid of the binding property, nobody needs it
        /// <summary>
        /// Gets the binding associated with the current client.
        /// </summary>
        IWampBinding Binding { get; }
    }

    /// <summary>
    /// Contains additional properties of a <see cref="IWampClient{TMessage}"/> proxy.
    /// </summary>
    public interface IWampClientProperties<TMessage>
    {
        /// <summary>
        /// Gets or sets a value indicating whether the connection was closed in an orderly manner
        /// </summary>
        bool OrderlyDisengagement { get; set; }

        /// <summary>
        /// Gets the session of the current client. 
        /// </summary>
        long Session { get; }

        /// <summary>
        /// Gets the realm the current client belongs to.
        /// </summary>
        IWampBindedRealm<TMessage> Realm { get; set; }

        // TODO: Maybe get rid of the binding property, nobody needs it
        /// <summary>
        /// Gets the binding associated with the current client.
        /// </summary>
        new IWampBinding<TMessage> Binding { get; }
    }
}