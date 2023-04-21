using System.Reflection;

namespace WampSharp.V2.CalleeProxy
{
    internal abstract class CalleeProxyInterceptorBase : ICalleeProxyInvocationInterceptor
    {
        private readonly ICalleeProxyInterceptor mInterceptor;

        public CalleeProxyInterceptorBase(MethodInfo method, IWampCalleeProxyInvocationHandler handler, ICalleeProxyInterceptor interceptor)
        {
            Method = method;
            Handler = handler;
            mInterceptor = interceptor;
        }

        public ICalleeProxyInterceptor Interceptor => mInterceptor;

        public IWampCalleeProxyInvocationHandler Handler { get; }

        public MethodInfo Method { get; }

        public abstract object Invoke(MethodInfo method, object[] arguments);
    }

    internal abstract class CalleeProxyInterceptorBase<TResult> : CalleeProxyInterceptorBase
    {
        public CalleeProxyInterceptorBase(MethodInfo method, IWampCalleeProxyInvocationHandler handler,
            ICalleeProxyInterceptor interceptor)
            : base(method, handler, interceptor)
        {
            ResultExtractor = OperationResultExtractor.GetResultExtractor<TResult>(method);
        }

        public IOperationResultExtractor<TResult> ResultExtractor { get; }
    }
}