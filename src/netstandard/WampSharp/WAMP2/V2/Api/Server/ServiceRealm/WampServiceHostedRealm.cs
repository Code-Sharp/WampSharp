using System;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;
using WampSharp.V2.Realm;
using WampSharp.V2.Rpc;

namespace WampSharp.V2
{
    internal class WampServiceHostedRealm : IWampHostedRealm
    {
        private readonly IWampHostedRealm mUnderlyingRealm;
        private readonly IWampChannel mInternalChannel;

        public WampServiceHostedRealm(IWampHostedRealm underlyingRealm, IWampChannel internalChannel, long sessionId)
        {
            mUnderlyingRealm = underlyingRealm;
            mInternalChannel = internalChannel;
            SessionId = sessionId;
        }

        public string Name => mUnderlyingRealm.Name;

        public IWampRpcOperationCatalog RpcCatalog => mUnderlyingRealm.RpcCatalog;

        public IWampTopicContainer TopicContainer => mUnderlyingRealm.TopicContainer;

        public event EventHandler<WampSessionCreatedEventArgs> SessionCreated
        {
            add => mUnderlyingRealm.SessionCreated += value;
            remove => mUnderlyingRealm.SessionCreated -= value;
        }

        public event EventHandler<WampSessionCloseEventArgs> SessionClosed
        {
            add => mUnderlyingRealm.SessionClosed += value;
            remove => mUnderlyingRealm.SessionClosed -= value;
        }
        
        public IWampRealmServiceProvider Services => mInternalChannel.RealmProxy.Services;

        public RouterRoles Roles => mUnderlyingRealm.Roles;

        public long SessionId { get; }
    }
}