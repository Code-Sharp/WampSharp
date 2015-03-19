using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WampSharp.Core.Utilities;
using WampSharp.V2.Rpc;
using TaskExtensions = WampSharp.Core.Utilities.TaskExtensions;

namespace WampSharp.V2.CalleeProxy
{
    internal abstract class WampCalleeProxyInvocationHandler : IWampCalleeProxyInvocationHandler
    {
        public T Invoke<T>(ICalleeProxyInterceptor interceptor, MethodInfo method, IOperationResultExtractor<T> extractor, object[] arguments)
        {
            Type unwrapped = TaskExtensions.UnwrapReturnType(method.ReturnType);

            SyncCallback<T> callback = InnerInvokeSync<T>(interceptor, method, extractor, arguments, unwrapped);

            WaitForResult(callback);

            Exception exception = callback.Exception;

            if (exception != null)
            {
                throw exception;
            }

            return callback.OperationResult;
        }

        private SyncCallback<T> InnerInvokeSync<T>(ICalleeProxyInterceptor interceptor, MethodInfo method, IOperationResultExtractor<T> extractor, object[] arguments, Type unwrapped)
        {
            MethodInfoHelper methodInfoHelper = new MethodInfoHelper(method);

            SyncCallback<T> syncCallback = new SyncCallback<T>(methodInfoHelper, arguments, extractor);

            object[] argumentsToSend = 
                methodInfoHelper.GetInputArguments(arguments);

            Invoke(interceptor, syncCallback, method, argumentsToSend);

            return syncCallback;
        }

        public Task<T> InvokeAsync<T>(ICalleeProxyInterceptor interceptor, MethodInfo method, IOperationResultExtractor<T> extractor, object[] arguments)
        {
            AsyncOperationCallback<T> callback = new AsyncOperationCallback<T>(extractor);

            Task<T> task = InnerInvokeAsync<T>(callback, interceptor, method, arguments);

            return task;
        }

#if !NET40
        public Task<T> InvokeProgressiveAsync<T>(ICalleeProxyInterceptor interceptor, MethodInfo method, IOperationResultExtractor<T> extractor, object[] arguments, IProgress<T> progress)
        {
            ProgressiveAsyncOperationCallback<T> asyncOperationCallback =
                new ProgressiveAsyncOperationCallback<T>(progress, extractor);

            Task<T> task = InnerInvokeAsync<T>(asyncOperationCallback, interceptor, method, arguments);

            return task;
        }
#endif

        private Task<T> InnerInvokeAsync<T>(AsyncOperationCallback<T> callback, ICalleeProxyInterceptor interceptor, MethodInfo method, object[] arguments)
        {
            Invoke(interceptor, callback, method, arguments);

            return AwaitForResult(callback);
        }

        protected abstract void Invoke(ICalleeProxyInterceptor interceptor, IWampRawRpcOperationClientCallback callback, MethodInfo method, object[] arguments);

        protected virtual void WaitForResult<T>(SyncCallback<T> callback)
        {
            callback.Wait(Timeout.Infinite);
        }

        protected virtual Task<T> AwaitForResult<T>(AsyncOperationCallback<T> asyncOperationCallback)
        {
            return asyncOperationCallback.Task;
        }
    }
}