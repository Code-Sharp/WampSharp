#if CASTLE
﻿using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using WampSharp.V2.Rpc;
using TaskExtensions = WampSharp.Core.Utilities.TaskExtensions;

namespace WampSharp.V2.CalleeProxy
{
    internal class WampCalleeProxyFactory : IWampCalleeProxyFactory
    {
        private readonly ProxyGenerator mGenerator = new ProxyGenerator();
        private readonly WampCalleeProxyInvocationHandler mHandler;

        public WampCalleeProxyFactory(WampCalleeProxyInvocationHandler handler)
        {
            mHandler = handler;
        }

        public virtual TProxy GetProxy<TProxy>(ICalleeProxyInterceptor interceptor) where TProxy : class
        {
            ProxyGenerationOptions options = new ProxyGenerationOptions()
            {
                Selector = new WampCalleeProxyInterceptorSelector()
            };

            IInterceptor[] interceptors =
                typeof (TProxy).GetMethods()
                    .Select(method => BuildInterceptor(method, interceptor))
                    .ToArray();

            TProxy proxy =
                mGenerator.CreateInterfaceProxyWithoutTarget<TProxy>(options, interceptors);

            return proxy;
        }

        private IInterceptor BuildInterceptor(MethodInfo method, ICalleeProxyInterceptor interceptor)
        {
            Type interceptorType =
                GetRelevantInterceptorType(method);

            IInterceptor result =
                (IInterceptor)
                    Activator.CreateInstance(interceptorType,
                        method,
                        mHandler,
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

#if !NET40
                if (method.IsDefined(typeof(WampProgressiveResultProcedureAttribute)))
                {
                    MethodInfoValidation.ValidateProgressiveMethod(method);
                    interceptorType = typeof(ProgressiveAsyncCalleeProxyInterceptor<>);
                }
                else
#endif
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