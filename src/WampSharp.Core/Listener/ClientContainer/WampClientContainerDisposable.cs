using System;

namespace WampSharp.Core.Listener
{
    public class WampClientContainerDisposable<TMessage, TClient> : IDisposable
    {
        private readonly IWampClientContainer<TMessage, TClient> mContainer;
        private readonly IWampConnection<TMessage> mConnection;

        public WampClientContainerDisposable(IWampClientContainer<TMessage, TClient> container, IWampConnection<TMessage> connection)
        {
            mContainer = container;
            mConnection = connection;
        }

        public void Dispose()
        {
            mContainer.RemoveClient(mConnection);
        }
    }
}