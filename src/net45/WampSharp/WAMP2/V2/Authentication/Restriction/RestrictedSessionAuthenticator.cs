using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Authentication
{
    internal class RestrictedSessionAuthenticator : IWampSessionAuthenticator
    {
        private readonly IWampSessionAuthenticator mAuthenticator;
        private readonly RestrictedAuthorizer mAuthorizer;

        public RestrictedSessionAuthenticator(IWampSessionAuthenticator authenticator)
        {
            mAuthenticator = authenticator;
            mAuthorizer = new RestrictedAuthorizer(mAuthenticator);
        }

        public bool IsAuthenticated => mAuthenticator.IsAuthenticated;

        public string AuthenticationId => mAuthenticator.AuthenticationId;

        public string AuthenticationMethod => mAuthenticator.AuthenticationMethod;

        public ChallengeDetails ChallengeDetails => mAuthenticator.ChallengeDetails;

        public void Authenticate(string signature, AuthenticateExtraData extra)
        {
            mAuthenticator.Authenticate(signature, extra);
        }

        public IWampAuthorizer Authorizer
        {
            get
            {
                if (mAuthenticator.Authorizer == null)
                {
                    return null;
                }
                
                return mAuthorizer;
            }
        }

        public WelcomeDetails WelcomeDetails => mAuthenticator.WelcomeDetails;
    }
}