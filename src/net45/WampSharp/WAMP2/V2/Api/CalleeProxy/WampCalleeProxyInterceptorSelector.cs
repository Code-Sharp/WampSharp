using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.CalleeProxy
{
    internal class WampCalleeProxyInterceptorSelector :
        IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            Type returnType = method.ReturnType;

            if (typeof(Task).IsAssignableFrom(returnType))
            {
#if !NET40
                if (method.IsDefined(typeof(WampProgressiveResultProcedureAttribute)))
                {
                    // TODO: Throw an exception if the method signature isn't suitable 
                    // TODO: (i.e. the last parameter isn't IProgress<TResult>).
                    return interceptors.OfType<ProgressiveAsyncCalleeProxyInterceptor>().Cast<IInterceptor>().ToArray();
                }
                else
#endif
                {
                    return interceptors.OfType<AsyncCalleeProxyInterceptor>().Cast<IInterceptor>().ToArray();
                }
            }

            return interceptors.OfType<SyncCalleeProxyInterceptor>().Cast<IInterceptor>().ToArray();
        }
    }
}