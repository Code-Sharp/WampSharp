#if DISPATCH_PROXY

using System.Reflection;

// ReSharper disable once CheckNamespace
namespace Castle.DynamicProxy
{
    internal interface IInterceptor
    {
        object Invoke(MethodInfo method, object[] arguments);
    }
}

#endif