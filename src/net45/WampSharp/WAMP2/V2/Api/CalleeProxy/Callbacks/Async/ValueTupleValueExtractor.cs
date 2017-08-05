using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Serialization;
using WampSharp.Core.Utilities.ValueTuple;
using WampSharp.V2.Core;

namespace WampSharp.V2.CalleeProxy
{
    internal class ValueTupleValueExtractor<T> : IOperationResultExtractor<T>
    {
        private readonly ArgumentUnpacker mUnpacker;
        private readonly ValueTupleArrayConverter mConverter;

        public ValueTupleValueExtractor(ArgumentUnpacker unpacker)
        {
            mUnpacker = unpacker;
            mConverter = ValueTupleArrayConverter<T>.Value;
        }

        public T GetResult<TMessage>(IWampFormatter<TMessage> formatter, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            object[] array =
                mUnpacker.UnpackParameters(formatter, arguments, argumentsKeywords)
                .ToArray();

            T result = (T) mConverter.ToTuple(array);

            return result;
        }
    }
}