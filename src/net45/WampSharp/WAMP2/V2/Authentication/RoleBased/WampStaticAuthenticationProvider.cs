using System.Collections.Generic;

namespace WampSharp.V2.Authentication
{
    public class WampStaticAuthenticationProvider : IWampAuthenticationProvider
    {
        private readonly IDictionary<string, IDictionary<string, WampAuthenticationRole>> mRealmToRoleNameToRole;

        public WampStaticAuthenticationProvider(IDictionary<string, IDictionary<string, WampAuthenticationRole>> realmToRoleNameToRole)
        {
            mRealmToRoleNameToRole = realmToRoleNameToRole;
        }

        public virtual string ProviderName
        {
            get
            {
                return "static";
            }
        }

        public WampAuthenticationRole GetRoleByName(string realm, string role)
        {
            IDictionary<string, WampAuthenticationRole> roleNameToRole;

            if (mRealmToRoleNameToRole.TryGetValue(realm, out roleNameToRole))
            {
                WampAuthenticationRole result;

                if (roleNameToRole.TryGetValue(role, out result))
                {
                    return result;
                }
            }

            return null;
        }
    }
}