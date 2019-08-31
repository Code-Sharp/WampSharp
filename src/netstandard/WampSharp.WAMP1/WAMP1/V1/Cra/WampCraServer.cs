using System;
using WampSharp.V1.Core.Contracts;
using WampSharp.V1.Core.Curie;
using WampSharp.V1.Rpc.Server;

namespace WampSharp.V1.Cra
{
    public class WampCraServer<TMessage> : IWampServer<TMessage>
    {
        private readonly IWampRpcServer<TMessage> mRpcServer;
        private readonly IWampPubSubServer<TMessage> mPubSubServer;
        private readonly IWampAuxiliaryServer mAuxiliaryServer;
        private readonly IWampRpcMetadata mWampCraProceduredMetadata;

        private readonly WampCraAuthenticaticatorBuilder<TMessage> mAuthFactory;

        public WampCraServer(WampCraAuthenticaticatorBuilder<TMessage> authFactory,
            IWampRpcServer<TMessage> rpcServer,
            IWampRpcMetadataCatalog rpcMetadataCatalog,
            IWampPubSubServer<TMessage> pubSubServer = null,
            IWampAuxiliaryServer auxiliaryServer = null)
        {
            mAuthFactory = authFactory;
            mRpcServer = rpcServer;
            mPubSubServer = pubSubServer;
            mAuxiliaryServer = auxiliaryServer;

            // Add ICraProcedures to the metadata catalog so the client can call the auth methods.
            mWampCraProceduredMetadata = new MethodInfoWampCraRpcMetadata(new MockWampCraProcedures());
            rpcMetadataCatalog.Register(mWampCraProceduredMetadata);
        }

        public void Prefix(IWampClient client, string prefix, string uri)
        {
            mAuxiliaryServer.Prefix(client, prefix, uri);
        }

        public void Call(IWampClient client, string callId, string procUri, params TMessage[] arguments)
        {
            string resolvedUri = ResolveUri(client, procUri);
            WampCraAuthenticator<TMessage> wampAuth = GetOrCreateWampAuthenticatorForClient(client);

            WampRpcPermissions rpcPerm = wampAuth.CraPermissionsMapper.LookupRpcPermissions(resolvedUri);
            
            if (rpcPerm != null && rpcPerm.call)
            {
                mRpcServer.Call(client, callId, resolvedUri, arguments);
            }
            else
            {
                client.CallError(callId, "http://api.wamp.ws/error#not_authorized", "No permissions");
            }
        }

        public void Subscribe(IWampClient client, string topicUri)
        {
            string resolvedTopicUri = ResolveUri(client, topicUri);
            WampCraAuthenticator<TMessage> wampAuth = GetOrCreateWampAuthenticatorForClient(client);

            if (wampAuth.IsAuthenticated)
            {
                WampPubSubPermissions pubSubPerm = wampAuth.CraPermissionsMapper.LookupPubSubPermissions(resolvedTopicUri);
                
                if (pubSubPerm != null && pubSubPerm.sub)
                {
                    mPubSubServer.Subscribe(client, resolvedTopicUri);
                }
            }
        }

        public void Unsubscribe(IWampClient client, string topicUri)
        {
            string resolvedTopicUri = ResolveUri(client, topicUri);
            WampCraAuthenticator<TMessage> wampAuth = GetOrCreateWampAuthenticatorForClient(client);
            
            if (wampAuth.IsAuthenticated)
            {
                WampPubSubPermissions pubSubPerm = wampAuth.CraPermissionsMapper.LookupPubSubPermissions(resolvedTopicUri);
                
                if (pubSubPerm != null && pubSubPerm.sub)
                {
                    mPubSubServer.Unsubscribe(client, resolvedTopicUri);
                }
            }
        }

        public void Publish(IWampClient client, string topicUri, TMessage @event)
        {
            string resolvedTopicUri = ResolveUri(client, topicUri);
            WampCraAuthenticator<TMessage> wampAuth = GetOrCreateWampAuthenticatorForClient(client);
            
            if (wampAuth.IsAuthenticated)
            {
                WampPubSubPermissions pubSubPerm = wampAuth.CraPermissionsMapper.LookupPubSubPermissions(resolvedTopicUri);
                if (pubSubPerm != null && pubSubPerm.pub)
                {
                    mPubSubServer.Publish(client, resolvedTopicUri, @event);
                }
            }
        }

        public void Publish(IWampClient client, string topicUri, TMessage @event, bool excludeMe)
        {
            string resolvedTopicUri = ResolveUri(client, topicUri);
            WampCraAuthenticator<TMessage> wampAuth = GetOrCreateWampAuthenticatorForClient(client);
            
            if (wampAuth.IsAuthenticated)
            {
                WampPubSubPermissions pubSubPerm = wampAuth.CraPermissionsMapper.LookupPubSubPermissions(resolvedTopicUri);
                if (pubSubPerm != null && pubSubPerm.pub)
                {
                    mPubSubServer.Publish(client, resolvedTopicUri, @event, excludeMe);
                }
            }
        }

        public void Publish(IWampClient client, string topicUri, TMessage @event, string[] exclude)
        {
            string resolvedTopicUri = ResolveUri(client, topicUri);
            WampCraAuthenticator<TMessage> wampAuth = GetOrCreateWampAuthenticatorForClient(client);
            
            if (wampAuth.IsAuthenticated)
            {
                WampPubSubPermissions pubSubPerm = wampAuth.CraPermissionsMapper.LookupPubSubPermissions(resolvedTopicUri);
                if (pubSubPerm != null && pubSubPerm.pub)
                {
                    mPubSubServer.Publish(client, resolvedTopicUri, @event, exclude);
                }
            }
        }

        public void Publish(IWampClient client, string topicUri, TMessage @event, string[] exclude, string[] eligible)
        {
            string resolvedTopicUri = ResolveUri(client, topicUri);
            WampCraAuthenticator<TMessage> wampAuth = GetOrCreateWampAuthenticatorForClient(client);
            
            if (wampAuth.IsAuthenticated)
            {
                WampPubSubPermissions pubSubPerm = wampAuth.CraPermissionsMapper.LookupPubSubPermissions(resolvedTopicUri);
                if (pubSubPerm != null && pubSubPerm.pub)
                {
                    mPubSubServer.Publish(client, resolvedTopicUri, @event, exclude, eligible);
                }
            }
        }

        private WampCraAuthenticator<TMessage> GetOrCreateWampAuthenticatorForClient(IWampClient client)
        {

            if (!(client.CraAuthenticator is WampCraAuthenticator<TMessage> authenticator))
            {
                if (!mAuthFactory.IsValid)
                {
                    throw new InvalidOperationException("WampCraAuthenticaticatorBuilder is not valid.");
                }

                authenticator = mAuthFactory.BuildAuthenticator(client.SessionId);

                // Very important to give the client permissions to the auth APIs...
                foreach (IWampRpcMethod method in mWampCraProceduredMetadata.GetServiceMethods())
                {
                    authenticator.CraPermissionsMapper.AddRpcPermission(new WampRpcPermissions(method.ProcUri, true));
                }

                client.CraAuthenticator = authenticator;
            }

            return authenticator;
        }

        private static string ResolveUri(IWampClient client, string uri)
        {
            IWampCurieMapper mapper = client as IWampCurieMapper;
            return mapper.Resolve(uri);
        }
    }
}