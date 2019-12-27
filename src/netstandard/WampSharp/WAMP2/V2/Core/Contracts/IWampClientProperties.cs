using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;
using WampSharp.V2.MetaApi;
using WampSharp.V2.Realm.Binded;

namespace WampSharp.V2.Core.Contracts
{
    /// <summary>
    /// Contains additional properties of a <see cref="IWampClientProxy"/> proxy.
    /// </summary>
    public interface IWampClientProperties
    {
        /// <summary>
        /// Gets or sets a value indicating whether the connection was closed in an orderly manner
        /// </summary>
        bool GoodbyeSent { get; set; }

        /// <summary>
        /// Gets the session of the current client. 
        /// </summary>
        long Session { get; }

        // TODO: Maybe get rid of the binding property, nobody needs it
        /// <summary>
        /// Gets the binding associated with the current client.
        /// </summary>
        IWampBinding Binding { get; }

        IWampSessionAuthenticator Authenticator { get; set; }

        IWampAuthorizer Authorizer { get; }

        HelloDetails HelloDetails { get; set; }

        WelcomeDetails WelcomeDetails { get; set; }
    }

    /// <summary>
    /// Contains additional properties of a <see cref="IWampClientProxy{TMessage}"/> proxy.
    /// </summary>
    public interface IWampClientProperties<TMessage>
    {
        /// <summary>
        /// Gets the realm the current client belongs to.
        /// </summary>
        IWampBindedRealm<TMessage> Realm { get; set; }

        // TODO: Maybe get rid of the binding property, nobody needs it
        /// <summary>
        /// Gets the binding associated with the current client.
        /// </summary>
         IWampBinding<TMessage> Binding { get; }

        /// <summary>
        /// Gets the transport details associated with this client.
        /// </summary>
        WampTransportDetails TransportDetails { get; }
    }
}