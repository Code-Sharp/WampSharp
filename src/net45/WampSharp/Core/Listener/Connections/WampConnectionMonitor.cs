using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

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
        private bool mConnected;

        private readonly IEventPatternSource<EventArgs> mDisconnectionEventSource;
        private readonly ISubject<EventPattern<EventArgs>> mDisconnectionSubject;

        public WampConnectionMonitor(IWampConnection<TMessage> connection)
        {
            mConnected = true;
            mConnection = connection;
            mDisconnectionSubject = new ReplaySubject<EventPattern<EventArgs>>(1);

            mDisconnectionEventSource = mDisconnectionSubject.ToEventPattern();
            mConnection.ConnectionError += OnConnectionError;
            mConnection.ConnectionClosed += OnConnectionClosed;
        }

        public object Client { private get; set; }

        public bool Connected
        {
            get
            {
                return mConnected;
            }
        }

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
            mConnected = false;
            mConnection.ConnectionError -= OnConnectionError;
            mConnection.ConnectionClosed -= OnConnectionClosed;
            RaiseConnectionClosed(Client, EventArgs.Empty);
        }

        private void RaiseConnectionClosed(object client, EventArgs empty)
        {
            mDisconnectionSubject.OnNext(new EventPattern<EventArgs>(client, empty));
        }

        public event EventHandler ConnectionClosed
        {
            add
            {
                mDisconnectionEventSource.OnNext += new EventHandler<EventArgs>(value);
            }
            remove
            {
                mDisconnectionEventSource.OnNext -= new EventHandler<EventArgs>(value);
            }
        }
    }
}