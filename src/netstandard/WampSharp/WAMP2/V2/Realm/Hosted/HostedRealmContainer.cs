using System.Collections.Concurrent;

namespace WampSharp.V2.Realm
{
    public class HostedRealmContainer : IWampHostedRealmContainer
    {
        private readonly ConcurrentDictionary<string, IWampHostedRealm> mRealmNameToRealm =
            new ConcurrentDictionary<string, IWampHostedRealm>();
 
        private readonly IWampRealmContainer mRealmContainer;

        public HostedRealmContainer(IWampRealmContainer realmContainer)
        {
            mRealmContainer = realmContainer;
        }

        public IWampHostedRealm GetRealmByName(string name)
        {
            return mRealmNameToRealm.GetOrAdd(name,
                                              x => BuildRealm(name));
        }

        private IWampHostedRealm BuildRealm(string name)
        {
            IWampRealm realRealm = mRealmContainer.GetRealmByName(name);
            WampHostedRealm decorated = new WampHostedRealm(realRealm);
            return decorated;
        }
    }
}