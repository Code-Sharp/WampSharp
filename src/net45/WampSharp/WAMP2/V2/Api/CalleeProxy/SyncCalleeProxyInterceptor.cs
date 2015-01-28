using Castle.DynamicProxy;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.CalleeProxy
{
    internal class SyncCalleeProxyInterceptor : IInterceptor
    {
        private readonly IWampCalleeProxyInvocationHandler mHandler;
        private readonly CallOptions mCallOptions;

        public SyncCalleeProxyInterceptor(IWampCalleeProxyInvocationHandler handler, CallOptions callOptions)
        {
            mHandler = handler;
            mCallOptions = callOptions;
        }

        public void Intercept(IInvocation invocation)
        {
            object result =
                mHandler.Invoke(mCallOptions, invocation.Method, invocation.Arguments);

            invocation.ReturnValue = result;
        }
    }
}