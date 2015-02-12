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
        private readonly CallOptions mCallOptions;

        public ProgressiveAsyncCalleeProxyInterceptor(IWampCalleeProxyInvocationHandler handler, CallOptions callOptions)
        {
            mHandler = handler;
            mCallOptions = callOptions;
        }

        public void Intercept(IInvocation invocation)
        {
            CallOptions options = new CallOptions(mCallOptions) {ReceiveProgress = true};

            object[] arguments = invocation.Arguments;
            
            object[] argumentsWithoutProgress = new object[arguments.Length - 1];

            Array.Copy(arguments, argumentsWithoutProgress, argumentsWithoutProgress.Length);

            IProgress<T> progress = arguments.Last() as IProgress<T>;

            Task result =
                mHandler.InvokeProgressiveAsync
                    (options, invocation.Method, argumentsWithoutProgress, progress);

            invocation.ReturnValue = result;
        }
    }
}
#endif