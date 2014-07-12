using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace WampSharp.V2.CalleeProxy
{
    internal class AsyncCalleeProxyInterceptor : IInterceptor
    {
        private readonly IWampCalleeProxyInvocationHandler mHandler;

        public AsyncCalleeProxyInterceptor(IWampCalleeProxyInvocationHandler handler)
        {
            mHandler = handler;
        }

        public void Intercept(IInvocation invocation)
        {
            Task result =
                mHandler.InvokeAsync(invocation.Method, invocation.Arguments);

            invocation.ReturnValue = result;
        }
    }
}