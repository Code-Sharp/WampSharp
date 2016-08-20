using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using WampSharp.Core.Serialization;
using WampSharp.Core.Utilities;
using WampSharp.Core.Utilities.ValueTuple;
using WampSharp.V2.Core;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.CalleeProxy
{
    internal interface IOperationResultExtractor<out TResult>
    {
        TResult GetResult<TMessage>(IWampFormatter<TMessage> formatter, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords);
    }

    // TODO: Move this to its own file
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
            ArgumentUnpacker unpacker = GetTupleArgumentUnpacker<T>(method);

            return new ValueTupleValueExtractor<T>(unpacker);
        }

        private static ArgumentUnpacker GetTupleArgumentUnpacker<T>(MethodInfo method)
        {
            IEnumerable<LocalParameter> localParameters;
            IEnumerable<string> transformNames;

            bool skipPositionalArguments = false;

            if (!method.ReturnParameter.IsDefined(typeof(TupleElementNamesAttribute)))
            {
                transformNames = Enumerable.Repeat(default(string), typeof(T).GetValueTupleLength());
            }
            else
            {
                TupleElementNamesAttribute attribute =
                    method.ReturnParameter.GetCustomAttribute<TupleElementNamesAttribute>();

                transformNames = attribute.TransformNames;

                skipPositionalArguments = true;
            }

            var argumentTypes =
                typeof(T).GetGenericArguments()
                         .Select((type, index) => new { type, index });

            localParameters =
                transformNames
                    .Zip
                    (argumentTypes,
                     (name, typeToIndex) =>
                         new LocalParameter(name,
                                            typeToIndex.type,
                                            typeToIndex.index));

            ArgumentUnpacker result = new ArgumentUnpacker(localParameters.ToArray());

            result.SkipPositionalArguments = skipPositionalArguments;

            return result;
        }
    }
}