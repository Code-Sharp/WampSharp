using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace WampSharp.V2.CalleeProxy
{
    internal class AsyncCalleeProxyInterceptor<TResult> : CalleeProxyInterceptorBase<TResult>
    {
        public AsyncCalleeProxyInterceptor(MethodInfo method, IWampCalleeProxyInvocationHandler handler, ICalleeProxyInterceptor interceptor) : 
            base(method, handler, interceptor)
        {
        }

        public override void Intercept(IInvocation invocation)
        {
            MethodInfo method = invocation.Method;

            Task result =
                Handler.InvokeAsync<TResult>(Interceptor, method, Extractor, invocation.Arguments);

            invocation.ReturnValue = result;
        }
    }
}