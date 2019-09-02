using System.Linq;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Authentication
{
    /// <summary>
    /// An implementation of <see cref="IWampSessionAuthenticatorFactory"/>
    /// which is based on <see cref="IWampCraUserDb"/> and <see cref="IWampAuthenticationProvider"/>.
    /// </summary>
    public class WampCraUserDbAuthenticationFactory : IWampSessionAuthenticatorFactory
    {
        private const string WampCra = WampAuthenticationMethods.WampCra;
        private readonly IWampAuthenticationProvider mAuthenticationProvider;
        private readonly IWampCraUserDb mUserDb;

        /// <summary>
        /// Instantiates a new instance of <see cref="WampCraUserDbAuthenticationFactory"/>
        /// given the <see cref="IWampAuthenticationProvider"/> and
        /// the <see cref="IWampCraUserDb"/> to use.
        /// </summary>
        /// <param name="authenticationProvider">The <see cref="IWampAuthenticationProvider"/> to get role permissions.</param>
        /// <param name="userDb">The <see cref="IWampCraUserDb"/> to use to get user WAMP-CRA details.</param>
        public WampCraUserDbAuthenticationFactory(IWampAuthenticationProvider authenticationProvider,
                                                  IWampCraUserDb userDb)
        {
            mAuthenticationProvider = authenticationProvider;
            mUserDb = userDb;
        }

        public IWampSessionAuthenticator GetSessionAuthenticator
            (WampPendingClientDetails details,
             IWampSessionAuthenticator transportAuthenticator)
        {
            HelloDetails helloDetails = details.HelloDetails;

            if ((helloDetails.AuthenticationMethods == null) || 
                !helloDetails.AuthenticationMethods.Contains(WampCra))
            {
                throw new WampAuthenticationException("supports only 'wampcra' authentication");
            }

            WampCraUser user = 
                mUserDb.GetUserById(helloDetails.AuthenticationId);

            if (user == null)
            {
                throw new WampAuthenticationException
                    ($"no user with authid '{helloDetails.AuthenticationId}' in user database");
            }

            user.AuthenticationId = user.AuthenticationId ?? 
                                    helloDetails.AuthenticationId;

            string authenticationRole = user.AuthenticationRole;

            WampAuthenticationRole role = 
                mAuthenticationProvider.GetRoleByName(details.Realm, authenticationRole);

            if (role == null)
            {
                throw new WampAuthenticationException
                    (message: $"authentication failed - realm '{details.Realm}' has no role '{authenticationRole}'",
                     reason: WampErrors.NoSuchRole);
            }

            role.AuthenticationRole = role.AuthenticationRole ??
                                      authenticationRole;

            role.AuthenticationProvider = role.AuthenticationProvider ??
                                          mAuthenticationProvider.ProviderName;

            return new WampCraUserDbSessionAuthenticator(user, role, details.SessionId);
        }
    }
}