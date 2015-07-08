using System;
using System.Collections.Generic;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm;
using WampSharp.V2.Realm.Binded;

namespace WampSharp.V2.Session
{
    internal class WampSessionServer<TMessage> : IWampSessionServer<TMessage>
    {
        private IWampBindedRealmContainer<TMessage> mRealmContainer;
        private readonly Dictionary<string, object> mWelcomeDetails = GetWelcomeDetails();

        private static Dictionary<string, object> GetWelcomeDetails()
        {
            return new Dictionary<string, object>()
            {
                {
                    "roles",
                    new Dictionary<string, object>()
                    {
                        {
                            "dealer", new Dictionary<string, object>()
                            {
                                {"pattern_based_registration", true},
                                //{"registration_revocation", true},
                                {"shared_registration", true},
                                {"caller_identification", true},
                                //{"registration_meta_api", true},
                                {"progressive_call_results", true},
                            }
                        },
                        {
                            "broker", new Dictionary<string, object>()
                            {
                                {
                                    "features",
                                    new Dictionary<string, object>()
                                    {
                                        {"publisher_identification", true},
                                        {"pattern_based_subscription", true},
                                        //{"subscription_meta_api", true},
                                        //{"subscription_revocation", true},
                                        {"publisher_exclusion", true},
                                        {"subscriber_blackwhite_listing", true},
                                    }
                                }
                            }
                        },
                    }
                }
            };
        }

        public WampSessionServer(IWampBinding<TMessage> binding,
                                 IWampHostedRealmContainer realmContainer,
                                 IWampRouterBuilder builder,
                                 IWampEventSerializer eventSerializer)
        {
            mRealmContainer =
                new WampBindedRealmContainer<TMessage>(realmContainer,
                                                       this,
                                                       builder,
                                                       eventSerializer,
                                                       binding);
        }

        public void OnNewClient(IWampClientProxy<TMessage> client)
        {
        }

        public void OnClientDisconnect(IWampClientProxy<TMessage> client)
        {
            if ((client.Realm != null) && !client.GoodbyeSent)
            {
                client.Realm.SessionLost(client.Session);
            }
        }

        public virtual void Hello(IWampSessionClient client, string realm, HelloDetails details)
        {
            IWampClientProxy<TMessage> wampClient = GetWampClient(client, realm, details);

            OnClientJoin(wampClient, default(TMessage));
        }

        protected IWampClientProxy<TMessage> GetWampClient(IWampSessionClient client, string realm, HelloDetails details)
        {
            IWampClientProxy<TMessage> wampClient = client as IWampClientProxy<TMessage>;

            IWampBindedRealm<TMessage> bindedRealm = mRealmContainer.GetRealmByName(realm);

            wampClient.Roles = details.Roles;
            
            wampClient.Realm = bindedRealm;
            
            return wampClient;
        }

        public virtual void Authenticate(IWampSessionClient client, string signature, AuthenticateExtraData extra)
        {
            // TODO: disconnect client.
        }

        protected void OnClientJoin(IWampClientProxy<TMessage> wampClient,
            // TODO: change this to welcome details
            TMessage details)
        {
            wampClient.Realm.Hello(wampClient.Session, details);

            IDictionary<string, object> welcomeDetails = GetWelcomeDetails(wampClient);

            wampClient.Welcome(wampClient.Session, welcomeDetails);
        }

        public virtual void Abort(IWampSessionClient client, TMessage details, string reason)
        {
            using (IDisposable disposable = client as IDisposable)
            {
                IWampClientProxy<TMessage> wampClient = client as IWampClientProxy<TMessage>;

                wampClient.GoodbyeSent = true;
                wampClient.Realm.Abort(wampClient.Session, details, reason);
            }
        }

        public virtual void Goodbye(IWampSessionClient client, TMessage details, string reason)
        {
            using (IDisposable disposable = client as IDisposable)
            {
                client.Goodbye(details, WampErrors.CloseNormal);

                IWampClientProxy<TMessage> wampClient = client as IWampClientProxy<TMessage>;
                wampClient.GoodbyeSent = true;
                wampClient.Realm.Goodbye(wampClient.Session, details, reason);
            }
        }

        protected virtual Dictionary<string, object> GetWelcomeDetails(IWampClientProxy<TMessage> wampClient)
        {
            var welcomeDetails =
                new Dictionary<string, object>(mWelcomeDetails);

            return welcomeDetails;
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