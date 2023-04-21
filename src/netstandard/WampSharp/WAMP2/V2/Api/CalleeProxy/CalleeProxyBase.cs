using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WampSharp.Core.Utilities;
using WampSharp.V2.Client;

namespace WampSharp.V2.CalleeProxy
{
    public class CalleeProxyBase
    {
        protected delegate T InvokeSyncDelegate<T>(CalleeProxyBase proxy, params object[] arguments);
        protected delegate Task<T> InvokeAsyncDelegate<T>(CalleeProxyBase proxy, CancellationToken cancellationToken, params object[] arguments);
        protected delegate Task<TResult> InvokeProgressiveAsyncDelegate<TProgress, TResult>(CalleeProxyBase proxy, IProgress<TProgress> progress, CancellationToken cancellationToken, params object[] arguments);

        private readonly WampCalleeProxyInvocationHandler mHandler;
        private readonly ICalleeProxyInterceptor mInterceptor;

        public CalleeProxyBase(IWampRealmProxy realmProxy,
                               ICalleeProxyInterceptor interceptor)
        {
            mInterceptor = interceptor;
            mHandler = new ClientInvocationHandler(realmProxy);
        }

        protected static InvokeSyncDelegate<T> GetInvokeSync<T>(MethodInfo method)
        {
            IOperationResultExtractor<T> extractor = GetExtractor<T>(method);
            return GetInvokeSync(method, extractor);
        }

        protected static InvokeAsyncDelegate<T> GetInvokeAsync<T>(MethodInfo method)
        {
            IOperationResultExtractor<T> extractor = GetExtractor<T>(method);
            return GetInvokeAsync(method, extractor);
        }

        protected static InvokeProgressiveAsyncDelegate<TProgress, TResult> GetInvokeProgressiveAsync<TProgress, TResult>(MethodInfo method)
        {
            IOperationResultExtractor<TResult> resultExtractor = GetExtractor<TResult>(method);
            IOperationResultExtractor<TProgress> progressExtractor = GetExtractor<TProgress>(method);
            return GetInvokeProgressiveAsync(method, progressExtractor, resultExtractor);
        }

        private static IOperationResultExtractor<T> GetExtractor<T>(MethodInfo method)
        {
            return OperationResultExtractor.GetResultExtractor<T>(method);
        }


        private static InvokeSyncDelegate<T> GetInvokeSync<T>(MethodBase method, IOperationResultExtractor<T> extractor)
        {
            InvokeSyncDelegate<T> result =
                (proxy, arguments) =>
                        proxy.InvokeSync(method, extractor, arguments);

            return result;
        }

        private static InvokeAsyncDelegate<T> GetInvokeAsync<T>(MethodBase method, IOperationResultExtractor<T> extractor)
        {
            InvokeAsyncDelegate<T> result =
                (proxy, cancellationToken, arguments) =>
                        proxy.InvokeAsync(method, extractor, cancellationToken, arguments);

            return result;
        }

        private static InvokeProgressiveAsyncDelegate<TProgress, TResult>
            GetInvokeProgressiveAsync<TProgress, TResult>(MethodBase method,
                                                          IOperationResultExtractor<TProgress> progressExtractor,
                                                          IOperationResultExtractor<TResult> resultExtractor)
        {
            InvokeProgressiveAsyncDelegate<TProgress, TResult> result =
                (proxy, progress, cancellationToken, arguments) =>
                        proxy.InvokeProgressiveAsync(method, progress, cancellationToken, progressExtractor, resultExtractor, arguments);

            return result;
        }

        protected T SingleInvokeSync<T>(MethodBase method, params object[] arguments)
        {
            return InvokeSync(method, new SingleValueExtractor<T>(true), arguments);
        }

        protected void SingleInvokeSync(MethodBase method, params object[] arguments)
        {
            InvokeSync(method, new SingleValueExtractor<object>(false), arguments);
        }

        protected T[] MultiInvokeSync<T>(MethodBase method, params object[] arguments)
        {
            return InvokeSync(method, new MultiValueExtractor<T>(), arguments);
        }

        protected Task<T> SingleInvokeAsync<T>(MethodBase method, params object[] arguments)
        {
            return InvokeAsync(method, new SingleValueExtractor<T>(true), arguments);
        }

        protected Task SingleInvokeAsync(MethodBase method, params object[] arguments)
        {
            return InvokeAsync(method, new SingleValueExtractor<object>(false), arguments);
        }

        protected Task<T[]> MultiInvokeAsync<T>(MethodBase method, params object[] arguments)
        {
            return InvokeAsync(method, new MultiValueExtractor<T>(), arguments);
        }

        protected Task<TResult> SingleInvokeProgressiveAsync<TProgress, TResult>(MethodBase method, IProgress<TProgress> progress,
                                                          params object[] arguments)
        {
            return InvokeProgressiveAsync<TProgress, TResult>(method, progress, new SingleValueExtractor<TProgress>(true), new SingleValueExtractor<TResult>(true), arguments);
        }

        protected Task<TResult[]> MultiInvokeProgressiveAsync<TProgress, TResult>(MethodBase method, IProgress<TProgress[]> progress,
                                                           params object[] arguments)
        {
            return InvokeProgressiveAsync<TProgress[], TResult[]>(method, progress, new MultiValueExtractor<TProgress>(), new MultiValueExtractor<TResult>(), arguments);
        }

        private T InvokeSync<T>(MethodBase method,
                                IOperationResultExtractor<T> valueExtractor,
                                params object[] arguments)
        {
            MethodInfo methodInfo = (MethodInfo) method;

            return mHandler.Invoke
                (mInterceptor,
                 methodInfo,
                 valueExtractor,
                 arguments);
        }

        private Task<T> InvokeAsync<T>(MethodBase method,
                                       IOperationResultExtractor<T> valueExtractor,
                                       params object[] arguments)
        {
            return InvokeAsync<T>(method, valueExtractor, CancellationToken.None, arguments);
        }

        private Task<T> InvokeAsync<T>(MethodBase method,
                                       IOperationResultExtractor<T> valueExtractor,
                                       CancellationToken cancellationToken,
                                       params object[] arguments)
        {
            MethodInfo methodInfo = (MethodInfo) method;

            return mHandler.InvokeAsync
                (mInterceptor,
                 methodInfo,
                 valueExtractor,
                 arguments,
                 cancellationToken);
        }

        private Task<TResult> InvokeProgressiveAsync<TProgress, TResult>
        (MethodBase method,
         IProgress<TProgress> progress,
         IOperationResultExtractor<TProgress> progressExtractor,
         IOperationResultExtractor<TResult> resultExtractor,
         params object[] arguments)
        {
            return InvokeProgressiveAsync
            (method,
             progress,
             CancellationToken.None,
             progressExtractor,
             resultExtractor,
             arguments);
        }

        private Task<TResult> InvokeProgressiveAsync<TProgress, TResult>
            (MethodBase method,
             IProgress<TProgress> progress,
             CancellationToken cancellationToken,
             IOperationResultExtractor<TProgress> progressExtractor,
             IOperationResultExtractor<TResult> resultExtractor,
             params object[] arguments)
        {
            MethodInfo methodInfo = (MethodInfo) method;

            return mHandler.InvokeProgressiveAsync
                (mInterceptor,
                 methodInfo,
                 progressExtractor,
                 resultExtractor,
                 arguments,
                 progress,
                 cancellationToken);
        }

        protected static MethodInfo GetMethodInfo<T>(Expression<Action<T>> expression)
        {
            return Method.Get(expression);
        }

        protected static MethodInfo GetMethodInfo<T>(Func<Expression<Action<T>>> expressionFactory)
        {
            return Method.Get(expressionFactory());
        }
    }
}