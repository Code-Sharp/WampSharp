using System;
using System.Linq;
using WampSharp.Core.Serialization;

namespace WampSharp.V2.CalleeProxy
{
    internal class MultiValueExtractor<TResult> : IOperationResultExtractor<TResult[]>
    {
        private readonly Type mReturnType = typeof(TResult[]);
        private readonly Type mElementType = typeof(TResult);

        public TResult[] GetResult<TMessage>(IWampFormatter<TMessage> formatter, TMessage[] arguments)
        {
            if (!arguments.Any())
            {
                return null;
            }
            else
            {
                TResult[] deserialized =
                    arguments.Select(x => formatter.Deserialize<TResult>(x))
                             .ToArray();

                return deserialized;
            }
        }
    }
}