using System.Threading.Tasks;
using Castle.DynamicProxy;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.CalleeProxy
{
    internal class AsyncCalleeProxyInterceptor : IInterceptor
    {
        private readonly IWampCalleeProxyInvocationHandler mHandler;
        private readonly ICalleeProxyInterceptor mInterceptor;

        public AsyncCalleeProxyInterceptor(IWampCalleeProxyInvocationHandler handler, ICalleeProxyInterceptor interceptor)
        {
            mHandler = handler;
            mInterceptor = interceptor;
        }

        public void Intercept(IInvocation invocation)
        {
            Task result =
                mHandler.InvokeAsync(mInterceptor, invocation.Method, invocation.Arguments);

            invocation.ReturnValue = result;
        }
    }
}