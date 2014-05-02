using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WampSharp.Core.Listener;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Core.Listener;

namespace WampSharp.V2.Client
{
    public class WampChannel<TMessage>
    {
        private readonly IControlledWampConnection<TMessage> mConnection;
        private readonly WampClient<TMessage> mClient;
        private readonly IWampServerProxy mServer;
        private int mConnectCalled;

        public WampChannel(IControlledWampConnection<TMessage> connection,
                           WampClient<TMessage> client)
        {
            mConnection = connection;
            mConnection.ConnectionOpen += OnConnectionOpen;
            mConnection.ConnectionClosed += OnConnectionClosed;
            mClient = client;
            mServer = client.Realm.Proxy;
        }

        private void OnConnectionOpen(object sender, EventArgs e)
        {
            mClient.OnConnectionOpen();
        }

        private void OnConnectionClosed(object sender, EventArgs e)
        {
            mClient.OnConnectionClosed();
        }

        public IWampServerProxy Server
        {
            get
            {
                return mServer;
            }
        }

        public IWampRealmProxy RealmProxy
        {
            get
            {
                return mClient.Realm;
            }
        }

        public Task Open()
        {
            if (Interlocked.CompareExchange(ref mConnectCalled, 1, 0) != 0)
            {
                // Throw something that says that "Open was already called."
                throw new ArgumentException();
            }
            else
            {
                mConnection.Connect();
                return mClient.OpenTask;
            }
        }
    }
}