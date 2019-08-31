using System;
using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Serialization;

namespace WampSharp.V2.CalleeProxy
{
    internal class SingleValueExtractor<TResult> : IOperationResultExtractor<TResult>
    {
        private readonly Type mReturnType = typeof(TResult);
        private readonly bool mHasReturnValue;

        public SingleValueExtractor(bool hasReturnValue)
        {
            mHasReturnValue = hasReturnValue;
        }

        public TResult GetResult<TMessage>(IWampFormatter<TMessage> formatter, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            if (!mHasReturnValue || !arguments.Any())
            {
                // TODO: throw exception if not nullable.
                return default(TResult);
            }
            else
            {
                TResult result = formatter.Deserialize<TResult>(arguments[0]);
                return result;
            }
        }
    }
}