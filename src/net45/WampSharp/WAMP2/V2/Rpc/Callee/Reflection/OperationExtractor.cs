using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

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
            if (typeof (Task).IsAssignableFrom(method.ReturnType))
            {
                if (method.GetParameters().Any(x => x.IsOut || x.ParameterType.IsByRef))
                {
                    ThrowHelper.AsyncOutRefMethod(method);
                }

                return new AsyncMethodInfoRpcOperation(instance, method);
            }
            else
            {
                return new SyncMethodInfoRpcOperation(instance, method);
            }
        }

        private static class ThrowHelper
        {
            public static void AsyncOutRefMethod(MethodInfo method)
            {
                throw new ArgumentException
                    (String.Format(
                        "Method {0} of type {1} is declared as a WAMP procedure, but it is both asynchronous and has out/ref parameters",
                        method.Name, method.DeclaringType.FullName));
            }
        }
    }
}