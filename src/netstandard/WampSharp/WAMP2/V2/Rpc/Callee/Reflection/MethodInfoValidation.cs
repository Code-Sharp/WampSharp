using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
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

                TupleElementNamesAttribute attribute =
                    method.ReturnParameter.GetCustomAttribute<TupleElementNamesAttribute>();

                if (attribute != null)
                {
                    int tupleLength = tupleType.GetValueTupleLength();

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

        public static void ValidateSyncMethod(MethodInfo method)
        {
            if (method.GetParameters().Any(x => x.ParameterType == typeof(CancellationToken)))
            {
                ThrowHelper.SyncMethodDoesNotSupportCancellation(method);
            }

            ValidateTupleReturnType(method);
        }

        internal static void ValidateProgressiveObservableMethod(MethodInfo method)
        {
            if (!method.IsDefined(typeof(WampProgressiveResultProcedureAttribute)))
            {
                ThrowHelper.ObservableMethodNotDeclaredProgressive(method);
            }
        }

        public static void ValidateAsyncMethod(MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();

            if (parameters.Any(x => x.ParameterType == typeof(CancellationToken) &&
                                    x.Position != parameters.Length - 1))
            {
                ThrowHelper.CancellationTokenMustBeLastParameter(method);
            }

            if (parameters.Any(x => x.IsOut || x.ParameterType.IsByRef))
            {
                ThrowHelper.AsyncOutRefMethod(method);
            }

            ValidateTupleReturnType(method);
        }

        public static void ValidateProgressiveMethod(MethodInfo method)
        {
            ValidateAsyncMethod(method);

            ParameterInfo progressParameter = method.GetProgressParameter();

            if ((progressParameter == null) || (progressParameter.ParameterType.GetGenericTypeDefinition() != typeof(IProgress<>)))
            {
                ThrowHelper.ProgressiveParameterTypeMismatch(method);
            }
        }

        private static class ThrowHelper
        {
            public static void AsyncOutRefMethod(MethodInfo method)
            {
                throw new ArgumentException
                    ($"Method {method.Name} of type {method.DeclaringType.FullName} is declared as a WAMP procedure, but it is both asynchronous and has out/ref parameters");
            }

            public static void ProgressiveParameterTypeMismatch(MethodInfo method)
            {
                throw new ArgumentException
                    ($"Method {method.Name} of type {method.DeclaringType.FullName} is declared as a progressive WAMP procedure, but its last (or second to last) parameter is not an IProgress<TProgress>");
            }

            public static void ObservableMethodNotDeclaredProgressive(MethodInfo method)
            {
                throw new ArgumentException
                    ($"Method {method.Name} of type {method.DeclaringType.FullName} is returning an IObservable and therefore is required to be declared as a progressive WAMP procedure, but it is not. Please use the [WampProgressiveResultProcedure] attribute.");
            }

            public static void InvalidTupleReturnType(MethodInfo method)
            {
                throw new ArgumentException
                    ($"Method {method.Name} of type {method.DeclaringType.FullName} is declared as a WAMP procedure that returns a tuple which some of its elements have names and some don't. Expected all tuple elements have names or none of them have names.");
            }

            public static void TupleReturnTypeAndOutRefParametersHaveCommonNames(MethodInfo method, ICollection<string> intersection)
            {
                throw new ArgumentException
                    ($"Method {method.Name} of type {method.DeclaringType.FullName} is declared as a WAMP procedure that returns a tuple and also has out/ref parameters. There exists some tuple elements with the same names as some of the out/ref parameters. Expected out/ref and element names of returned tuple to be distinct. Details: conflicted names {string.Join(", ", intersection)}");
            }

            public static void MethodReturnsInvalidValueTuple(MethodInfo method)
            {
                throw new ArgumentException
                    ($"Method {method.Name} of type {method.DeclaringType.FullName} returns an invalid ValueTuple. Expected TRest to be a ValueTuple.");
            }

            public static void SyncMethodDoesNotSupportCancellation(MethodInfo method)
            {
                throw new ArgumentException
                ($"Method {method.Name} of type {method.DeclaringType.FullName} is a synchronous method, but expects to receive a CancellationToken.");
            }

            public static void CancellationTokenMustBeLastParameter(MethodInfo method)
            {
                throw new ArgumentException
                ($"Method {method.Name} of type {method.DeclaringType.FullName} receives a CancellationToken not as its last argument. A CancellationToken can be declared only as the last argument of a method.");
            }
        }
    }
}
