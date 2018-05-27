using System.Collections.Generic;

namespace WampSharp.V2.Authentication
{
    /// <summary>
    /// Represents an implementation of <see cref="IWampAuthenticationProvider"/>
    /// based on static data.
    /// </summary>
    public class WampStaticAuthenticationProvider : IWampAuthenticationProvider
    {
        private readonly IDictionary<string, IDictionary<string, WampAuthenticationRole>> mRealmToRoleNameToRole;

        /// <summary>
        /// Initializes a new instance of <see cref="WampStaticAuthenticationProvider"/>
        /// given the static data.
        /// </summary>
        /// <param name="realmToRoleNameToRole">A mapping of realm name -> role name -> role object.</param>
        public WampStaticAuthenticationProvider(IDictionary<string, IDictionary<string, WampAuthenticationRole>> realmToRoleNameToRole)
        {
            mRealmToRoleNameToRole = realmToRoleNameToRole;
        }

        public virtual string ProviderName => "static";

        public WampAuthenticationRole GetRoleByName(string realm, string role)
        {

            if (mRealmToRoleNameToRole.TryGetValue(realm, out IDictionary<string, WampAuthenticationRole> roleNameToRole))
            {

                if (roleNameToRole.TryGetValue(role, out WampAuthenticationRole result))
                {
                    return result;
                }
            }

            return null;
        }
    }
}