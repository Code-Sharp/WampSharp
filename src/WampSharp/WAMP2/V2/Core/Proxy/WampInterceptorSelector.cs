using System;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using WampSharp.Core.Contracts;
using WampSharp.Core.Proxy;
using WampSharp.V1.Core.Proxy;

namespace WampSharp.V2.Core.Proxy
{
    /// <summary>
    /// An <see cref="IInterceptorSelector"/> that chooses between
    /// <see cref="WampOutgoingInterceptor{TMessage}"/> and 
    /// <see cref="V1.Core.Proxy.SessionIdPropertyInterceptor"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampInterceptorSelector<TMessage> : IInterceptorSelector
    {
        private readonly WampOutgoingInterceptor<TMessage> mInterceptor;

        /// <summary>
        /// Creates a new instance of <see cref="V1.Core.Proxy.WampInterceptorSelector{TMessage}"/>.
        /// </summary>
        /// <param name="interceptor">The given <see cref="WampOutgoingInterceptor{TMessage}"/> used
        /// for WAMP method calls</param>
        public WampInterceptorSelector(WampOutgoingInterceptor<TMessage> interceptor)
        {
            mInterceptor = interceptor;
        }

        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            if (method.IsDefined(typeof (WampHandlerAttribute), true))
            {
                return new IInterceptor[] {mInterceptor};
            }
            // In case you were wondering, this is how a patch looks like.
            else if (method.IsSpecialName &&
                     method.Name == "get_Session")
            {
                return interceptors.OfType<SessionIdPropertyInterceptor>()
                                   .Cast<IInterceptor>()
                                   .ToArray();
            }

            return null;
        }
    }
}