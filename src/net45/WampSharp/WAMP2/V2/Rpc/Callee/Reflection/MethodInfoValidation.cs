using System;
using System.Linq;
using System.Reflection;
using WampSharp.Core.Utilities;

namespace WampSharp.V2.Rpc
{
    internal class MethodInfoValidation
    {
        public static void ValidateAsyncMethod(MethodInfo method)
        {
            if (method.GetParameters().Any(x => x.IsOut || x.ParameterType.IsByRef))
            {
                ThrowHelper.AsyncOutRefMethod(method);
            }
        }

        public static void ValidateProgressiveMethod(MethodInfo method)
        {
            ValidateAsyncMethod(method);

            Type returnType =
                TaskExtensions.UnwrapReturnType(method.ReturnType);

            ParameterInfo lastParameter = method.GetParameters().Last();

            Type expectedParameterType =
                typeof(IProgress<>).MakeGenericType(returnType);

            if (lastParameter.ParameterType != expectedParameterType)
            {
                ThrowHelper.ProgressiveParameterTypeMismatch(method, returnType);
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