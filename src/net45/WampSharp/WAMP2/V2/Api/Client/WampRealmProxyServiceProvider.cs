using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using WampSharp.V2.CalleeProxy;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;

namespace WampSharp.V2
{
    internal class WampRealmProxyServiceProvider : IWampRealmServiceProvider
    {
        private readonly IOperationExtractor mExtractor =
            new OperationExtractor();

        private readonly IWampRealmProxy mProxy;
        private readonly WampCalleeClientProxyFactory mCalleeProxyFactory;
        private readonly RegisterOptions EmptyOptions = 
            new RegisterOptions();

        public WampRealmProxyServiceProvider(IWampRealmProxy proxy)
        {
            mProxy = proxy;
            mCalleeProxyFactory = new WampCalleeClientProxyFactory
                (mProxy.RpcCatalog,
                 mProxy.Monitor);
        }

        public Task RegisterCallee(object instance)
        {
            return RegisterCallee(EmptyOptions);
        }

        public Task RegisterCallee(object instance, RegisterOptions registerOptions)
        {
            IEnumerable<IWampRpcOperation> operations =
                mExtractor.ExtractOperations(instance);

            List<Task> registrations = new List<Task>();

            foreach (IWampRpcOperation operation in operations)
            {
                Task task =
                    mProxy.RpcCatalog.Register(operation, registerOptions);

                registrations.Add(task);
            }

#if !NET40
            return Task.WhenAll(registrations);
#else
            IEnumerable<IObservable<Unit>> tasksAsObservables = 
                registrations.Select(x => x.ToObservable());

            IObservable<Unit> merged = tasksAsObservables.Merge();

            Task<Unit> result = merged.ToTask();

            return result;
#endif
        }

        public TProxy GetCalleeProxy<TProxy>() where TProxy : class
        {
            return mCalleeProxyFactory.GetProxy<TProxy>(new CallOptions());
        }

        public TProxy GetCalleeProxy<TProxy>(CallOptions callOptions) where TProxy : class
        {
            return mCalleeProxyFactory.GetProxy<TProxy>(callOptions);
        }

        public ISubject<TEvent> GetSubject<TEvent>(string topicUri)
        {
            IWampSubject subject = GetSubject(topicUri);

            WampTopicSubject<TEvent> result = new WampTopicSubject<TEvent>(subject);

            return result;
        }

        public IWampSubject GetSubject(string topicUri)
        {
            IWampTopicProxy topicProxy = mProxy.TopicContainer.GetTopicByUri(topicUri);

            WampClientSubject result = new WampClientSubject(topicProxy, mProxy.Monitor);

            return result;
        }
    }
}