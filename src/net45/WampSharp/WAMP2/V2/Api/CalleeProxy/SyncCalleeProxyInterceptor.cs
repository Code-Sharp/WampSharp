using Castle.DynamicProxy;

namespace WampSharp.V2.CalleeProxy
{
    internal class SyncCalleeProxyInterceptor : IInterceptor
    {
        private readonly IWampCalleeProxyInvocationHandler mHandler;

        public SyncCalleeProxyInterceptor(IWampCalleeProxyInvocationHandler handler)
        {
            mHandler = handler;
        }

        public void Intercept(IInvocation invocation)
        {
            object result =
                mHandler.Invoke(invocation.Method, invocation.Arguments);

            invocation.ReturnValue = result;
        }
    }
}