using System;
using System.Collections.Concurrent;
using System.Reactive.Concurrency;
using WampSharp.V2.Realm;

namespace WampSharp.V2
{
    internal class ServiceHostedRealmContainer : IWampHostedRealmContainer
    {
        private readonly IWampHostedRealmContainer mRealmContainer;
        
        private readonly InMemoryWampHost mInternalHost;

        private readonly ConcurrentDictionary<string, IWampHostedRealm> mRealmNameToRealm =
            new ConcurrentDictionary<string, IWampHostedRealm>();

        public ServiceHostedRealmContainer(IWampHostedRealmContainer realmContainer,
            InMemoryWampHost internalHost)
        {
            mRealmContainer = realmContainer;
            mInternalHost = internalHost;
        }

        private IWampHostedRealm BuildRealm(string name)
        {
            IWampChannel channel = mInternalHost.CreateClientConnection(name, Scheduler.Immediate);

            long sessionId = 0;

            EventHandler<WampSessionCreatedEventArgs> connectionEstablished = 
                (sender, args) => sessionId = args.SessionId;
            
            channel.RealmProxy.Monitor.ConnectionEstablished += connectionEstablished;

            channel.Open();

            channel.RealmProxy.Monitor.ConnectionEstablished -= connectionEstablished;

            return new WampServiceHostedRealm
                (mRealmContainer.GetRealmByName(name),
                    channel, sessionId);
        }

        public IWampHostedRealm GetRealmByName(string name)
        {
            return mRealmNameToRealm.GetOrAdd(name,
                x => BuildRealm(name));
        }
    }
}