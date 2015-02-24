using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using SystemEx;
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

        public WampRealmProxyServiceProvider(IWampRealmProxy proxy)
        {
            mProxy = proxy;
            mCalleeProxyFactory = new WampCalleeClientProxyFactory
                (mProxy.RpcCatalog,
                 mProxy.Monitor);
        }

        public Task<IAsyncDisposable> RegisterCallee(object instance)
        {
            return RegisterCallee(instance, CalleeRegistrationInterceptor.Default);
        }

        public Task<IAsyncDisposable> RegisterCallee(object instance, ICalleeRegistrationInterceptor interceptor)
        {
            IEnumerable<OperationToRegister> operationsToRegister =
                mExtractor.ExtractOperations(instance, interceptor);

            List<Task<IAsyncDisposable>> registrations = 
                new List<Task<IAsyncDisposable>>();

            foreach (OperationToRegister operationToRegister in operationsToRegister)
            {
                Task<IAsyncDisposable> task = 
                    mProxy.RpcCatalog.Register(operationToRegister.Operation, operationToRegister.Options);

                registrations.Add(task);
            }

            return registrations.ToAsyncDisposableTask();
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