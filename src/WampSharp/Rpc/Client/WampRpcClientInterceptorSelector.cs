using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace WampSharp.Rpc.Client
{
    public class WampRpcClientInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            if (typeof (Task).IsAssignableFrom(method.ReturnType))
            {
                return interceptors.OfType<WampRpcClientAsyncInterceptor>().Cast<IInterceptor>().ToArray();
            }

            return interceptors.OfType<WampRpcClientSyncInterceptor>().Cast<IInterceptor>().ToArray();
        }
    }
}