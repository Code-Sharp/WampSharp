using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TaskExtensions = WampSharp.Core.Utilities.TaskExtensions;

namespace WampSharp.V2.Rpc
{
    internal class OperationExtractor : IOperationExtractor
    {
        public IEnumerable<IWampRpcOperation> ExtractOperations(object instance)
        {
            Type type = instance.GetType();
            IEnumerable<Type> typesToExplore = GetTypesToExplore(type);

            foreach (Type currentType in typesToExplore)
            {
                IEnumerable<IWampRpcOperation> currentOperations = 
                    GetServiceMethodsOfType(instance, currentType);

                foreach (IWampRpcOperation operation in currentOperations)
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

        private IEnumerable<IWampRpcOperation> GetServiceMethodsOfType(object instance, Type type)
        {
            return type.GetMethods()
                       .Where(method => method.IsDefined(typeof (WampProcedureAttribute), true))
                       .Select(method => CreateRpcMethod(instance, method));
        }

        protected IWampRpcOperation CreateRpcMethod(object instance, MethodInfo method)
        {
            if (!typeof (Task).IsAssignableFrom(method.ReturnType))
            {
                return new SyncMethodInfoRpcOperation(instance, method);
            }
            else
            {
                if (method.GetParameters().Any(x => x.IsOut || x.ParameterType.IsByRef))
                {
                    ThrowHelper.AsyncOutRefMethod(method);
                }
#if !NET40
                if (method.IsDefined(typeof (WampProgressiveResultProcedureAttribute)))
                {
                    return CreateProgressiveOperation(instance, method);
                }
                else
#endif
                {
                    return new AsyncMethodInfoRpcOperation(instance, method);
                }
            }
        }

#if !NET40
        private static IWampRpcOperation CreateProgressiveOperation(object instance, MethodInfo method)
        {
            //return new ProgressiveAsyncMethodInfoRpcOperation<returnType>
            // (instance, method);

            Type returnType =
                TaskExtensions.UnwrapReturnType(method.ReturnType);

            ParameterInfo lastParameter = method.GetParameters().Last();

            Type expectedParameterType = 
                typeof (IProgress<>).MakeGenericType(returnType);
            
            if (lastParameter.ParameterType != expectedParameterType)
            {
                ThrowHelper.ProgressiveParameterTypeMismatch(method, returnType);
            }

            Type operationType =
                typeof (ProgressiveAsyncMethodInfoRpcOperation<>)
                    .MakeGenericType(returnType);

            IWampRpcOperation operation =
                (IWampRpcOperation) Activator.CreateInstance(operationType,
                    instance,
                    method);

            return operation;
        }
#endif

        private static class ThrowHelper
        {
            public static void AsyncOutRefMethod(MethodInfo method)
            {
                throw new ArgumentException
                    (String.Format(
                        "Method {0} of type {1} is declared as a WAMP procedure, but it is both asynchronous and has out/ref parameters",
                        method.Name, method.DeclaringType.FullName));
            }

            public static void ProgressiveParameterTypeMismatch(MethodInfo method, Type returnType)
            {
                throw new ArgumentException
                    (String.Format(
                        "Method {0} of type {1} is declared as a progressive WAMP procedure, but its last parameter is not a IProgress of its return type. Expected: IProgress<{2}>",
                        method.Name, method.DeclaringType.FullName, returnType.FullName));
            }
        }
    }
}