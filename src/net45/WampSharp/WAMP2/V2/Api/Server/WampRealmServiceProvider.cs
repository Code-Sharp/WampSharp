using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using WampSharp.V2.CalleeProxy;
using WampSharp.V2.Realm;

namespace WampSharp.V2
{
    internal class WampRealmServiceProvider : IWampRealmServiceProvider
    {
        private readonly IWampRealm mRealm;
        private readonly IWampCalleeProxyFactory mCalleeProxyFactory;

        public WampRealmServiceProvider(IWampRealm realm)
        {
            mRealm = realm;
            mCalleeProxyFactory = new WampCalleeRouterProxyFactory(mRealm.RpcCatalog);
        }

        public Task RegisterCallee(object instance)
        {
            throw new NotImplementedException();
        }

        public TProxy GetCalleeProxy<TProxy>() where TProxy : class
        {
            return mCalleeProxyFactory.GetProxy<TProxy>();
        }

        public ISubject<TEvent> GetSubject<TEvent>(string topicUri)
        {
            return mRealm.TopicContainer.GetTopicByUri(topicUri).ToSubject<TEvent>();
        }

        public IWampSubject GetSubject(string topicUri)
        {
            return mRealm.TopicContainer.GetTopicByUri(topicUri).ToSubject();
        }
    }
}