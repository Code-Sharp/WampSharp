using WampSharp.Core.Listener;

namespace WampSharp.V2.Authentication
{
    /// <summary>
    /// Represents a WAMP connection that a built-in authentication mechanism.
    /// (For example WebSockets with cookies)
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IWampAuthenticatedConnection<TMessage> : IWampConnection<TMessage>
    {
        /// <summary>
        /// Gets the authenticator associated with this connection.
        /// </summary>
        IWampSessionAuthenticator Authenticator { get; }
    }
}