using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm;

namespace WampSharp.V2.Session
{
    internal class WampSessionServer<TMessage> : IWampSessionServer<TMessage> where TMessage : class
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
            client.Welcome(wampClient.Session, details);
        }

        public void Authenticate(IWampSessionClient client, string signature, TMessage extra)
        {
        }

        public void Welcome(IWampSessionClient client, long session, TMessage details)
        {
        }

        public void Goodbye(IWampSessionClient client, string reason, TMessage details)
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