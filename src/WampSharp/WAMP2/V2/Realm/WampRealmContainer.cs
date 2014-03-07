using System.Collections.Concurrent;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Core.Listener;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.Realm
{
    public class WampRealmContainer<TMessage> : IWampRealmContainer<TMessage> where TMessage : class
    {
        private readonly IWampSessionServer<TMessage> mSession;
        private readonly IWampEventSerializer<TMessage> mEventSerializer;
        private readonly IWampBinding<TMessage> mBinding;

        private ConcurrentDictionary<string, IWampRealm<TMessage>> mRealmNameToRealm =
            new ConcurrentDictionary<string, IWampRealm<TMessage>>();


        public WampRealmContainer(IWampSessionServer<TMessage> session,
                                  IWampEventSerializer<TMessage> eventSerializer,
                                  IWampBinding<TMessage> binding)
        {
            mSession = session;
            mEventSerializer = eventSerializer;
            mBinding = binding;
        }

        public IWampRealm<TMessage> GetRealmByName(string name)
        {
            return mRealmNameToRealm.GetOrAdd(name, realmName => CreateRealm(realmName));
        }

        private IWampRealm<TMessage> CreateRealm(string realmName)
        {
            WampRealm<TMessage> result =
                new WampRealm<TMessage>(realmName,
                                        new WampRpcOperationCatalog<TMessage>(),
                                        null,
                                        mSession,
                                        mEventSerializer,
                                        mBinding);

            return result;
        }
    }
}