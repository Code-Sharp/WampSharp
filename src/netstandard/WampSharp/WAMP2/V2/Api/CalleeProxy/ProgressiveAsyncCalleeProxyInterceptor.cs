#if CASTLE || DISPATCH_PROXY
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace WampSharp.V2.CalleeProxy
{
    internal class ProgressiveAsyncCalleeProxyInterceptor<T> : CalleeProxyInterceptorBase<T>
    {
        public bool SupportsCancellation { get; }

        public ProgressiveAsyncCalleeProxyInterceptor(MethodInfo method, IWampCalleeProxyInvocationHandler handler, ICalleeProxyInterceptor interceptor) : base(method, handler, interceptor)
        {
            SupportsCancellation = 
                method.GetParameters().LastOrDefault()?.ParameterType == typeof(CancellationToken);
        }

        public override object Invoke(MethodInfo method, object[] arguments)
        {
            int parametersLength = arguments.Length - 1;
            int progressPosition = arguments.Length - 1;
            CancellationToken cancellationToken = CancellationToken.None;

            if (SupportsCancellation)
            {
                parametersLength = parametersLength - 1;
                progressPosition = progressPosition - 1;
                cancellationToken = (CancellationToken) arguments.Last();
            }

            object[] argumentsWithoutProgress = new object[parametersLength];

            Array.Copy(arguments, argumentsWithoutProgress, argumentsWithoutProgress.Length);

            IProgress<T> progress = arguments[progressPosition] as IProgress<T>;

            Task result =
                Handler.InvokeProgressiveAsync
                    (Interceptor, method, Extractor, argumentsWithoutProgress, progress, cancellationToken);

            return result;
        }
    }
}
#endif