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
    internal class WampCalleeProxyInterceptorSelector :
        IInterceptorSelector
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

            if (typeof(Task).IsAssignableFrom(returnType))
            {
#if !NET40
                if (method.IsDefined(typeof(WampProgressiveResultProcedureAttribute)))
                {
                    Type taskType = TaskExtensions.UnwrapReturnType(returnType);

                    IInterceptor interceptor =
                        (IInterceptor)
                            Activator.CreateInstance(typeof (ProgressiveAsyncCalleeProxyInterceptor<>)
                                .MakeGenericType(taskType),
                                mHandler, mInterceptor);

                    return new IInterceptor[] {interceptor};
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