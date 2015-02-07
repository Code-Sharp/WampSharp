using System.Threading.Tasks;
using Castle.DynamicProxy;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.CalleeProxy
{
    internal class AsyncCalleeProxyInterceptor : IInterceptor
    {
        private readonly IWampCalleeProxyInvocationHandler mHandler;
        private readonly CallOptions mCallOptions;

        public AsyncCalleeProxyInterceptor(IWampCalleeProxyInvocationHandler handler, CallOptions callOptions)
        {
            mHandler = handler;
            mCallOptions = callOptions;
        }

        public void Intercept(IInvocation invocation)
        {
            Task result =
                mHandler.InvokeAsync(mCallOptions, invocation.Method, invocation.Arguments);

            invocation.ReturnValue = result;
        }
    }
}