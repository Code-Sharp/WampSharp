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
        protected delegate Task<T> InvokeProgressiveAsyncDelegate<T>(CalleeProxyBase proxy, IProgress<T> progress, CancellationToken cancellationToken, params object[] arguments);

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

        protected static InvokeProgressiveAsyncDelegate<T> GetInvokeProgressiveAsync<T>(MethodInfo method)
        {
            IOperationResultExtractor<T> extractor = GetExtractor<T>(method);
            return GetInvokeProgressiveAsync(method, extractor);
        }

        private static IOperationResultExtractor<T> GetExtractor<T>(MethodInfo method)
        {
            return OperationResultExtractor.Get<T>(method);
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

        private static InvokeProgressiveAsyncDelegate<T>
            GetInvokeProgressiveAsync<T>(MethodBase method,
                                         IOperationResultExtractor<T> extractor)
        {
            InvokeProgressiveAsyncDelegate<T> result =
                (proxy, progress, cancellationToken, arguments) =>
                        proxy.InvokeProgressiveAsync(method, progress, cancellationToken, extractor, arguments);

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

        protected Task<T> SingleInvokeProgressiveAsync<T>(MethodBase method, IProgress<T> progress,
                                                          params object[] arguments)
        {
            return InvokeProgressiveAsync<T>(method, progress, new SingleValueExtractor<T>(true), arguments);
        }

        protected Task<T[]> MultiInvokeProgressiveAsync<T>(MethodBase method, IProgress<T[]> progress,
                                                           params object[] arguments)
        {
            return InvokeProgressiveAsync<T[]>(method, progress, new MultiValueExtractor<T>(), arguments);
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

        private Task<T> InvokeProgressiveAsync<T>
        (MethodBase method,
         IProgress<T> progress,
         IOperationResultExtractor<T> resultExtractor,
         params object[] arguments)
        {
            return InvokeProgressiveAsync<T>
            (method,
             progress,
             CancellationToken.None,
             resultExtractor,
             arguments);
        }

        private Task<T> InvokeProgressiveAsync<T>
            (MethodBase method,
             IProgress<T> progress,
             CancellationToken cancellationToken,
             IOperationResultExtractor<T> resultExtractor,
             params object[] arguments)
        {
            MethodInfo methodInfo = (MethodInfo) method;

            return mHandler.InvokeProgressiveAsync
                (mInterceptor,
                 methodInfo,
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