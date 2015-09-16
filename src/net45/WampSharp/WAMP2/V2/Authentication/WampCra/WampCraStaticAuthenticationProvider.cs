using System.Collections.Generic;

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
}