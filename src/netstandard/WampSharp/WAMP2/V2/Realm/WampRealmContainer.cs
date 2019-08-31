using System.Collections.Concurrent;
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
}