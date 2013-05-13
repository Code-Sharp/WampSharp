namespace WampSharp.Core.Listener
{
    /// <summary>
    /// Builds a proxy to a WAMP client given a <see cref="IWampConnection{TMessage}"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <typeparam name="TClient"></typeparam>
    public interface IWampClientBuilder<TMessage, TClient>
    {
        /// <summary>
        /// Creates a proxy to a WAMP client by the given connection.
        /// </summary>
        /// <param name="connection">The connection to the client.</param>
        /// <returns>A proxy to the WAMP client.</returns>
        TClient Create(IWampConnection<TMessage> connection);
    }
}