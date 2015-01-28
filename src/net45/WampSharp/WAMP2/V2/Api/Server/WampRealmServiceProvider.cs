using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using WampSharp.V2.CalleeProxy;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;
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

        // TODO: the options overloads don't work here, but its OK,
        // TODO: since this whole class is going to be obsolete soon.
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
        
        public Task RegisterCallee(object instance, RegisterOptions registerOptions)
        {
            return RegisterCallee(instance);
        }

        public TProxy GetCalleeProxy<TProxy>() where TProxy : class
        {
            return GetCalleeProxy<TProxy>(new CallOptions());
        }

        public TProxy GetCalleeProxy<TProxy>(CallOptions callOptions) where TProxy : class
        {
            return mCalleeProxyFactory.GetProxy<TProxy>(callOptions);
        }

        public ISubject<TEvent> GetSubject<TEvent>(string topicUri)
        {
            IWampSubject wampSubject = GetSubject(topicUri);

            WampTopicSubject<TEvent> result =
                new WampTopicSubject<TEvent>(wampSubject);

            return result;
        }

        public IWampSubject GetSubject(string topicUri)
        {
            IWampTopicContainer wampTopicContainer = mRealm.TopicContainer;

            WampRouterSubject result = 
                new WampRouterSubject(topicUri, wampTopicContainer);

            return result;
        }
    }
}