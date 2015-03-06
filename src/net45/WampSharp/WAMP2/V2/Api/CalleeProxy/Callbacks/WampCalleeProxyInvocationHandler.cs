using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WampSharp.Core.Utilities;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;
using TaskExtensions = WampSharp.Core.Utilities.TaskExtensions;

namespace WampSharp.V2.CalleeProxy
{
    internal abstract class WampCalleeProxyInvocationHandler : IWampCalleeProxyInvocationHandler
    {
        public object Invoke(ICalleeProxyInterceptor interceptor, MethodInfo method, object[] arguments)
        {
            Type unwrapped = TaskExtensions.UnwrapReturnType(method.ReturnType);

            SyncCallback callback = InnerInvokeSync(interceptor, method, arguments, unwrapped);

            WaitForResult(callback);

            Exception exception = callback.Exception;

            if (exception != null)
            {
                throw exception;
            }

            return callback.OperationResult;
        }

        private SyncCallback InnerInvokeSync(ICalleeProxyInterceptor interceptor, MethodInfo method, object[] arguments, Type unwrapped)
        {
            MethodInfoHelper methodInfoHelper = new MethodInfoHelper(method);

            IOperationResultExtractor extractor;
            
            if (method.HasMultivaluedResult())
            {
                extractor = new MultiValueExtractor(method.ReturnType);
            }
            else
            {
                extractor = new SingleValueExtractor(method.ReturnType, method.HasReturnValue());
            }

            string procedureUri = interceptor.GetProcedureUri(method);

            SyncCallback syncCallback = new SyncCallback(procedureUri, methodInfoHelper, arguments, extractor);

            object[] argumentsToSend = 
                methodInfoHelper.GetInputArguments(arguments);

            Invoke(interceptor, syncCallback, method, argumentsToSend);

            return syncCallback;
        }

        public Task InvokeAsync(ICalleeProxyInterceptor interceptor, MethodInfo method, object[] arguments)
        {
            Type returnType = method.ReturnType;

            Type unwrapped = TaskExtensions.UnwrapReturnType(returnType);

            IOperationResultExtractor extractor = GetOperationExtractor(method, unwrapped);

            AsyncOperationCallback callback = new AsyncOperationCallback(extractor);

            Task<object> task = InnerInvokeAsync(callback, interceptor, method, arguments);

            Task casted = task.Cast(unwrapped);

            return casted;
        }

#if !NET40
        public Task InvokeProgressiveAsync<T>(ICalleeProxyInterceptor interceptor, MethodInfo method, object[] arguments, IProgress<T> progress)
        {
            Type returnType = typeof(T);

            IOperationResultExtractor extractor = GetOperationExtractor(method, returnType);

            ProgressiveAsyncOperationCallback<T> asyncOperationCallback =
                new ProgressiveAsyncOperationCallback<T>(progress, extractor);

            Task<object> task = InnerInvokeAsync(asyncOperationCallback, interceptor, method, arguments);

            Task casted = task.Cast(returnType);

            return casted;
        }
#endif

        private static IOperationResultExtractor GetOperationExtractor(MethodInfo method, Type returnType)
        {
            IOperationResultExtractor extractor;

            if (method.HasMultivaluedResult())
            {
                extractor = new MultiValueExtractor(returnType);
            }
            else
            {
                bool hasReturnValue = method.HasReturnValue();
                extractor = new SingleValueExtractor(returnType, hasReturnValue);
            }

            return extractor;
        }

        private Task<object> InnerInvokeAsync(AsyncOperationCallback callback, ICalleeProxyInterceptor interceptor, MethodInfo method, object[] arguments)
        {
            Invoke(interceptor, callback, method, arguments);

            return AwaitForResult(callback);
        }

        protected abstract void Invoke(ICalleeProxyInterceptor interceptor, IWampRawRpcOperationClientCallback callback, MethodInfo method, object[] arguments);

        protected virtual void WaitForResult(SyncCallback callback)
        {
            callback.Wait(Timeout.Infinite);
        }

        protected virtual Task<object> AwaitForResult(AsyncOperationCallback asyncOperationCallback)
        {
            return asyncOperationCallback.Task;
        }
    }
}