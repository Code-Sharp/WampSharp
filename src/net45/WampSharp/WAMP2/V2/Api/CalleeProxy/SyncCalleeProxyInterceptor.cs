#if CASTLE
using System.Reflection;
using Castle.DynamicProxy;

namespace WampSharp.V2.CalleeProxy
{
    internal class SyncCalleeProxyInterceptor<TResult> : CalleeProxyInterceptorBase<TResult>
    {
        public SyncCalleeProxyInterceptor(MethodInfo method, IWampCalleeProxyInvocationHandler handler, ICalleeProxyInterceptor interceptor) : base(method, handler, interceptor)
        {
        }

        public override void Intercept(IInvocation invocation)
        {
            TResult result =
                Handler.Invoke<TResult>(Interceptor, Method, Extractor, invocation.Arguments);

            invocation.ReturnValue = result;
        }
    }
}
#endif