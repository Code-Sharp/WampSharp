using System;

namespace WampSharp.Core.Listener
{
    public class WampClientContainerDisposable<TMessage> : IDisposable
    {
        private readonly IWampClientContainer<TMessage> mContainer;
        private readonly IWampConnection<TMessage> mConnection;

        public WampClientContainerDisposable(IWampClientContainer<TMessage> container, IWampConnection<TMessage> connection)
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