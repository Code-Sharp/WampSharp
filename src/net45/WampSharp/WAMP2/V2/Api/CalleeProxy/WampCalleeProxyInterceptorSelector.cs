using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;

namespace WampSharp.V2.CalleeProxy
{
    internal class WampCalleeProxyInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            IEnumerable<IInterceptor> relevantInterceptors =
                interceptors.OfType<CalleeProxyInterceptorBase>()
                    .Where(x => x.Method == method);

            IInterceptor[] result = relevantInterceptors.ToArray();

            return result;
        }
    }
}