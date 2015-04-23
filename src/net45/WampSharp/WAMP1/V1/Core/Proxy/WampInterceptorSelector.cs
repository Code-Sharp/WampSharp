#if CASTLE
using System;
using System.Collections.Generic;
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
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            IEnumerable<IInterceptor> relevantInterceptors = Enumerable.Empty<IInterceptor>();

            if (method.IsDefined(typeof(WampHandlerAttribute), true))
            {
                relevantInterceptors = 
                    interceptors.OfType<WampOutgoingInterceptor<TMessage>>();
            }
            else if (method.IsSpecialName)
            {
                // In case you were wondering, this is how a patch looks like.
                if (method.Name == "get_SessionId")
                {
                    relevantInterceptors =
                        interceptors.OfType<SessionIdPropertyInterceptor>();
                }
                else if (method.Name == "get_CraAuthenticator" || method.Name == "set_CraAuthenticator")
                {
                    relevantInterceptors =
                        interceptors.OfType<WampCraAuthenticatorPropertyInterceptor>();
                }
            }

            IInterceptor[] result = relevantInterceptors.ToArray();

            return result;
        }
    }
}
#endif