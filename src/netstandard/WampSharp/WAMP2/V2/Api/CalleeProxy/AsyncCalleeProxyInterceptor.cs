using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace WampSharp.V2.CalleeProxy
{
    internal class AsyncCalleeProxyInterceptor<TResult> : CalleeProxyInterceptorBase<TResult>
    {
        public bool SupportsCancellation { get; }

        public AsyncCalleeProxyInterceptor(MethodInfo method, IWampCalleeProxyInvocationHandler handler, ICalleeProxyInterceptor interceptor) : 
            base(method, handler, interceptor)
        {
            SupportsCancellation =
                method.GetParameters().LastOrDefault()?.ParameterType == typeof(CancellationToken);
        }

        public override object Invoke(MethodInfo method, object[] arguments)
        {
            object[] methodArguments = arguments;
            CancellationToken cancellationToken = CancellationToken.None;

            if (SupportsCancellation)
            {
                cancellationToken = (CancellationToken)arguments.Last();
                methodArguments = 
                    arguments.Take(arguments.Length - 1).ToArray();
            }

            Task result =
                Handler.InvokeAsync<TResult>(Interceptor, method, ResultExtractor, methodArguments, cancellationToken);

            return result;
        }
    }
}