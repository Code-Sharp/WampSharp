using Castle.DynamicProxy;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.CalleeProxy
{
    internal class SyncCalleeProxyInterceptor : IInterceptor
    {
        private readonly IWampCalleeProxyInvocationHandler mHandler;
        private readonly ICalleeProxyInterceptor mInterceptor;

        public SyncCalleeProxyInterceptor(IWampCalleeProxyInvocationHandler handler, ICalleeProxyInterceptor interceptor)
        {
            mHandler = handler;
            mInterceptor = interceptor;
        }

        public void Intercept(IInvocation invocation)
        {
            object result =
                mHandler.Invoke(mInterceptor, invocation.Method, invocation.Arguments);

            invocation.ReturnValue = result;
        }
    }
}