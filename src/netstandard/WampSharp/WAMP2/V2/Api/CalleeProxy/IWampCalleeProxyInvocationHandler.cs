using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace WampSharp.V2.CalleeProxy
{
    internal interface IWampCalleeProxyInvocationHandler
    {
        T Invoke<T>(ICalleeProxyInterceptor interceptor, MethodInfo method, IOperationResultExtractor<T> extractor, object[] arguments);
        Task<T> InvokeAsync<T>(ICalleeProxyInterceptor interceptor, MethodInfo method, IOperationResultExtractor<T> extractor, object[] arguments, CancellationToken cancellationToken);
        Task<TResult> InvokeProgressiveAsync<TProgress,TResult>(ICalleeProxyInterceptor interceptor, MethodInfo method, IOperationResultExtractor<TProgress> progressExtractor, IOperationResultExtractor<TResult> resultExtractor, object[] arguments, IProgress<TProgress> progress, CancellationToken cancellationToken);
    }
}