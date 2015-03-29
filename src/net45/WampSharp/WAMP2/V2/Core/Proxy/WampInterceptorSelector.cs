#if CASTLE
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using WampSharp.Core.Contracts;
using WampSharp.Core.Proxy;

namespace WampSharp.V2.Core.Proxy
{
    /// <summary>
    /// An <see cref="IInterceptorSelector"/> that chooses between
    /// <see cref="WampOutgoingInterceptor{TMessage}"/> and 
    /// <see cref="WampRawOutgoingInterceptor{TMessage}"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class WampInterceptorSelector<TMessage> : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            IEnumerable<IInterceptor> relevantInterceptors = Enumerable.Empty<IInterceptor>();

            if (method.IsDefined(typeof(WampRawHandlerAttribute), true))
            {
                relevantInterceptors =
                    interceptors.OfType<WampRawOutgoingInterceptor<TMessage>>();
            }
            else if (method.IsDefined(typeof(WampHandlerAttribute), true))
            {
                relevantInterceptors =
                    interceptors.OfType<WampOutgoingInterceptor<TMessage>>();
            }

            IInterceptor[] result = relevantInterceptors.ToArray();

            return result;
        }
    }
}
#endif