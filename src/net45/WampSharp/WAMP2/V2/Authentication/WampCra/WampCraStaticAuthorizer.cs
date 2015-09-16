using System;
using System.Collections.Generic;
using System.Linq;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Authentication
{
    public class WampCraStaticAuthorizer : IWampAuthorizer
    {
        private readonly Dictionary<string, WampCraUriPermissions> mExactUriToPermissions =
            new Dictionary<string, WampCraUriPermissions>();

        private readonly SortedDictionary<string, WampCraUriPermissions> mPrefixedUriToPermissions =
            new SortedDictionary<string, WampCraUriPermissions>(StringComparer.Ordinal);

        public WampCraStaticAuthorizer(ICollection<WampCraUriPermissions> uriPermissions)
        {
            foreach (WampCraUriPermissions currentUriPermissions in uriPermissions)
            {
                IDictionary<string, WampCraUriPermissions> permissions;

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