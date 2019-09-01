#if CASTLE || DISPATCH_PROXY
using System;
using System.Reflection;
using System.Threading.Tasks;
using WampSharp.V2.ReflectionDispatchProxy;
using WampSharp.V2.Rpc;
using TaskExtensions = WampSharp.Core.Utilities.TaskExtensions;

namespace WampSharp.V2.CalleeProxy
{
    internal static class CalleeProxyInterceptorFactory
    {
        public static ICalleeProxyInvocationInterceptor BuildInterceptor(MethodInfo method, ICalleeProxyInterceptor interceptor, WampCalleeProxyInvocationHandler handler)
        {
            Type interceptorType = GetRelevantInterceptorType(method);

            ICalleeProxyInvocationInterceptor result =
                (ICalleeProxyInvocationInterceptor)
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
                MethodInfoValidation.ValidateSyncMethod(method);

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