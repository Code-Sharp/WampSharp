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
        private readonly MethodInfo mMethod;
        private readonly IWampCalleeProxyInvocationHandler mHandler;
        private readonly ICalleeProxyInterceptor mInterceptor;

        public CalleeProxyInterceptorBase(MethodInfo method, IWampCalleeProxyInvocationHandler handler, ICalleeProxyInterceptor interceptor)
        {
            mMethod = method;
            mHandler = handler;
            mInterceptor = interceptor;
        }

        public ICalleeProxyInterceptor Interceptor
        {
            get
            {
                return mInterceptor;
            }
        }

        public IWampCalleeProxyInvocationHandler Handler
        {
            get
            {
                return mHandler;
            }
        }

        public MethodInfo Method
        {
            get
            {
                return mMethod;
            }
        }

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
        private readonly IOperationResultExtractor<TResult> mExtractor;

        public CalleeProxyInterceptorBase(MethodInfo method, IWampCalleeProxyInvocationHandler handler,
            ICalleeProxyInterceptor interceptor)
            : base(method, handler, interceptor)
        {
            mExtractor = OperationResultExtractor.Get<TResult>(method);
        }

        public IOperationResultExtractor<TResult> Extractor
        {
            get
            {
                return mExtractor;
            }
        }
    }
}
#endif