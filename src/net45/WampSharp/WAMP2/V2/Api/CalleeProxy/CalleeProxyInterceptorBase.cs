#if CASTLE || DISPATCH_PROXY
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Castle.DynamicProxy;
using WampSharp.Core.Utilities;
using WampSharp.Core.Utilities.ValueTuple;
using WampSharp.V2.Core;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.CalleeProxy
{
    internal abstract class CalleeProxyInterceptorBase : IInterceptor
    {
        private readonly ICalleeProxyInterceptor mInterceptor;

        public CalleeProxyInterceptorBase(MethodInfo method, IWampCalleeProxyInvocationHandler handler, ICalleeProxyInterceptor interceptor)
        {
            Method = method;
            Handler = handler;
            mInterceptor = interceptor;
        }

        public ICalleeProxyInterceptor Interceptor
        {
            get
            {
                return mInterceptor;
            }
        }

        public IWampCalleeProxyInvocationHandler Handler { get; }

        public MethodInfo Method { get; }

        public abstract object Invoke(MethodInfo method, object[] arguments);

#if CASTLE

        public void Intercept(IInvocation invocation)
        {
            object result = Invoke(invocation.Method, invocation.Arguments);
            invocation.ReturnValue = result;
        }

#endif
    }

    internal abstract class CalleeProxyInterceptorBase<TResult> : CalleeProxyInterceptorBase
    {
        public CalleeProxyInterceptorBase(MethodInfo method, IWampCalleeProxyInvocationHandler handler,
            ICalleeProxyInterceptor interceptor)
            : base(method, handler, interceptor)
        {
            Extractor = OperationResultExtractor.Get<TResult>(method);
        }

        public IOperationResultExtractor<TResult> Extractor { get; }
    }
}
#endif