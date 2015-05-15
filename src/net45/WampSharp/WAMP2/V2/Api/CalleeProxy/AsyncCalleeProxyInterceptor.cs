#if CASTLE || DNX
using System.Reflection;
using System.Threading.Tasks;

namespace WampSharp.V2.CalleeProxy
{
    internal class AsyncCalleeProxyInterceptor<TResult> : CalleeProxyInterceptorBase<TResult>
    {
        public AsyncCalleeProxyInterceptor(MethodInfo method, IWampCalleeProxyInvocationHandler handler, ICalleeProxyInterceptor interceptor) : 
            base(method, handler, interceptor)
        {
        }

        public override object Invoke(MethodInfo method, object[] arguments)
        {
            Task result =
                Handler.InvokeAsync<TResult>(Interceptor, method, Extractor, arguments);

            return result;
        }
    }
}
#endif