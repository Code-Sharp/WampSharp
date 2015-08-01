#if !PCL
using System;
using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Cra;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Authentication
{
    public class WampCraStaticAuthenticationProvider : IWampCraAuthenticationProvider
    {
        private readonly IDictionary<string, IDictionary<string, WampCraAuthenticationRole>> mRealmToRoleNameToRole;

        public WampCraStaticAuthenticationProvider(IDictionary<string, IDictionary<string, WampCraAuthenticationRole>> realmToRoleNameToRole)
        {
            mRealmToRoleNameToRole = realmToRoleNameToRole;
        }

        public string ProviderName
        {
            get
            {
                return "static";
            }
        }

        public WampCraAuthenticationRole GetRoleByName(string realm, string role)
        {
            IDictionary<string, WampCraAuthenticationRole> roleNameToRole;

            if (mRealmToRoleNameToRole.TryGetValue(realm, out roleNameToRole))
            {
                WampCraAuthenticationRole result;

                if (roleNameToRole.TryGetValue(role, out result))
                {
                    return result;
                }
            }

            return null;
        }
    }

    public class WampCraStaticUserDb : IWampCraUserDb
    {
        private readonly IDictionary<string, WampCraUser> mUsers;

        public WampCraStaticUserDb(IDictionary<string, WampCraUser> users)
        {
            mUsers = users;
        }

        public WampCraUser GetUserById(string authenticationId)
        {
            WampCraUser result;
            mUsers.TryGetValue(authenticationId, out result);
            return result;
        }
    }

    public interface IWampCraUserDb
    {
        WampCraUser GetUserById(string authenticationId);
    }

    public interface IWampCraAuthenticationProvider
    {
        string ProviderName { get; }

        WampCraAuthenticationRole GetRoleByName(string realm, string role);
    }

    public class WampCraAuthenticationRole
    {
        public string AuthenticationRole { get; set; }

        public string AuthenticationProvider { get; set; }

        public ICollection<WampCraUriPermissions> Permissions { get; set; }
    }

    public class WampCraUriPermissions
    {
        public string Uri { get; set; }

        public bool Prefixed { get; set; }

        public bool CanPublish { get; set; }

        public bool CanSubscribe { get; set; }

        public bool CanCall { get; set; }

        public bool CanRegister { get; set; }
    }

    public class WampCraUser : IWampCraChallenge
    {
        public string AuthenticationId { get; set; }

        public string AuthenticationRole { get; set; }

        public string Secret { get; set; }

        public string Salt { get; set; }

        public int? Iterations { get; set; }

        public int? KeyLength { get; set; }
    }

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
            (PendingClientDetails details,
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

            Authorizer = new WampCraUserDbAuthorizer(role);

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

    public class WampCraUserDbAuthorizer : IWampAuthorizer
    {
        private readonly Dictionary<string, WampCraUriPermissions> mExactUriToPermissions =
            new Dictionary<string, WampCraUriPermissions>();

        private readonly SortedDictionary<string, WampCraUriPermissions> mPrefixedUriToPermissions =
            new SortedDictionary<string, WampCraUriPermissions>(StringComparer.Ordinal);

        public WampCraUserDbAuthorizer(WampCraAuthenticationRole role)
        {
            foreach (WampCraUriPermissions uriPermissions in role.Permissions)
            {
                IDictionary<string, WampCraUriPermissions> permissions;

                if (uriPermissions.Prefixed)
                {
                    permissions = mPrefixedUriToPermissions;
                }
                else
                {
                    permissions = mExactUriToPermissions;
                }

                MapPermission(permissions, uriPermissions);
            }
        }

        private void MapPermission(IDictionary<string, WampCraUriPermissions> permissionsMap, WampCraUriPermissions uriPermissions)
        {
            if (!permissionsMap.ContainsKey(uriPermissions.Uri))
            {
                permissionsMap[uriPermissions.Uri] = uriPermissions;
            }
            else
            {
                throw new ArgumentException
                    (string.Format("Role has specified permissions for uri '{0}' more than once",
                                   uriPermissions.Uri),
                     "role");
            }
        }

        public bool CanRegister(RegisterOptions options, string procedure)
        {
            return CheckAction(procedure, permissions => permissions.CanRegister);
        }

        public bool CanCall(CallOptions options, string procedure)
        {
            return CheckAction(procedure, permissions => permissions.CanCall);
        }

        public bool CanPublish(PublishOptions options, string topicUri)
        {
            return CheckAction(topicUri, permissions => permissions.CanPublish);
        }

        public bool CanSubscribe(SubscribeOptions options, string topicUri)
        {
            return CheckAction(topicUri, permissions => permissions.CanSubscribe);
        }

        private bool CheckAction(string procedure, Func<WampCraUriPermissions, bool> permissionChecker)
        {
            WampCraUriPermissions permissions;

            if (mExactUriToPermissions.TryGetValue(procedure, out permissions))
            {
                return permissionChecker(permissions);
            }

            foreach (WampCraUriPermissions prefixedUriToPermission in 
                mPrefixedUriToPermissions.Values.Reverse())
            {
                string currentUri = prefixedUriToPermission.Uri;

                if (procedure.StartsWith(currentUri))
                {
                    return permissionChecker(prefixedUriToPermission);
                }
            }

            return false;
        }
    }
}
#endif