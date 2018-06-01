using System;
using System.Collections.Generic;

namespace WampSharp.V1.Cra
{
    public class WampCraPermissionsMapper
    {
        private readonly Dictionary<string, WampRpcPermissions> mRpcPermissions;
        private readonly Dictionary<string, WampPubSubPermissions> mPubSubPermissions;
        private readonly List<WampPubSubPermissions> mPubSubPrefixPermissions;

        public WampCraPermissionsMapper()
        {
            mRpcPermissions = new Dictionary<string, WampRpcPermissions>(StringComparer.Ordinal);
            mPubSubPermissions = new Dictionary<string, WampPubSubPermissions>(StringComparer.Ordinal);
            mPubSubPrefixPermissions = new List<WampPubSubPermissions>();
        }

        public void AddPermissions(WampCraPermissions permissions)
        {
            foreach (WampRpcPermissions rpc in permissions.rpc)
            {
                AddRpcPermission(rpc);
            }

            foreach (WampPubSubPermissions pubsub in permissions.pubsub)
            {
                AddPubSubPermission(pubsub);
            }
        }

        public void AddRpcPermission(WampRpcPermissions rpcPermission)
        {
            mRpcPermissions[rpcPermission.uri] = rpcPermission;
        }

        public void AddPubSubPermission(WampPubSubPermissions pubSubPermission)
        {
            if (pubSubPermission.prefix)
            {
                mPubSubPrefixPermissions.Add(pubSubPermission);
            }
            else
            {
                mPubSubPermissions[pubSubPermission.uri] = pubSubPermission;
            }
        }

        public WampRpcPermissions LookupRpcPermissions(string unprefixedUri)
        {
            if (mRpcPermissions == null || unprefixedUri == null)
            {
                return null;
            }

            mRpcPermissions.TryGetValue(unprefixedUri, out WampRpcPermissions rpc);
            return rpc;
        }

        public WampPubSubPermissions LookupPubSubPermissions(string unprefixedUri)
        {
            if (mPubSubPermissions == null || unprefixedUri == null)
            {
                return null;
            }


            if (mPubSubPermissions.TryGetValue(unprefixedUri, out WampPubSubPermissions pubsub))
            {
                return pubsub;
            }

            foreach (WampPubSubPermissions permission in mPubSubPrefixPermissions)
            {
                if (unprefixedUri.StartsWith(permission.uri, StringComparison.Ordinal))
                {
                    return permission;
                }
            }

            return null;
        }
    }
}