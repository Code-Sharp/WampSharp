#if !NET40
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.CalleeProxy
{
    internal class ProgressiveAsyncCalleeProxyInterceptor<T> : IInterceptor
    {
        private readonly IWampCalleeProxyInvocationHandler mHandler;
        private readonly ICalleeProxyInterceptor mInterceptor;

        public ProgressiveAsyncCalleeProxyInterceptor(IWampCalleeProxyInvocationHandler handler, ICalleeProxyInterceptor interceptor)
        {
            mHandler = handler;
            mInterceptor = interceptor;
        }

        public void Intercept(IInvocation invocation)
        {
            object[] arguments = invocation.Arguments;
            
            object[] argumentsWithoutProgress = new object[arguments.Length - 1];

            Array.Copy(arguments, argumentsWithoutProgress, argumentsWithoutProgress.Length);

            IProgress<T> progress = arguments.Last() as IProgress<T>;

            Task result =
                mHandler.InvokeProgressiveAsync
                    (mInterceptor, invocation.Method, argumentsWithoutProgress, progress);

            invocation.ReturnValue = result;
        }
    }
}
#endif