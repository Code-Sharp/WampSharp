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

        public bool IsAuthenticated
        {
            get
            {
                return mAuthenticator.IsAuthenticated;
            }
        }

        public string AuthenticationId
        {
            get
            {
                return mAuthenticator.AuthenticationId;
            }
        }

        public string AuthenticationMethod
        {
            get
            {
                return mAuthenticator.AuthenticationMethod;
            }
        }

        public ChallengeDetails ChallengeDetails
        {
            get
            {
                return mAuthenticator.ChallengeDetails;
            }
        }

        public void Authenticate(string signature, AuthenticateExtraData extra)
        {
            mAuthenticator.Authenticate(signature, extra);
        }

        public IWampAuthorizer Authorizer
        {
            get
            {
                return mAuthorizer;
            }
        }

        public WelcomeDetails WelcomeDetails
        {
            get { return mAuthenticator.WelcomeDetails; }
        }
    }
}