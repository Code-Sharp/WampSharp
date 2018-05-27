using System;
using System.Collections.Generic;
using System.Linq;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Authentication
{
    /// <summary>
    /// An implementation of <see cref="IWampAuthorizer"/> based on static data.
    /// </summary>
    public class WampStaticAuthorizer : IWampAuthorizer
    {
        private readonly Dictionary<string, WampUriPermissions> mExactUriToPermissions =
            new Dictionary<string, WampUriPermissions>();

        private readonly SortedDictionary<string, WampUriPermissions> mPrefixedUriToPermissions =
            new SortedDictionary<string, WampUriPermissions>(StringComparer.Ordinal);

        /// <summary>
        /// Initializes a new instance of <see cref="WampStaticAuthorizer"/> given
        /// the uri permissions data.
        /// </summary>
        /// <param name="uriPermissions">A collection of the uri permissions.</param>
        public WampStaticAuthorizer(ICollection<WampUriPermissions> uriPermissions)
        {
            foreach (WampUriPermissions currentUriPermissions in uriPermissions)
            {
                IDictionary<string, WampUriPermissions> permissions;

                if (currentUriPermissions.Prefixed)
                {
                    permissions = mPrefixedUriToPermissions;
                }
                else
                {
                    permissions = mExactUriToPermissions;
                }

                MapPermission(permissions, currentUriPermissions);
            }
        }

        private void MapPermission(IDictionary<string, WampUriPermissions> permissionsMap, WampUriPermissions uriPermissions)
        {
            if (!permissionsMap.ContainsKey(uriPermissions.Uri))
            {
                permissionsMap[uriPermissions.Uri] = uriPermissions;
            }
            else
            {
                throw new ArgumentException
                    ($"Role has specified permissions for uri '{uriPermissions.Uri}' more than once",
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

        private bool CheckAction(string procedure, Func<WampUriPermissions, bool> permissionChecker)
        {

            if (mExactUriToPermissions.TryGetValue(procedure, out WampUriPermissions permissions))
            {
                return permissionChecker(permissions);
            }

            foreach (WampUriPermissions prefixedUriToPermission in 
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