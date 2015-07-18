using System;
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
        private readonly WelcomeDetails mWelcomeDetails = GetWelcomeDetails();

        private static WelcomeDetails GetWelcomeDetails()
        {
            return new WelcomeDetails()
            {
                Roles = new RouterRoles()
                {
                    Dealer = new DealerFeatures()
                    {
                        PatternBasedRegistration = true,
                        SharedRegistration = true,
                        CallerIdentification = true,
                        ProgressiveCallResults = true
                    },
                    Broker = new BrokerFeatures()
                    {
                        PublisherIdentification = true,
                        PatternBasedSubscription = true,
                        PublisherExclusion = true,
                        SubscriberBlackwhiteListing = true
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

            OnClientJoin(wampClient, details);
        }

        protected IWampClientProxy<TMessage> GetWampClient(IWampSessionClient client, string realm, HelloDetails details)
        {
            IWampClientProxy<TMessage> wampClient = client as IWampClientProxy<TMessage>;

            IWampBindedRealm<TMessage> bindedRealm = mRealmContainer.GetRealmByName(realm);

            wampClient.HelloDetails = details;
            
            wampClient.Realm = bindedRealm;
            
            return wampClient;
        }

        public virtual void Authenticate(IWampSessionClient client, string signature, AuthenticateExtraData extra)
        {
            // TODO: disconnect client.
        }

        protected void OnClientJoin(IWampClientProxy<TMessage> wampClient,
                                    HelloDetails details)
        {
            WelcomeDetails welcomeDetails = GetWelcomeDetails(wampClient);

            wampClient.Realm.Hello(wampClient.Session, details, welcomeDetails);

            wampClient.Welcome(wampClient.Session, welcomeDetails);
        }

        public virtual void Abort(IWampSessionClient client, AbortDetails details, string reason)
        {
            using (IDisposable disposable = client as IDisposable)
            {
                IWampClientProxy<TMessage> wampClient = client as IWampClientProxy<TMessage>;

                wampClient.GoodbyeSent = true;
                wampClient.Realm.Abort(wampClient.Session, details, reason);
            }
        }

        public virtual void Goodbye(IWampSessionClient client, GoodbyeDetails details, string reason)
        {
            using (IDisposable disposable = client as IDisposable)
            {
                client.Goodbye(details, WampErrors.CloseNormal);

                IWampClientProxy<TMessage> wampClient = client as IWampClientProxy<TMessage>;
                wampClient.GoodbyeSent = true;
                wampClient.Realm.Goodbye(wampClient.Session, details, reason);
            }
        }

        protected virtual WelcomeDetails GetWelcomeDetails(IWampClientProxy<TMessage> wampClient)
        {
            return mWelcomeDetails;
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