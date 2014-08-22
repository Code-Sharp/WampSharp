using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WampSharp.Core.Serialization;
using WampSharp.Core.Utilities;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Rpc;
using TaskExtensions = WampSharp.Core.Utilities.TaskExtensions;

namespace WampSharp.V2.CalleeProxy
{
    internal abstract partial class WampCalleeProxyInvocationHandler : IWampCalleeProxyInvocationHandler
    {
        protected readonly Dictionary<string, object> mEmptyOptions = new Dictionary<string, object>();

        public virtual object Invoke(MethodInfo method, object[] arguments)
        {
            Type unwrapped = TaskExtensions.UnwrapReturnType(method.ReturnType);

            SyncCallback callback = InnerInvokeSync(method, arguments, unwrapped);

            // TODO: register to connection lost events and raise an exception
            // TODO: WaitHandle.WaitAny(connectionLost, callback.WaitHandle)
            callback.Wait(Timeout.Infinite);

            WampException exception = callback.Exception;

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

            if (HasMultivaluedResult(method))
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

            // TODO: register to connection lost events and raise an exception
            Task<object> task = InnerInvokeAsync(method, arguments, unwrapped);

            Task casted = task.Cast(unwrapped);

            // TODO: return await Task.WhenAny(casted, connectionLost);
            return casted;
        }

        private Task<object> InnerInvokeAsync(MethodInfo method, object[] arguments, Type returnType)
        {
            AsyncOperationCallback asyncOperationCallback;

            if (HasMultivaluedResult(method))
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

            return asyncOperationCallback.Task;
        }

        protected abstract void Invoke(IWampRawRpcOperationCallback callback, string procedure, object[] arguments);

        private bool HasMultivaluedResult(MethodInfo method)
        {
            WampResultAttribute resultAttribute = 
                method.ReturnParameter.GetCustomAttribute<WampResultAttribute>(true);

            if (!method.ReturnType.IsArray)
            {
                return false;
            }

            if ((resultAttribute != null) &&
                (resultAttribute.CollectionResultTreatment == CollectionResultTreatment.Multivalued))
            {
                return true;
            }

            return false;
        }
    }

}