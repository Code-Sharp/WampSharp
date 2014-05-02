using System;

namespace WampSharp.Core.Listener
{
    /// <summary>
    /// An <see cref="IDisposable"/> that is used as a mixin for implementing
    /// <see cref="IDisposable"/> for generated proxies.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <typeparam name="TClient"></typeparam>
    public class WampClientContainerDisposable<TMessage, TClient> : IDisposable
    {
        private readonly IWampClientContainer<TMessage, TClient> mContainer;
        private readonly IWampConnection<TMessage> mConnection;

        /// <summary>
        /// Initializes a new instance of <see cref="WampClientContainerDisposable{TMessage,TClient}"/>.
        /// </summary>
        /// <param name="container">The container that contains the client proxy.</param>
        /// <param name="connection">The connection of the client.</param>
        public WampClientContainerDisposable(IWampClientContainer<TMessage, TClient> container, IWampConnection<TMessage> connection)
        {
            mContainer = container;
            mConnection = connection;
        }

        public void Dispose()
        {
            mConnection.Dispose();
            mContainer.RemoveClient(mConnection);
        }
    }
}