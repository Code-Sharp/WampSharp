using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace WampSharp.V2.CalleeProxy
{
    internal class WampCalleProxyInterceptorSelector :
        IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            Type returnType = method.ReturnType;

            if (typeof(Task).IsAssignableFrom(returnType))
            {
                return interceptors.OfType<SyncCalleeProxyInterceptor>().Cast<IInterceptor>().ToArray();
            }

            return interceptors.OfType<AsyncCalleeProxyInterceptor>().Cast<IInterceptor>().ToArray();
        }
    }
}