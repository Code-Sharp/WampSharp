#if !PCL
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Authentication
{
    public class WampCraUserDbSessionAuthenticator : WampCraSessionAuthenticator
    {
        private readonly WampCraUser mUser;
        private readonly string mAuthenticationChallenge;

        public WampCraUserDbSessionAuthenticator(WampCraUser user, WampCraAuthenticationRole role, long sessionId) :
            base(user.AuthenticationId)
        {
            mUser = user;

            if (user.Salt != null)
            {
                CraChallengeDetails =
                    new WampCraChallengeDetails(user.Salt,
                                                user.Iterations,
                                                user.KeyLength);
            }

            WelcomeDetails = new WelcomeDetails
            {
                AuthenticationRole = role.AuthenticationRole,
                AuthenticationProvider = role.AuthenticationProvider,
            };

            WampCraPendingClientDetails details =
                new WampCraPendingClientDetails()
                {
                    AuthenticationId = user.AuthenticationId,
                    SessionId = sessionId,
                    AuthenticationProvider = role.AuthenticationProvider,
                    AuthenticationRole = role.AuthenticationRole
                };

            Authorizer = role.Authorizer;

            mAuthenticationChallenge = details.ToString();
        }

        public override string AuthenticationChallenge
        {
            get
            {
                return mAuthenticationChallenge;
            }
        }

        public sealed override WelcomeDetails WelcomeDetails
        {
            get;
            protected set;
        }

        public sealed override IWampAuthorizer Authorizer
        {
            get;
            protected set;
        }

        public override string Secret
        {
            get
            {
                return mUser.Secret;
            }
        }
    }
}
#endif