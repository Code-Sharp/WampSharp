using System;

namespace WampSharp.Core.Listener
{
    /// <summary>
    /// An implementation of <see cref="IWampConnectionMonitor"/> used
    /// as a mixin in <see cref="IWampClientBuilder{TMessage,TClient}"/>
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    internal class WampConnectionMonitor<TMessage> : IWampConnectionMonitor
    {
        private readonly IWampConnection<TMessage> mConnection;
        private EventHandler mConnectionClosed;

        public WampConnectionMonitor(IWampConnection<TMessage> connection)
        {
            mConnection = connection;
            mConnection.ConnectionError += OnConnectionError;
            mConnection.ConnectionClosed += OnConnectionClosed;
        }

        public object Client { private get; set; }

        private void OnConnectionError(object sender, WampConnectionErrorEventArgs e)
        {
            OnConnectionClosed();
        }

        private void OnConnectionClosed(object sender, EventArgs e)
        {
            OnConnectionClosed();
        }

        private void OnConnectionClosed()
        {
            RaiseConnectionClosed(Client, EventArgs.Empty);
        }

        private void RaiseConnectionClosed(object client, EventArgs empty)
        {
            EventHandler connectionClosed = mConnectionClosed;

            if (connectionClosed != null)
            {
                connectionClosed(client, empty);
            }
        }

        public event EventHandler ConnectionClosed
        {
            add
            {
                mConnectionClosed += value;
            }
            remove
            {
                mConnectionClosed -= value;
            }
        }
    }
}