using System.Reflection;

namespace WampSharp.V2.CalleeProxy
{
    internal interface ICalleeProxyInvocationInterceptor
    {
        object Invoke(MethodInfo method, object[] arguments);
    }
}