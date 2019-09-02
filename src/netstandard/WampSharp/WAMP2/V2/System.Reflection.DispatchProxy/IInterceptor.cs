using System.Reflection;

namespace WampSharp.V2.ReflectionDispatchProxy
{
    internal interface ICalleeProxyInvocationInterceptor
    {
        object Invoke(MethodInfo method, object[] arguments);
    }
}