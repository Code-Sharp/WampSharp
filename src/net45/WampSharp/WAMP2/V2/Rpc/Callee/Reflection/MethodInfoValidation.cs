using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using WampSharp.Core.Utilities;
using WampSharp.Core.Utilities.ValueTuple;

namespace WampSharp.V2.Rpc
{
    internal class MethodInfoValidation
    {
        public static void ValidateTupleReturnType(MethodInfo method)
        {
            if (method.ReturnsTuple())
            {
                Type returnType = method.ReturnType;
                Type tupleType = TaskExtensions.UnwrapReturnType(returnType);

                if (!tupleType.IsValidTupleType())
                {
                    ThrowHelper.MethodReturnsInvalidValueTuple(method);
                }

                if (method.ReturnParameter.IsDefined(typeof(TupleElementNamesAttribute)))
                {
                    int tupleLength = tupleType.GetValueTupleLength();

                    TupleElementNamesAttribute attribute = 
                        method.ReturnParameter.GetCustomAttribute<TupleElementNamesAttribute>();

                    IList<string> transformNames = attribute.TransformNames;

                    List<string> tupleNames = 
                        transformNames.Take(tupleLength).ToList();

                    ValidateTupleReturnType(method, tupleNames);
                    ValidateTupleReturnTypeWithOutRefParameters(method, tupleNames);
                }
            }
        }

        private static void ValidateTupleReturnTypeWithOutRefParameters(MethodInfo method, IEnumerable<string> tupleNames)
        {
            IEnumerable<string> outOrRefNames =
                method.GetParameters().Where(x => x.IsOut || x.ParameterType.IsByRef)
                  .Select(x => x.Name);

            ICollection<string> intersection = 
                tupleNames.Intersect(outOrRefNames).ToList();

            if (intersection.Count > 0)
            {
                ThrowHelper.TupleReturnTypeAndOutRefParametersHaveCommonNames(method, intersection);
            }
        }

        private static void ValidateTupleReturnType(MethodInfo method, IList<string> tupleNames)
        {
            if (tupleNames.Any(x => x == null) && tupleNames.Any(x => x != null))
            {
                ThrowHelper.InvalidTupleReturnType(method);
            }
        }

        public static void ValidateAsyncMethod(MethodInfo method)
        {
            if (method.GetParameters().Any(x => x.IsOut || x.ParameterType.IsByRef))
            {
                ThrowHelper.AsyncOutRefMethod(method);
            }

            ValidateTupleReturnType(method);
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

            ValidateTupleReturnTypeOfProgressiveMethod(method, lastParameter);
        }

        private static void ValidateTupleReturnTypeOfProgressiveMethod(MethodInfo method, ParameterInfo lastParameter)
        {
            bool methodHasAttribute = method.ReturnParameter.IsDefined(typeof(TupleElementNamesAttribute));
            bool parameterHasAttributte = lastParameter.IsDefined(typeof(TupleElementNamesAttribute));

            bool attributesMatch = methodHasAttribute == parameterHasAttributte;

            if (methodHasAttribute && parameterHasAttributte)
            {
                TupleElementNamesAttribute methodAttribute =
                    method.ReturnParameter.GetCustomAttribute<TupleElementNamesAttribute>();

                TupleElementNamesAttribute parameterAttribute =
                    lastParameter.GetCustomAttribute<TupleElementNamesAttribute>();

                IList<string> methodTransformNames = methodAttribute.TransformNames;
                IList<string> parameterTransformNames = parameterAttribute.TransformNames;

                attributesMatch = methodTransformNames.SequenceEqual(parameterTransformNames);
            }

            if (!attributesMatch)
            {
                ThrowHelper.ProgressiveParameterTupleMismatch(method);
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

            public static void ProgressiveParameterTupleMismatch(MethodInfo method)
            {
                throw new ArgumentException
                    (String.Format(
                        "Method {0} of type {1} is declared as a progressive WAMP procedure that returns a tuple, but its last parameter tuple definition does not match its return type tuple definition.",
                        method.Name, method.DeclaringType.FullName));
            }

            public static void InvalidTupleReturnType(MethodInfo method)
            {
                throw new ArgumentException
                    (String.Format(
                        "Method {0} of type {1} is declared as a WAMP procedure that returns a tuple which some of its elements have names and some don't. Expected all tuple elements have names or none of them have names.",
                        method.Name, method.DeclaringType.FullName));
            }

            public static void TupleReturnTypeAndOutRefParametersHaveCommonNames(MethodInfo method, ICollection<string> intersection)
            {
                throw new ArgumentException
                    (String.Format(
                        "Method {0} of type {1} is declared as a WAMP procedure that returns a tuple and also has out/ref parameters. There exists some tuple elements with the same names as some of the out/ref parameters. Expected out/ref and element names of returned tuple to be distinct. Details: conflicted names {2}",
                        method.Name, method.DeclaringType.FullName, 
                        string.Join(", ", intersection)));
            }

            public static void MethodReturnsInvalidValueTuple(MethodInfo method)
            {
                throw new ArgumentException
                    (String.Format(
                        "Method {0} of type {1} returns an invalid ValueTuple. Expected TRest to be a ValueTuple.",
                        method.Name, method.DeclaringType.FullName));
            }
        }
    }
}