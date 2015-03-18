using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;
using TaskExtensions = WampSharp.Core.Utilities.TaskExtensions;

namespace WampSharp.V2.CalleeProxy
{
    internal class WampCalleeProxyInterceptorSelector : IInterceptorSelector
    {
        private readonly IWampCalleeProxyInvocationHandler mHandler;
        private readonly ICalleeProxyInterceptor mInterceptor;

        public WampCalleeProxyInterceptorSelector(IWampCalleeProxyInvocationHandler handler, ICalleeProxyInterceptor interceptor)
        {
            mHandler = handler;
            mInterceptor = interceptor;
        }

        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            Type returnType = method.ReturnType;
            Type genericArgument;
            Type interceptorType;

            if (!typeof (Task).IsAssignableFrom(returnType))
            {
                genericArgument = returnType == typeof (void) ? typeof(object) : returnType;
                interceptorType = typeof (SyncCalleeProxyInterceptor<>);
            }
            else
            {
                genericArgument = TaskExtensions.UnwrapReturnType(returnType);

#if !NET40
                if (method.IsDefined(typeof (WampProgressiveResultProcedureAttribute)))
                {
                    MethodInfoValidation.ValidateProgressiveMethod(method);
                    interceptorType = typeof (ProgressiveAsyncCalleeProxyInterceptor<>);
                }
                else
#endif
                {
                    MethodInfoValidation.ValidateAsyncMethod(method);
                    interceptorType = typeof (AsyncCalleeProxyInterceptor<>);
                }
            }

            IInterceptor interceptor =
                (IInterceptor)
                    Activator.CreateInstance(
                        interceptorType.MakeGenericType(genericArgument),
                        method, mHandler, mInterceptor);

            return new IInterceptor[] {interceptor};
        }
    }
}