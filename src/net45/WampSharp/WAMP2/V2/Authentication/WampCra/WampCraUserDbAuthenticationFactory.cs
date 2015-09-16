#if !PCL
using System.Linq;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Authentication
{
    public class WampCraUserDbAuthenticationFactory : IWampSessionAuthenticatorFactory
    {
        private const string WAMP_CRA = "wampcra";
        private readonly IWampCraAuthenticationProvider mAuthenticationProvider;
        private readonly IWampCraUserDb mUserDb;

        public WampCraUserDbAuthenticationFactory(IWampCraAuthenticationProvider authenticationProvider,
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

            if (!helloDetails.AuthenticationMethods.Contains(WAMP_CRA))
            {
                throw new WampAuthenticationException("supports only 'wampcra' authentication");
            }

            WampCraUser user = 
                mUserDb.GetUserById(helloDetails.AuthenticationId);

            if (user == null)
            {
                throw new WampAuthenticationException
                    (string.Format("no user with authid '{0}' in user database",
                                   user.AuthenticationId));
            }

            user.AuthenticationId = user.AuthenticationId ?? 
                                    helloDetails.AuthenticationId;

            string authenticationRole = user.AuthenticationRole;

            WampCraAuthenticationRole role = 
                mAuthenticationProvider.GetRoleByName(details.Realm, authenticationRole);

            if (role == null)
            {
                throw new WampAuthenticationException
                    (message: string.Format("authentication failed - realm '{0}' has no role '{1}'",
                                            details.Realm,
                                            authenticationRole),
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

#endif