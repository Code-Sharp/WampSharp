using System.Reflection;

namespace WampSharp.V2.CalleeProxy
{
    internal class SyncCalleeProxyInterceptor<TResult> : CalleeProxyInterceptorBase<TResult>
    {
        public SyncCalleeProxyInterceptor(MethodInfo method, IWampCalleeProxyInvocationHandler handler, ICalleeProxyInterceptor interceptor) : base(method, handler, interceptor)
        {
        }

        public override object Invoke(MethodInfo method, object[] arguments)
        {
            TResult result =
                Handler.Invoke<TResult>(Interceptor, Method, ResultExtractor, arguments);

            return result;
        }
    }
}