using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using WampSharp.V2.CalleeProxy;
using WampSharp.V2.Client;
using WampSharp.V2.Rpc;

namespace WampSharp.V2
{
    internal class WampRealmProxyServiceProvider : IWampRealmServiceProvider
    {
        private readonly IOperationExtractor mExtractor =
            new OperationExtractor();

        private readonly IWampRealmProxy mProxy;
        private readonly WampCalleeClientProxyFactory mCalleeProxyFactory;
        private readonly IDictionary<string, object> EmptyOptions = 
            new Dictionary<string, object>();

        public WampRealmProxyServiceProvider(IWampRealmProxy proxy)
        {
            mProxy = proxy;
            mCalleeProxyFactory = new WampCalleeClientProxyFactory(mProxy.RpcCatalog);
        }

        public Task RegisterCallee(object instance)
        {
            IEnumerable<IWampRpcOperation> operations =
                mExtractor.ExtractOperations(instance);

            List<Task> registrations = new List<Task>();

            foreach (IWampRpcOperation operation in operations)
            {
                Task task =
                    mProxy.RpcCatalog.Register(operation, EmptyOptions);

                registrations.Add(task);
            }

#if !NET40
            return Task.WhenAll(registrations);
#elif
            // TODO: Implement a framework 4 version.
            return null;
#endif
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