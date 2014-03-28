using System.Collections.Generic;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm;

namespace WampSharp.V2.Session
{
    internal class WampSessionServer<TMessage> : IWampSessionServer<TMessage>
    {
        private IWampRealmContainer<TMessage> mRealmContainer;

        public void OnNewClient(IWampClient<TMessage> client)
        {
        }

        public void Hello(IWampSessionClient client, string realm, TMessage details)
        {
            IWampClient<TMessage> wampClient = client as IWampClient<TMessage>;
            wampClient.Realm = mRealmContainer.GetRealmByName(realm);
            
            // TODO: Send real details to the client.
            client.Welcome(wampClient.Session, new Dictionary<string,object>()
                                                   {
                                                       {"roles",
                                                   new Dictionary<string,object>()
                                                       {
                                                           {"dealer", new Dictionary<string,object>()},
                                                           {"broker", new Dictionary<string,object>()},
                                                       }}
                                                   });
        }

        public void Abort(IWampSessionClient client, TMessage details, string reason)
        {
        }

        public void Authenticate(IWampSessionClient client, string signature, TMessage extra)
        {
        }

        public void Goodbye(IWampSessionClient client, TMessage details, string reason)
        {
        }

        public void Heartbeat(IWampSessionClient client, int incomingSeq, int outgoingSeq)
        {
        }

        public void Heartbeat(IWampSessionClient client, int incomingSeq, int outgoingSeq, string discard)
        {
        }

        public IWampRealmContainer<TMessage> RealmContainer
        {
            get
            {
                return mRealmContainer;
            }
            set
            {
                mRealmContainer = value;
            }
        }
    }
}