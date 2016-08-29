using System;
using System.Collections.Generic;
using System.Linq;
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
        public static IOperationResultExtractor<T> Get<T>(MethodInfo method)
        {
            IOperationResultExtractor<T> extractor;

            if (typeof(T).IsValueTuple())
            {
                extractor = GetValueTupleOperationResultExtractor<T>(method);
            }
            else if (!method.HasMultivaluedResult())
            {
                bool hasReturnValue = method.HasReturnValue();
                extractor = new SingleValueExtractor<T>(hasReturnValue);
            }
            else
            {
                Type elementType = typeof(T).GetElementType();

                Type extractorType =
                    typeof(MultiValueExtractor<>).MakeGenericType(elementType);

                extractor =
                    (IOperationResultExtractor<T>)Activator.CreateInstance(extractorType);
            }

            return extractor;
        }

        private static IOperationResultExtractor<T> GetValueTupleOperationResultExtractor<T>(MethodInfo method)
        {
            ArgumentUnpacker unpacker = GetTupleArgumentUnpacker(method);

            return new ValueTupleValueExtractor<T>(unpacker);
        }

        private static ArgumentUnpacker GetTupleArgumentUnpacker(MethodInfo method)
        {
            Type tupleType = TaskExtensions.UnwrapReturnType(method.ReturnType);

            IEnumerable<string> transformNames = null;

            if (method.ReturnParameter.IsDefined(typeof(TupleElementNamesAttribute)))
            {
                TupleElementNamesAttribute attribute =
                    method.ReturnParameter.GetCustomAttribute<TupleElementNamesAttribute>();

                transformNames = attribute.TransformNames;
            }

            return ArgumentUnpackerHelper.GetValueTupleArgumentUnpacker
                (tupleType, transformNames);
        }
    }
}