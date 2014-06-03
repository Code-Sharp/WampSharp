using System;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using WampSharp.Core.Contracts;
using WampSharp.Core.Proxy;

namespace WampSharp.V1.Core.Proxy
{
    /// <summary>
    /// An <see cref="IInterceptorSelector"/> that chooses between
    /// <see cref="WampOutgoingInterceptor{TMessage}"/> and 
    /// <see cref="SessionIdPropertyInterceptor"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampInterceptorSelector<TMessage> : IInterceptorSelector
    {
        private readonly WampOutgoingInterceptor<TMessage> mInterceptor;

        /// <summary>
        /// Creates a new instance of <see cref="WampInterceptorSelector{TMessage}"/>.
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
            else if (method.IsSpecialName)
            {
                // In case you were wondering, this is how a patch looks like.
                if (method.Name == "get_SessionId")
                {
                    return interceptors.OfType<SessionIdPropertyInterceptor>()
                        .Cast<IInterceptor>()
                        .ToArray();
                }
                else if (method.Name == "get_CraAuthenticator" || method.Name == "set_CraAuthenticator")
                {
                    return interceptors.OfType<WampCraAuthenticatorPropertyInterceptor>()
                        .Cast<IInterceptor>()
                        .ToArray();
                }
            }

            return null;
        }
    }
}