using System.Reflection;
using WampSharp.Core.Utilities;

namespace WampSharp.V2.CalleeProxy
{
    public class CalleeDispatchProxy : DispatchProxy
    {
        private readonly SwapDictionary<MethodInfo, ICalleeProxyInvocationInterceptor> mMethodToInterceptor =
            new SwapDictionary<MethodInfo, ICalleeProxyInvocationInterceptor>();

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            ICalleeProxyInvocationInterceptor interceptor;

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