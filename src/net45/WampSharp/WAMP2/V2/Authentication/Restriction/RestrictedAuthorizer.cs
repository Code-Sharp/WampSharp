using System;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Authentication
{
    internal class RestrictedAuthorizer : IWampAuthorizer
    {
        private readonly IWampSessionAuthenticator mAuthenticator;

        public RestrictedAuthorizer(IWampSessionAuthenticator authenticator)
        {
            mAuthenticator = authenticator;
        }

        public bool CanRegister(RegisterOptions options, string procedure)
        {
            if (procedure.StartsWith("wamp.", StringComparison.Ordinal))
            {
                return false;
            }
            else
            {
                return Authorizer.CanRegister(options, procedure);                
            }
        }

        public bool CanCall(CallOptions options, string procedure)
        {
            return Authorizer.CanCall(options, procedure);
        }

        public bool CanPublish(PublishOptions options, string topicUri)
        {
            if (topicUri.StartsWith("wamp.", StringComparison.Ordinal))
            {
                return false;
            }
            else
            {
                return Authorizer.CanPublish(options, topicUri);
            }
        }

        public bool CanSubscribe(SubscribeOptions options, string topicUri)
        {
            return Authorizer.CanSubscribe(options, topicUri);
        }

        public IWampAuthorizer Authorizer
        {
            get
            {
                return mAuthenticator.Authorizer;
            }
        }
    }
}