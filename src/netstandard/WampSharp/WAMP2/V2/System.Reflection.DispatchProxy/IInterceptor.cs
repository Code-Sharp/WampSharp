#if DISPATCH_PROXY

using System.Reflection;

namespace WampSharp.V2.System.Reflection.DispatchProxy
{
    internal interface ICalleeProxyInvocationInterceptor
    {
        object Invoke(MethodInfo method, object[] arguments);
    }
}

#endif