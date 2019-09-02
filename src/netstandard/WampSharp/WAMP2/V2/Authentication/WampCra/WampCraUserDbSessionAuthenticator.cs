using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Authentication
{
    /// <summary>
    /// An implementation of <see cref="IWampSessionAuthenticator"/> which uses
    /// <see cref="WampCraUser"/> in order to compute WAMP-CRA key, and uses
    /// <see cref="WampAuthenticationRole"/> in order to check permissions.
    /// </summary>
    public class WampCraUserDbSessionAuthenticator : WampCraSessionAuthenticator
    {
        private readonly WampCraUser mUser;
        private readonly string mAuthenticationChallenge;

        /// <summary>
        /// Instantiates a new instance of <see cref="WampCraUserDbSessionAuthenticator"/>
        /// given the WAMP-CRA user details, the <see cref="WampAuthenticationRole"/> and
        /// the user's session id.
        /// </summary>
        /// <param name="user">The given user's WAMP-CRA details, used for authentication.</param>
        /// <param name="role">The given user's role, used for authorization.</param>
        /// <param name="sessionId">The given user's session id.</param>
        public WampCraUserDbSessionAuthenticator(WampCraUser user, WampAuthenticationRole role, long sessionId) :
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

        public override string AuthenticationChallenge => mAuthenticationChallenge;

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

        public override string Secret => mUser.Secret;
    }
}