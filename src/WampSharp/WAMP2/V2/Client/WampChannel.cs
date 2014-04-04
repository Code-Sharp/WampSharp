using System.Collections.Generic;
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
        private readonly IWampRealmProxy mRealmProxy;

        public WampChannel(IControlledWampConnection<TMessage> connection,
                           IWampBinding<TMessage> binding,
                           WampClient<TMessage> client,
                           IWampServerProxy server, string realm)
        {
            mConnection = connection;
            mConnection.ConnectionOpen += mConnection_ConnectionOpen;
            mClient = client;
            mServer = server;
            mRealmProxy = new WampRealmProxy<TMessage>(realm, server, binding);
            mClient.Realm = mRealmProxy;
        }

        private void mConnection_ConnectionOpen(object sender, System.EventArgs e)
        {
            mServer.Hello(RealmProxy.Name,
                          new Dictionary<string, object>()
                              {
                                  {
                                      "roles",
                                      new Dictionary<string, object>()
                                          {
                                              {"caller", new Dictionary<string, object>()},
                                              {"callee", new Dictionary<string, object>()},
                                              {"publisher", new Dictionary<string, object>()},
                                              {"subscriber", new Dictionary<string, object>()},
                                          }
                                  }
                              });
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
            get { return mRealmProxy; }
        }

        public void Open()
        {
            mConnection.Connect();
        }
    }
}