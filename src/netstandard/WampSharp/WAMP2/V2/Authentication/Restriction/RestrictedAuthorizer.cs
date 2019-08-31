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
            if (WampRestrictedUris.IsRestrictedUri(procedure))
            {
                throw new WampException(WampErrors.InvalidUri,
                                        $"register for restricted procedure URI '{procedure}'");
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
            if (WampRestrictedUris.IsRestrictedUri(topicUri))
            {
                throw new WampException(WampErrors.InvalidUri,
                                        $"publish with restricted topic URI '{topicUri}'");
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

        public IWampAuthorizer Authorizer => mAuthenticator.Authorizer;
    }
}