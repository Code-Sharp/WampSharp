using System;
using System.Threading;
using System.Threading.Tasks;
using WampSharp.Core.Listener;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Client
{
    internal class WampChannel<TMessage> : IWampChannel
    {
        private IControlledWampConnection<TMessage> mConnection;
        private readonly WampClient<TMessage> mClient;
        private int mConnectCalled;
        private readonly object mLock = new object();

        public WampChannel(IControlledWampConnection<TMessage> connection,
                           WampClient<TMessage> client)
        {
            mConnection = connection;
            mConnection.ConnectionOpen += OnConnectionOpen;
            mConnection.ConnectionError += OnConnectionError;
            mConnection.ConnectionClosed += OnConnectionClosed;
            mClient = client;
            Server = client.Realm.Proxy;
        }

        private void OnConnectionOpen(object sender, EventArgs e)
        {
            mClient.OnConnectionOpen();
        }

        private void OnConnectionClosed(object sender, EventArgs e)
        {
            Interlocked.Exchange(ref mConnectCalled, 0);
            mClient.OnConnectionClosed();
        }

        private void OnConnectionError(object sender, WampConnectionErrorEventArgs e)
        {
            mClient.OnConnectionError(e.Exception);
        }

        public IWampServerProxy Server { get; }

        public IWampRealmProxy RealmProxy => mClient.Realm;

        public Task Open()
        {
            if (Interlocked.CompareExchange(ref mConnectCalled, 1, 0) != 0)
            {
                // Throw something that says that "Open was already called."
                throw new ArgumentException();
            }
            else
            {
                Task openTask = mClient.OpenTask;
                mConnection.Connect();
                return openTask;
            }
        }

        public void Close()
        {
            lock (mLock)
            {
                if (mConnection != null)
                {
                    mConnection.Dispose();
                    mConnection = null;
                }                
            }
        }

        public Task<GoodbyeMessage> Close(string reason, GoodbyeDetails details)
        {
            return mClient.Close(reason, details);
        }
    }
}