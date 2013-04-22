using System;

namespace WampSharp.Core.Listener
{
    public class WampClientContainerDisposable<TConnection> : IDisposable
    {
        private readonly IWampClientContainer<TConnection> mContainer;
        private readonly TConnection mConnection;

        public WampClientContainerDisposable(IWampClientContainer<TConnection> container, TConnection connection)
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