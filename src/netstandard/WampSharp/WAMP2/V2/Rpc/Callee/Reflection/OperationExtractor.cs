using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using WampSharp.V2.Core.Contracts;
using TaskExtensions = WampSharp.Core.Utilities.TaskExtensions;

namespace WampSharp.V2.Rpc
{
    internal class OperationExtractor : IOperationExtractor
    {
        public IEnumerable<OperationToRegister> ExtractOperations(Type serviceType, Func<object> instance, ICalleeRegistrationInterceptor interceptor)
        {
            IEnumerable<Type> typesToExplore = GetTypesToExplore(serviceType);

            foreach (Type currentType in typesToExplore)
            {
                IEnumerable<OperationToRegister> currentOperations =
                    GetServiceMethodsOfType(instance, currentType, interceptor);

                foreach (OperationToRegister operation in currentOperations)
                {
                    yield return operation;
                }
            }
        }

        private static IEnumerable<Type> GetTypesToExplore(Type type)
        {
            yield return type;

            foreach (Type currentInterface in type.GetInterfaces())
            {
                yield return currentInterface;
            }
        }

        private IEnumerable<OperationToRegister> GetServiceMethodsOfType
            (Func<object> instance,
                Type type,
                ICalleeRegistrationInterceptor interceptor)
        {
            foreach (var method in type.GetPublicInstanceMethods())
            {
                if (interceptor.IsCalleeProcedure(method))
                {
                    IWampRpcOperation operation = CreateRpcMethod(instance, interceptor, method);
                    RegisterOptions options = interceptor.GetRegisterOptions(method);

                    yield return new OperationToRegister(operation, options);
                }
            }
        }

        protected IWampRpcOperation CreateRpcMethod(Func<object> instanceProvider, ICalleeRegistrationInterceptor interceptor, MethodInfo method)
        {
            string procedureUri =
                interceptor.GetProcedureUri(method);

            if (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(IObservable<>))
            {
                MethodInfoValidation.ValidateProgressiveObservableMethod(method);
                return CreateProgressiveObservableOperation(instanceProvider, method, procedureUri);
            }
            else if (!typeof (Task).IsAssignableFrom(method.ReturnType))
            {
                MethodInfoValidation.ValidateSyncMethod(method);
                return new SyncMethodInfoRpcOperation(instanceProvider, method, procedureUri);
            }
            else
            {
                if (method.IsDefined(typeof (WampProgressiveResultProcedureAttribute)))
                {
                    MethodInfoValidation.ValidateProgressiveMethod(method);
                    return CreateProgressiveOperation(instanceProvider, method, procedureUri);
                }
                else
                {
                    MethodInfoValidation.ValidateAsyncMethod(method);
                    return new AsyncMethodInfoRpcOperation(instanceProvider, method, procedureUri);
                }
            }
        }

        private static IWampRpcOperation CreateProgressiveOperation(Func<object> instanceProvider, MethodInfo method, string procedureUri)
        {
            //return new ProgressiveAsyncMethodInfoRpcOperation<returnType>
            // (instance, method, procedureUri);

            Type returnType =
                TaskExtensions.UnwrapReturnType(method.ReturnType);

            Type operationType =
                typeof (ProgressiveAsyncMethodInfoRpcOperation<>)
                    .MakeGenericType(returnType);

            IWampRpcOperation operation =
                (IWampRpcOperation) Activator.CreateInstance(operationType,
                    instanceProvider,
                    method,
                    procedureUri);

            return operation;
        }

        private static IWampRpcOperation CreateProgressiveObservableOperation(Func<object> instanceProvider, MethodInfo method, string procedureUri)
        {
            //return new ProgressiveObservableMethodInfoRpcOperation<returnType>
            // (instance, method, procedureUri);

            Type returnType = method.ReturnType.GetGenericArguments()[0];

            Type operationType =
                typeof(ProgressiveObservableMethodInfoRpcOperation<>)
                    .MakeGenericType(returnType);

            IWampRpcOperation operation =
                (IWampRpcOperation)Activator.CreateInstance(operationType,
                    instanceProvider,
                    method,
                    procedureUri);

            return operation;
        }
    }
}
