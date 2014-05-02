using System.Collections.Concurrent;
using WampSharp.V2.Binding;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Core.Listener;
using WampSharp.V2.PubSub;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.Realm
{
    public class WampRealmContainer : IWampRealmContainer
    {
        private readonly ConcurrentDictionary<string, IWampRealm> mRealmNameToRealm =
            new ConcurrentDictionary<string, IWampRealm>();

        public IWampRealm GetRealmByName(string name)
        {
            return mRealmNameToRealm.GetOrAdd(name, realmName => CreateRealm(realmName));
        }

        private IWampRealm CreateRealm(string realmName)
        {
            WampRealm result =
                new WampRealm(realmName,
                              new WampRpcOperationCatalog(),
                              new WampTopicContainer());

            return result;
        }
    }

    // TODO: Rename this and IWampRealmContainer<TMessage>.
    // TODO: Think about a suitable name.
    public class WampRealmContainer<TMessage> : IWampRealmContainer<TMessage>
    {
        private readonly IWampRealmContainer mRealmContainer;
        private readonly IWampSessionServer<TMessage> mSession;
        private readonly IWampEventSerializer<TMessage> mEventSerializer;
        private readonly IWampBinding<TMessage> mBinding;

        private readonly ConcurrentDictionary<string, IWampRealm<TMessage>> mRealmNameToRealm =
            new ConcurrentDictionary<string, IWampRealm<TMessage>>();


        public WampRealmContainer(IWampRealmContainer realmContainer,
                                  IWampSessionServer<TMessage> session,
                                  IWampEventSerializer<TMessage> eventSerializer,
                                  IWampBinding<TMessage> binding)
        {
            mSession = session;
            mEventSerializer = eventSerializer;
            mBinding = binding;
            mRealmContainer = realmContainer;
        }

        public IWampRealm<TMessage> GetRealmByName(string name)
        {
            return mRealmNameToRealm.GetOrAdd(name, realmName => CreateRealm(realmName));
        }

        private IWampRealm<TMessage> CreateRealm(string realmName)
        {
            IWampRealm realm = mRealmContainer.GetRealmByName(realmName);

            return new WampRealm<TMessage>(realm,
                                           mSession,
                                           mEventSerializer,
                                           mBinding);
        }
    }
}