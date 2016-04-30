#if CASTLE || DISPATCH_PROXY

using System;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using WampSharp.V2.Rpc;
using TaskExtensions = WampSharp.Core.Utilities.TaskExtensions;

namespace WampSharp.V2.CalleeProxy
{
    internal static class CalleeProxyInterceptorFactory
    {
        public static IInterceptor BuildInterceptor(MethodInfo method, ICalleeProxyInterceptor interceptor, WampCalleeProxyInvocationHandler handler)
        {
            Type interceptorType = GetRelevantInterceptorType(method);

            IInterceptor result =
                (IInterceptor)
                    Activator.CreateInstance(interceptorType,
                                             method,
                                             handler,
                                             interceptor);

            return result;
        }

        private static Type GetRelevantInterceptorType(MethodInfo method)
        {
            Type returnType = method.ReturnType;
            Type genericArgument;
            Type interceptorType;

            if (!typeof(Task).IsAssignableFrom(returnType))
            {
                genericArgument = returnType == typeof(void) ? typeof(object) : returnType;
                interceptorType = typeof(SyncCalleeProxyInterceptor<>);
            }
            else
            {
                genericArgument = TaskExtensions.UnwrapReturnType(returnType);

                if (method.IsDefined(typeof(WampProgressiveResultProcedureAttribute)))
                {
                    MethodInfoValidation.ValidateProgressiveMethod(method);
                    interceptorType = typeof(ProgressiveAsyncCalleeProxyInterceptor<>);
                }
                else
                {
                    MethodInfoValidation.ValidateAsyncMethod(method);
                    interceptorType = typeof(AsyncCalleeProxyInterceptor<>);
                }
            }

            Type closedGenericType = interceptorType.MakeGenericType(genericArgument);
            return closedGenericType;
        }
    }
}

#endif