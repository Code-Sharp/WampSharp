using System;
using System.Collections.Generic;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm;

namespace WampSharp.V2.Session
{
    internal class WampAuthenticationSessionServer<TMessage> : WampSessionServer<TMessage>
    {
        private readonly IWampSessionAuthenticatorFactory mSessionAuthenticatorFactory;

        public WampAuthenticationSessionServer(IWampBinding<TMessage> binding,
                                               IWampHostedRealmContainer realmContainer,
                                               IWampRouterBuilder builder,
                                               IWampEventSerializer eventSerializer,
                                               IWampSessionAuthenticatorFactory sessionAuthenticatorFactory)
            : base(binding, realmContainer, builder, eventSerializer)
        {
            mSessionAuthenticatorFactory = sessionAuthenticatorFactory;
        }

        public override void Hello(IWampSessionClient client, string realm, HelloDetails details)
        {
            IWampClientProxy<TMessage> wampClient = GetWampClient(client, realm, details);

            // TODO: Set authenticator with IWampAuthenticator
            IWampSessionAuthenticator authenticator =
                mSessionAuthenticatorFactory.GetSessionAuthenticator
                    (realm,
                     details,
                     wampClient.Authenticator);

            try
            {
                bool authenticated = authenticator.IsAuthenticated;

                if (authenticated)
                {
                    OnClientJoin(wampClient, default(TMessage));
                }
                else
                {
                    wampClient.Challenge(authenticator.AuthenticationMethod,
                                         authenticator.Details);
                }
            }
            catch (WampAuthenticationException ex)
            {
                SendAbort(client, ex);
            }
        }

        public override void Authenticate(IWampSessionClient client, string signature, AuthenticateExtraData extra)
        {
            IWampClientProxy<TMessage> wampClient = client as IWampClientProxy<TMessage>;

            IWampSessionAuthenticator authenticator = wampClient.Authenticator;

            try
            {
                authenticator.Authenticate(signature, extra);

                OnClientJoin(wampClient, default(TMessage));
            }
            catch (WampAuthenticationException ex)
            {
                SendAbort(client, ex);
            }
        }

        private static void SendAbort(IWampSessionClient client, WampAuthenticationException ex)
        {
            using (IDisposable disposable = client as IDisposable)
            {
                client.Abort(ex.Details, ex.Reason);
            }
        }

        protected override Dictionary<string, object> GetWelcomeDetails(IWampClientProxy<TMessage> wampClient)
        {
            Dictionary<string, object> result = 
                base.GetWelcomeDetails(wampClient);
            
            result["authmethod"] = wampClient.Authenticator.AuthenticationMethod;
            result["authid"] = wampClient.Authenticator.AuthenticationId;

            return result;
        }
    }
}