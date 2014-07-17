using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using WampSharp.V2.CalleeProxy;
using WampSharp.V2.Realm;
using WampSharp.V2.Rpc;

namespace WampSharp.V2
{
    internal class WampRealmServiceProvider : IWampRealmServiceProvider
    {
        private readonly IOperationExtractor mExtractor = 
            new OperationExtractor();
        
        private readonly IWampRealm mRealm;
        
        private readonly IWampCalleeProxyFactory mCalleeProxyFactory;

        public WampRealmServiceProvider(IWampRealm realm)
        {
            mRealm = realm;
            mCalleeProxyFactory = new WampCalleeRouterProxyFactory(mRealm.RpcCatalog);
        }

        public Task RegisterCallee(object instance)
        {
            IEnumerable<IWampRpcOperation> operations =
                mExtractor.ExtractOperations(instance);

            foreach (IWampRpcOperation operation in operations)
            {
                mRealm.RpcCatalog.Register(operation);
            }

            TaskCompletionSource<bool> result = new TaskCompletionSource<bool>();
            result.SetResult(true);
            return result.Task;
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