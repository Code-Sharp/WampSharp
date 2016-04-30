#if CASTLE || DISPATCH_PROXY
#if !NET40
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WampSharp.V2.CalleeProxy
{
    internal class ProgressiveAsyncCalleeProxyInterceptor<T> : CalleeProxyInterceptorBase<T>
    {
        public ProgressiveAsyncCalleeProxyInterceptor(MethodInfo method, IWampCalleeProxyInvocationHandler handler, ICalleeProxyInterceptor interceptor) : base(method, handler, interceptor)
        {
        }

        public override object Invoke(MethodInfo method, object[] arguments)
        {
            object[] argumentsWithoutProgress = new object[arguments.Length - 1];

            Array.Copy(arguments, argumentsWithoutProgress, argumentsWithoutProgress.Length);

            IProgress<T> progress = arguments.Last() as IProgress<T>;

            Task result =
                Handler.InvokeProgressiveAsync
                    (Interceptor, method, Extractor, argumentsWithoutProgress, progress);

            return result;
        }
    }
}
#endif
#endif