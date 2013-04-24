using System;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using WampSharp.Core.Contracts;
using WampSharp.Core.Listener;

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
            // In case you were wondering, this is how a patch looks like.
            else if (method.IsSpecialName &&
                     method.Name == "get_SessionId")
            {
                return interceptors.OfType<SessionIdPropertyInterceptor>()
                                   .Cast<IInterceptor>()
                                   .ToArray();
            }

            return null;
        }
    }
}