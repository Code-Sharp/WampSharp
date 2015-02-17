using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
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

        public WampRealmProxyServiceProvider(IWampRealmProxy proxy)
        {
            mProxy = proxy;
            mCalleeProxyFactory = new WampCalleeClientProxyFactory
                (mProxy.RpcCatalog,
                 mProxy.Monitor);
        }

        public Task RegisterCallee(object instance)
        {
            return RegisterCallee(instance, CalleeRegistrationInterceptor.Default);
        }

        public Task UnregisterCallee(object instance)
        {
            return UnregisterCallee(instance, CalleeRegistrationInterceptor.Default);
        }

        public Task RegisterCallee(object instance, ICalleeRegistrationInterceptor interceptor)
        {
            Task result =
                CalleeAggregatedCall(instance, interceptor,
                    (operation, options) => mProxy.RpcCatalog.Register(operation, options));
            
            return result;
        }

        public Task UnregisterCallee(object instance, ICalleeRegistrationInterceptor interceptor)
        {
            Task result = CalleeAggregatedCall(instance, interceptor,
                (operation, options) => mProxy.RpcCatalog.Unregister(operation));

            return result;
        }

        private Task CalleeAggregatedCall(object instance, ICalleeRegistrationInterceptor interceptor, Func<IWampRpcOperation, RegisterOptions, Task> action)
        {
            IEnumerable<OperationToRegister> operationsToRegister =
                mExtractor.ExtractOperations(instance, interceptor);

            List<Task> registrations = new List<Task>();

            foreach (OperationToRegister operationToRegister in operationsToRegister)
            {
                IWampRpcOperation operation = operationToRegister.Operation;
                RegisterOptions options = operationToRegister.Options;

                Task task = action(operation, options);

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
            return mCalleeProxyFactory.GetProxy<TProxy>(CalleeProxyInterceptor.Default);
        }

        public TProxy GetCalleeProxy<TProxy>(ICalleeProxyInterceptor interceptor) where TProxy : class
        {
            return mCalleeProxyFactory.GetProxy<TProxy>(interceptor);
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