using System;
using System.Reflection;
using Castle.DynamicProxy;
using WampSharp.Core.Contracts;

namespace WampSharp.Core.Proxy
{
    public class WampInterceptorSelector<TMessage> : IInterceptorSelector
    {
        private readonly WampOutgoingInterceptor<TMessage> mInterceptor;

        public WampInterceptorSelector(WampOutgoingInterceptor<TMessage> interceptor)
        {
            mInterceptor = interceptor;
        }

        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            if (method.IsDefined(typeof (WampHandlerAttribute)))
            {
                return new IInterceptor[] {mInterceptor};
            }

            return null;
        }
    }
}