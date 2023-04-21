using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace WampSharp.V2.CalleeProxy
{
    internal class ProgressiveAsyncCalleeProxyInterceptor<TProgress, TResult> : CalleeProxyInterceptorBase<TResult>
    {
        public IOperationResultExtractor<TProgress> ProgressExtractor { get; }

        public bool SupportsCancellation { get; }

        public ProgressiveAsyncCalleeProxyInterceptor(MethodInfo method, IWampCalleeProxyInvocationHandler handler, ICalleeProxyInterceptor interceptor) : base(method, handler, interceptor)
        {
            ProgressExtractor = OperationResultExtractor.GetProgressExtractor<TProgress>(method);

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

            IProgress<TProgress> progress = arguments[progressPosition] as IProgress<TProgress>;

            Task result =
                Handler.InvokeProgressiveAsync
                    (Interceptor, method, ProgressExtractor, ResultExtractor, argumentsWithoutProgress, progress, cancellationToken);

            return result;
        }
    }
}