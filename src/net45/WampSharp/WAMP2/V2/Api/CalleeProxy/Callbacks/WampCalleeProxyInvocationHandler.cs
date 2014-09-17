using System;
using System.Collections.Generic;
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
        public object Invoke(MethodInfo method, object[] arguments)
        {
            Type unwrapped = TaskExtensions.UnwrapReturnType(method.ReturnType);

            SyncCallback callback = InnerInvokeSync(method, arguments, unwrapped);

            WaitForResult(callback);

            Exception exception = callback.Exception;

            if (exception != null)
            {
                throw exception;
            }

            return callback.OperationResult;
        }

        private SyncCallback InnerInvokeSync(MethodInfo method, object[] arguments, Type unwrapped)
        {
            SyncCallback syncCallback;

            MethodInfoHelper methodInfoHelper = new MethodInfoHelper(method);

            if (method.HasMultivaluedResult())
            {
                syncCallback = new MultiValueSyncCallback(methodInfoHelper, arguments);
            }
            else
            {
                syncCallback = new SingleValueSyncCallback(methodInfoHelper, arguments);
            }

            WampProcedureAttribute procedureAttribute =
                method.GetCustomAttribute<WampProcedureAttribute>(true);

            object[] argumentsToSend = 
                methodInfoHelper.GetInputArguments(arguments);

            Invoke(syncCallback, procedureAttribute.Procedure, argumentsToSend);

            return syncCallback;
        }

        public Task InvokeAsync(MethodInfo method, object[] arguments)
        {
            Type returnType = method.ReturnType;

            Type unwrapped = TaskExtensions.UnwrapReturnType(returnType);

            Task<object> task = InnerInvokeAsync(method, arguments, unwrapped);

            Task casted = task.Cast(unwrapped);

            return casted;
        }

        private Task<object> InnerInvokeAsync(MethodInfo method, object[] arguments, Type returnType)
        {
            AsyncOperationCallback asyncOperationCallback;

            if (method.HasMultivaluedResult())
            {
                asyncOperationCallback = new MultiValueAsyncOperationCallback(returnType);
            }
            else
            {
                bool hasReturnValue = method.HasReturnValue();
                asyncOperationCallback = new SingleValueAsyncOperationCallback(returnType, hasReturnValue);
            }

            WampProcedureAttribute procedureAttribute = 
                method.GetCustomAttribute<WampProcedureAttribute>(true);

            Invoke(asyncOperationCallback, procedureAttribute.Procedure, arguments);

            return AwaitForResult(asyncOperationCallback);
        }

        protected abstract void Invoke(IWampClientRawRpcOperationCallback callback, string procedure, object[] arguments);

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