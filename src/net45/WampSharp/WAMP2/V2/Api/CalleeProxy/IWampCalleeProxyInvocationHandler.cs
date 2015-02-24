using System;
using System.Reflection;
using System.Threading.Tasks;

namespace WampSharp.V2.CalleeProxy
{
    internal interface IWampCalleeProxyInvocationHandler
    {
        object Invoke(ICalleeProxyInterceptor interceptor, MethodInfo method, object[] arguments);
        Task InvokeAsync(ICalleeProxyInterceptor interceptor, MethodInfo method, object[] arguments);

#if !NET40
        Task InvokeProgressiveAsync<T>(ICalleeProxyInterceptor interceptor, MethodInfo method, object[] arguments, IProgress<T> progress);
#endif
    }
}