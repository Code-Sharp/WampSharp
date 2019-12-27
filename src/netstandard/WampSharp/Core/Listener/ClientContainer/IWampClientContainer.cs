using System.Collections.Generic;

namespace WampSharp.Core.Listener
{
    /// <summary>
    /// A container of client proxies.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <typeparam name="TClient"></typeparam>
    public interface IWampClientContainer<TMessage, TClient>
    {
        /// <summary>
        /// Gets a client proxy for the given <paramref name="connection"/>.
        /// </summary>
        /// <param name="connection">The given connection.</param>
        /// <returns>A client proxy.</returns>
        TClient GetClient(IWampConnection<TMessage> connection);

        /// <summary>
        /// Returns all clients currently registered in the container.
        /// </summary>
        /// <returns>An enumerable of clients currently present in the container.</returns>
        IEnumerable<TClient> GetAllClients(); 

        /// <summary>
        /// Removes a client from the container given its connection.
        /// </summary>
        /// <param name="connection">The given client's connection.</param>
        void RemoveClient(IWampConnection<TMessage> connection);

        /// <summary>
        /// Gets a client proxy for the given <paramref name="connection"/> if it
        /// is present.
        /// </summary>
        /// <param name="connection">The given connection.</param>
        /// <param name="client">The requested client.</param>
        /// <returns>A value indicating whether the client was resolved successfully.</returns>
        bool TryGetClient(IWampConnection<TMessage> connection, out TClient client);

        /// <summary>
        /// Generates a client id.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        object GenerateClientId(TClient client);
    }
}