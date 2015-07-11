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

            IWampSessionAuthenticator authenticator =
                mSessionAuthenticatorFactory.GetSessionAuthenticator
                    (realm,
                     details,
                     wampClient.Authenticator);

            if (authenticator == null)
            {
                throw new Exception("Get null authenticator.");
            }

            wampClient.Authenticator = authenticator;

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

                if (authenticator.IsAuthenticated)
                {
                    OnClientJoin(wampClient, default(TMessage));
                }
                else
                {
                    SendAbort(client,
                              new WampAuthenticationException(new AbortDetails(),
                                                              WampErrors.AuthorizationFailed));
                }
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

        protected override WelcomeDetails GetWelcomeDetails(IWampClientProxy<TMessage> wampClient)
        {
            WelcomeDetails welcomeDetails = 
                base.GetWelcomeDetails(wampClient);

            IWampSessionAuthenticator authenticator = wampClient.Authenticator;

            WelcomeDetails result = 
                authenticator.WelcomeDetails ?? welcomeDetails;

            result.Roles = welcomeDetails.Roles;
            result.AuthenticationMethod = authenticator.AuthenticationMethod;
            result.AuthenticationId = authenticator.AuthenticationId;

            return result;
        }
    }
}