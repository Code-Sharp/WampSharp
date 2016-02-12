using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using WampSharp.Core.Utilities;
using WampSharp.V2.Core.Contracts;
using WampSharp.WAMP2.V2.Rpc.Callee;
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
                IEnumerable<OperationToRegister> currentMemberOperations = GetServiceMembersOfType(instance, currentType, interceptor);

                foreach (OperationToRegister operation in currentMemberOperations)
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

        private IEnumerable<OperationToRegister> GetServiceMembersOfType
           (Func<object> instance,
               Type type,
               ICalleeRegistrationInterceptor interceptor)
        {
            foreach (var member in type.GetPublicInstanceMembers())
            {
                if (interceptor.IsCalleeMember(member))
                {
                    IWampRpcOperation operation = CreateRpcMethod(instance, interceptor, member);
                    RegisterOptions options = interceptor.GetRegisterOptions(member);

                    yield return new OperationToRegister(operation, options);
                }
            }
        }

        private IWampRpcOperation CreateRpcMethod(Func<object> instanceProvider, ICalleeRegistrationInterceptor interceptor, MemberInfo member)
        {
            if (member.MemberType == MemberTypes.Method)
            {
                return CreateRpcMethod(instanceProvider, interceptor, member as MethodInfo);
            }
            else if (member.MemberType == MemberTypes.Property)
            {
                //Use getter method of a property
                PropertyInfo propertyInfo = member as PropertyInfo;

                if (propertyInfo.GetMethod != null)
                    return CreateRpcMethod(instanceProvider, interceptor, propertyInfo.GetMethod, member);
                else
                    throw new Exception(string.Format("Getter not found for {1}", member.MemberType, member));
            }

            throw new Exception(string.Format("Unsupported member type {0} found when registering {1}", member.MemberType, member));

        }

        private bool HasServiceMembersInType
            (Type type,
                ICalleeRegistrationInterceptor interceptor)
        {
            foreach (var method in type.GetPublicInstanceMethods())
            {
                if (interceptor.IsCalleeMember(method))
                {
                    return true;
                }
            }

            return false;
        }

        //When procedure URI attribute is defined on method itself, not "parent" memember
        protected IWampRpcOperation CreateRpcMethod(Func<object> instanceProvider,
            ICalleeRegistrationInterceptor interceptor, MethodInfo method)
        {
            return CreateRpcMethod(instanceProvider, interceptor, method, method);
        }

        protected IWampRpcOperation CreateRpcMethod(Func<object> instanceProvider, ICalleeRegistrationInterceptor interceptor, MethodInfo method, MemberInfo procedureUriSource)
        {
            string procedureUri =
                interceptor.GetProcedureUri(procedureUriSource);

            //TODO: need better detection of nested
            if (HasServiceMembersInType(method.ReturnType, interceptor) && method.GetParameters().Length == 0)
            {
                return new LocalRpcInterfaceOperation(method.ReturnType, () => method.Invoke(instanceProvider(), new object[]{}), procedureUri, interceptor);
            }
            else if (!typeof (Task).IsAssignableFrom(method.ReturnType))
            {
                return new SyncMethodInfoRpcOperation(instanceProvider, method, procedureUri);
            }
            else
            {
#if !NET40
                if (method.IsDefined(typeof (WampProgressiveResultProcedureAttribute)))
                {
                    MethodInfoValidation.ValidateProgressiveMethod(method);
                    return CreateProgressiveOperation(instanceProvider, method, procedureUri);
                }
                else
#endif
                {
                    MethodInfoValidation.ValidateAsyncMethod(method);
                    return new AsyncMethodInfoRpcOperation(instanceProvider, method, procedureUri);
                }
            }
        }

#if !NET40
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

#endif

    }
}