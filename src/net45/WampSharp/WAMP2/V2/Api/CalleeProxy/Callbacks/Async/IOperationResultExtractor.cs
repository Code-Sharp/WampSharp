using System.Collections.Generic;
using WampSharp.Core.Serialization;

namespace WampSharp.V2.CalleeProxy
{
    internal interface IOperationResultExtractor<out TResult>
    {
        TResult GetResult<TMessage>(IWampFormatter<TMessage> formatter, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords);
    }
}