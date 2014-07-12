using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using WampSharp.V2.CalleeProxy;
using WampSharp.V2.Client;

namespace WampSharp.V2
{
    internal class WampRealmProxyServiceProvider : IWampRealmServiceProvider
    {
        private readonly IWampRealmProxy mProxy;
        private readonly WampCalleeClientProxyFactory mCalleeProxyFactory;

        public WampRealmProxyServiceProvider(IWampRealmProxy proxy)
        {
            mProxy = proxy;
            mCalleeProxyFactory = new WampCalleeClientProxyFactory(mProxy.RpcCatalog);
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
            return mProxy.TopicContainer.GetTopicByUri(topicUri).ToSubject<TEvent>();
        }

        public IWampSubject GetSubject(string topicUri)
        {
            return mProxy.TopicContainer.GetTopicByUri(topicUri).ToSubject();
        }
    }
}