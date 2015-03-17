using System;
using System.Collections.Generic;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm.Binded;

namespace WampSharp.V2.Session
{
    internal class WampSessionServer<TMessage> : IWampSessionServer<TMessage>
    {
        private IWampBindedRealmContainer<TMessage> mRealmContainer;
        private readonly Dictionary<string, object> mWelcomeDetails;

        public WampSessionServer()
        {
            // Do it with reflection and attributes :)
            mWelcomeDetails = new Dictionary<string,object>()
                {
                    {"roles",
                        new Dictionary<string,object>()
                            {
                                {"dealer", new Dictionary<string, object>()
                                    {
                                                                   
                                    }},
                                {"broker", new Dictionary<string,object>()
                                    {
                                        {"features",
                                            new Dictionary<string, object>()
                                                {
                                                    {"publisher_identification", true},
                                                    {"publisher_exclusion", true},
                                                    {"subscriber_blackwhite_listing", true},
                                                }}
                                    }},
                            }}
                };
        }

        public void OnNewClient(IWampClient<TMessage> client)
        {
        }

        public void OnClientDisconnect(IWampClient<TMessage> client)
        {
            if ((client.Realm != null) && !client.GoodbyeSent)
            {
                client.Realm.SessionLost(client.Session);
            }
        }

        public void Hello(IWampSessionClient client, string realm, TMessage details)
        {
            IWampClient<TMessage> wampClient = client as IWampClient<TMessage>;
            
            IWampBindedRealm<TMessage> bindedRealm = 
                mRealmContainer.GetRealmByName(realm);
            
            wampClient.Realm = bindedRealm;

            bindedRealm.Hello(wampClient.Session, wampClient.TransportDetails, details);

            client.Welcome(wampClient.Session, mWelcomeDetails);
        }

        public void Abort(IWampSessionClient client, TMessage details, string reason)
        {
            using (IDisposable disposable = client as IDisposable)
            {
                IWampClient<TMessage> wampClient = client as IWampClient<TMessage>;

                wampClient.GoodbyeSent = true;
                wampClient.Realm.Abort(wampClient.Session, details, reason);
            }
        }

        public void Authenticate(IWampSessionClient client, string signature, TMessage extra)
        {
        }

        public void Goodbye(IWampSessionClient client, TMessage details, string reason)
        {
            using (IDisposable disposable = client as IDisposable)
            {
                client.Goodbye(details, WampErrors.CloseNormal);

                IWampClient<TMessage> wampClient = client as IWampClient<TMessage>;
                wampClient.GoodbyeSent = true;
                wampClient.Realm.Goodbye(wampClient.Session, details, reason);
            }
        }

        public void Heartbeat(IWampSessionClient client, int incomingSeq, int outgoingSeq)
        {
        }

        public void Heartbeat(IWampSessionClient client, int incomingSeq, int outgoingSeq, string discard)
        {
        }

        public IWampBindedRealmContainer<TMessage> RealmContainer
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