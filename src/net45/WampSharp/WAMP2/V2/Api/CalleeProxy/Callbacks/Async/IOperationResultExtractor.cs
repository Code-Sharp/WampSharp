using WampSharp.Core.Serialization;

namespace WampSharp.V2.CalleeProxy
{
    internal interface IOperationResultExtractor
    {
        object GetResult<TMessage>(IWampFormatter<TMessage> formatter, TMessage[] arguments);
    }
}