using System;
using System.Reflection;
using System.Threading.Tasks;

namespace WampSharp.V2.CalleeProxy
{
    internal interface IWampCalleeProxyInvocationHandler
    {
        T Invoke<T>(ICalleeProxyInterceptor interceptor, MethodInfo method, IOperationResultExtractor<T> extractor, object[] arguments);
        Task<T> InvokeAsync<T>(ICalleeProxyInterceptor interceptor, MethodInfo method, IOperationResultExtractor<T> extractor, object[] arguments);
        Task<T> InvokeProgressiveAsync<T>(ICalleeProxyInterceptor interceptor, MethodInfo method, IOperationResultExtractor<T> extractor, object[] arguments, IProgress<T> progress);
    }
}