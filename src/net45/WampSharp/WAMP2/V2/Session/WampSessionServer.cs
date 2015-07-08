using System;
using System.Collections.Generic;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm;
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
            mWelcomeDetails = new Dictionary<string, object>()
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
                                 IWampEventSerializer eventSerializer) :
            this()
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

        public void Hello(IWampSessionClient client, string realm, HelloDetails details)
        {
            IWampClientProxy<TMessage> wampClient = client as IWampClientProxy<TMessage>;

            IWampBindedRealm<TMessage> bindedRealm = mRealmContainer.GetRealmByName(realm);

            wampClient.Realm = bindedRealm;

            // TODO: Set authenticator with IWampAuthenticator
            IWampSessionAuthenticator authenticator = wampClient.Authenticator ??
                new TrustedAuthenticator();

            try
            {
                bool authenticated = authenticator.IsAuthenticated;

                if (authenticated)
                {
                    OnClientJoin(wampClient);
                }
                else
                {
                    wampClient.Challenge(authenticator.AuthenticationMethod,
                                         authenticator.Details);
                }
            }
            catch (WampAuthenticationException ex)
            {
                using (IDisposable disposable = client as IDisposable)
                {
                    client.Abort(ex.Details, ex.Reason);
                }
            }
        }

        internal class TrustedAuthenticator : IWampSessionAuthenticator
        {
            public bool IsAuthenticated { get { return true; }}
            public string AuthenticationId { get { return "anonymous"; } }
            public string AuthenticationMethod { get; private set; }
            public ChallengeDetails Details { get; private set; }
            public void Authenticate(string signature, AuthenticateExtraData extra)
            {
                throw new NotImplementedException();
            }

            public IWampAuthorizer Authorizer { get; private set; }
        }

        public void Authenticate(IWampSessionClient client, string signature, AuthenticateExtraData extra)
        {
            IWampClientProxy<TMessage> wampClient = client as IWampClientProxy<TMessage>;

            IWampSessionAuthenticator authenticator = wampClient.Authenticator;

            try
            {
                authenticator.Authenticate(signature, extra);

                OnClientJoin(wampClient);
            }
            catch (WampAuthenticationException ex)
            {
                using (IDisposable disposable = client as IDisposable)
                {
                    wampClient.Abort(ex.Details, ex.Reason);
                }
            }
        }

        private void OnClientJoin(IWampClientProxy<TMessage> wampClient)
        {
            // TODO: change this to welcome details
            TMessage details = default(TMessage);

            wampClient.Realm.Hello(wampClient.Session, details);

            var welcomeDetails = 
                new Dictionary<string, object>(mWelcomeDetails);
            
            // TODO: Fill welcome details
            wampClient.Welcome(wampClient.Session, welcomeDetails);
        }

        public void Abort(IWampSessionClient client, TMessage details, string reason)
        {
            using (IDisposable disposable = client as IDisposable)
            {
                IWampClientProxy<TMessage> wampClient = client as IWampClientProxy<TMessage>;

                wampClient.GoodbyeSent = true;
                wampClient.Realm.Abort(wampClient.Session, details, reason);
            }
        }

        public void Goodbye(IWampSessionClient client, TMessage details, string reason)
        {
            using (IDisposable disposable = client as IDisposable)
            {
                client.Goodbye(details, WampErrors.CloseNormal);

                IWampClientProxy<TMessage> wampClient = client as IWampClientProxy<TMessage>;
                wampClient.GoodbyeSent = true;
                wampClient.Realm.Goodbye(wampClient.Session, details, reason);
            }
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