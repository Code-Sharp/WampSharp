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
        private IDisposable mSubscription;

        public WampConnectionMonitor(IWampConnection<TMessage> connection)
        {
            mConnection = connection;
        }

        public object Client { private get; set; }

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
                if (mSubscription == null)
                {
                    mSubscription =
                        mConnection.Subscribe(x => { }, OnConnectionClosed);
                }

                mConnectionClosed += value;
            }
            remove
            {
                mConnectionClosed -= value;
            }
        }
    }
}