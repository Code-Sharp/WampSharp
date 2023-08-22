using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using WampSharp.Core.Utilities;
using WampSharp.Core.Utilities.ValueTuple;
using WampSharp.V2.Core;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.CalleeProxy
{
    internal class OperationResultExtractor
    {
        public static IOperationResultExtractor<TResult> GetResultExtractor<TResult>(MethodInfo method)
        {
            IOperationResultExtractor<TResult> extractor;

            if (typeof(TResult).IsValueTuple())
            {
                extractor = GetValueTupleOperationResultExtractor<TResult>(method);
            }
            else
            {
                extractor = GetNonTupleExtractor<TResult>(method, method.HasReturnValue());
            }

            return extractor;
        }

        public static IOperationResultExtractor<TProgress> GetProgressExtractor<TProgress>(MethodInfo method)
        {
            IOperationResultExtractor<TProgress> extractor;

            if (typeof(TProgress).IsValueTuple())
            {
                extractor = GetValueTupleOperationResultExtractor<TProgress>(method.GetProgressParameter());
            }
            else
            {
                extractor = GetNonTupleExtractor<TProgress>(method, true);
            }

            return extractor;
        }

        private static IOperationResultExtractor<TResult> GetNonTupleExtractor<TResult>(MethodInfo method, bool hasReturnValue)
        {
            IOperationResultExtractor<TResult> extractor;
            
            if (!method.HasMultivaluedResult(typeof(TResult)))
            {
                extractor = new SingleValueExtractor<TResult>(hasReturnValue);
            }
            else
            {
                Type elementType = typeof(TResult).GetElementType();

                Type extractorType =
                    typeof(MultiValueExtractor<>).MakeGenericType(elementType);

                extractor =
                    (IOperationResultExtractor<TResult>)Activator.CreateInstance(extractorType);
            }

            return extractor;
        }

        private static IOperationResultExtractor<TProgress> GetValueTupleOperationResultExtractor<TProgress>(ParameterInfo progressParameter)
        {
            Type tupleType = progressParameter.ParameterType.GetGenericArguments()[0];
            
            ArgumentUnpacker unpacker = GetTupleArgumentUnpacker(progressParameter, tupleType);

            return new ValueTupleValueExtractor<TProgress>(unpacker);
        }

        private static IOperationResultExtractor<TResult> GetValueTupleOperationResultExtractor<TResult>(MethodInfo method)
        {
            ArgumentUnpacker unpacker = GetTupleArgumentUnpacker(method);

            return new ValueTupleValueExtractor<TResult>(unpacker);
        }

        private static ArgumentUnpacker GetTupleArgumentUnpacker(MethodInfo method)
        {
            Type tupleType = TaskExtensions.UnwrapReturnType(method.ReturnType);

            ParameterInfo valueTupleParameterInfo = method.ReturnParameter;
            
            return GetTupleArgumentUnpacker(valueTupleParameterInfo, tupleType);
        }

        private static ArgumentUnpacker GetTupleArgumentUnpacker(ParameterInfo valueTupleParameterInfo, Type tupleType)
        {
            IEnumerable<string> transformNames = null;

            TupleElementNamesAttribute attribute =
                valueTupleParameterInfo.GetCustomAttribute<TupleElementNamesAttribute>();

            if (attribute != null)
            {
                transformNames = attribute.TransformNames;
            }

            return ArgumentUnpackerHelper.GetValueTupleArgumentUnpacker
                (tupleType, transformNames);
        }
    }
}