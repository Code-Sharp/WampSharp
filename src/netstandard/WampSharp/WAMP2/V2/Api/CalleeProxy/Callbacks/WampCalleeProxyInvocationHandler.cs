using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WampSharp.V2.Core.Contracts;
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

            string procedureUri = interceptor.GetProcedureUri(method);

            SyncCallback<T> syncCallback = new SyncCallback<T>(procedureUri, methodInfoHelper, arguments, extractor);

            object[] argumentsToSend = 
                methodInfoHelper.GetInputArguments(arguments);

            Invoke(interceptor, syncCallback, method, argumentsToSend);

            return syncCallback;
        }

        public Task<T> InvokeAsync<T>(ICalleeProxyInterceptor interceptor, MethodInfo method, IOperationResultExtractor<T> extractor, object[] arguments, CancellationToken cancellationToken)
        {
            AsyncOperationCallback<T> callback = new AsyncOperationCallback<T>(extractor);

            Task<T> task = InnerInvokeAsync<T>(callback, interceptor, method, arguments, cancellationToken);

            return task;
        }

        public Task<TResult> InvokeProgressiveAsync<TProgress, TResult>(ICalleeProxyInterceptor interceptor, MethodInfo method, IOperationResultExtractor<TProgress> progressExtractor, IOperationResultExtractor<TResult> resultExtractor, object[] arguments, IProgress<TProgress> progress, CancellationToken cancellationToken)
        {
            ProgressiveAsyncOperationCallback<TProgress, TResult> asyncOperationCallback =
                new ProgressiveAsyncOperationCallback<TProgress, TResult>(progress, progressExtractor,resultExtractor);

            Task<TResult> task = InnerInvokeAsync(asyncOperationCallback, interceptor, method, arguments, cancellationToken);

            return task;
        }

        private Task<T> InnerInvokeAsync<T>(AsyncOperationCallback<T> callback, ICalleeProxyInterceptor interceptor, MethodInfo method, object[] arguments, CancellationToken cancellationToken)
        {
            var cancellableInvocation =  Invoke(interceptor, callback, method, arguments);

            // TODO: make the CancelOptions come from the ICalleeProxyInterceptor or something.
            CancellationTokenRegistration registration = cancellationToken.Register(() => cancellableInvocation.Cancel(new CancelOptions()));

            return AwaitForResult(callback, registration);
        }

        protected abstract IWampCancellableInvocationProxy Invoke(ICalleeProxyInterceptor interceptor, IWampRawRpcOperationClientCallback callback, MethodInfo method, object[] arguments);

        protected virtual void WaitForResult<T>(SyncCallback<T> callback)
        {
            callback.Wait(Timeout.Infinite);
        }

        protected virtual Task<T> AwaitForResult<T>(AsyncOperationCallback<T> asyncOperationCallback, CancellationTokenRegistration registration)
        {
            return asyncOperationCallback.Task;
        }
    }
}