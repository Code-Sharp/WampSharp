using System;
using System.Linq;
using WampSharp.Core.Serialization;

namespace WampSharp.V2.CalleeProxy
{
    internal class MultiValueExtractor : IOperationResultExtractor
    {
        private readonly Type mReturnType;
        private readonly Type mElementType;

        public MultiValueExtractor(Type returnType)
        {
            mReturnType = returnType;

            mElementType = returnType.GetElementType();
        }

        public object GetResult<TMessage>(IWampFormatter<TMessage> formatter, TMessage[] arguments)
        {
            if (!arguments.Any())
            {
                return null;
            }
            else
            {
                object[] deserialized =
                    arguments.Select(x => formatter.Deserialize(mElementType, x))
                             .ToArray();

                Array array =
                    Array.CreateInstance(mElementType, deserialized.Length);

                deserialized.CopyTo(array, 0);

                return array;
            }
        }
    }
}