using System;
using System.Reflection;
using Castle.DynamicProxy;
using WampSharp.Core.Utilities;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.CalleeProxy
{
    internal abstract class CalleeProxyInterceptorBase<TResult> : IInterceptor
    {
        private readonly MethodInfo mMethod;
        private readonly IWampCalleeProxyInvocationHandler mHandler;
        private readonly ICalleeProxyInterceptor mInterceptor;
        private readonly IOperationResultExtractor<TResult> mExtractor;

        public CalleeProxyInterceptorBase(MethodInfo method, IWampCalleeProxyInvocationHandler handler, ICalleeProxyInterceptor interceptor)
        {
            mMethod = method;
            mHandler = handler;
            mInterceptor = interceptor;
            mExtractor = GetOperationResultExtractor<TResult>(method);
        }

        public IOperationResultExtractor<TResult> Extractor
        {
            get
            {
                return mExtractor;
            }
        }

        public ICalleeProxyInterceptor Interceptor
        {
            get
            {
                return mInterceptor;
            }
        }

        public IWampCalleeProxyInvocationHandler Handler
        {
            get
            {
                return mHandler;
            }
        }

        public MethodInfo Method
        {
            get
            {
                return mMethod;
            }
        }

        public abstract void Intercept(IInvocation invocation);

        private static IOperationResultExtractor<T> GetOperationResultExtractor<T>(MethodInfo method)
        {
            IOperationResultExtractor<T> extractor;

            if (!method.HasMultivaluedResult())
            {
                bool hasReturnValue = method.HasReturnValue();
                extractor = new SingleValueExtractor<T>(hasReturnValue);
            }
            else
            {
                Type elementType = typeof(T).GetElementType();

                Type extractorType =
                    typeof(MultiValueExtractor<>).MakeGenericType(elementType);

                extractor =
                    (IOperationResultExtractor<T>)Activator.CreateInstance(extractorType);
            }

            return extractor;
        }
    }
}