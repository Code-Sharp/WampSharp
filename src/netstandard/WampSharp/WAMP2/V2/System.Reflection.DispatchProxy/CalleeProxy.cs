﻿#if DISPATCH_PROXY

using System;
using System.Reflection;
using Castle.DynamicProxy;
using WampSharp.Core.Utilities;

// ReSharper disable once CheckNamespace
namespace WampSharp.V2.CalleeProxy
{
    public class CalleeProxy : DispatchProxy
    {
        private readonly SwapDictionary<MethodInfo, IInterceptor> mMethodToInterceptor =
            new SwapDictionary<MethodInfo, IInterceptor>();

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            IInterceptor interceptor;

            if (!mMethodToInterceptor.TryGetValue(targetMethod, out interceptor))
            {
                interceptor = CalleeProxyInterceptorFactory.BuildInterceptor(targetMethod, this.CalleeProxyInterceptor, Handler);
                mMethodToInterceptor[targetMethod] = interceptor;
            }

            object result = interceptor.Invoke(targetMethod, args);

            return result;
        }

        internal ICalleeProxyInterceptor CalleeProxyInterceptor { get; set; }

        internal WampCalleeProxyInvocationHandler Handler { get; set; }
    }
}

#endif