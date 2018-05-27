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

        public WampConnectionMonitor(IWampConnection<TMessage> connection)
        {
            Connected = true;
            mConnection = connection;

            mConnection.ConnectionError += OnConnectionError;
            mConnection.ConnectionClosed += OnConnectionClosed;
        }

        public object Client { private get; set; }

        public bool Connected { get; private set; }

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
            Connected = false;
            mConnection.ConnectionError -= OnConnectionError;
            mConnection.ConnectionClosed -= OnConnectionClosed;
            RaiseConnectionClosed(Client, EventArgs.Empty);
        }

        private void RaiseConnectionClosed(object client, EventArgs empty)
        {
            ConnectionClosed?.Invoke(client, empty);
        }

        public event EventHandler ConnectionClosed;
    }
}