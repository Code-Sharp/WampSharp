using WampSharp.Core.Serialization;

namespace WampSharp.V2.CalleeProxy
{
#if PCL
    public interface IOperationResultExtractor<out TResult>
#else
    internal interface IOperationResultExtractor<out TResult>
#endif
    {
        TResult GetResult<TMessage>(IWampFormatter<TMessage> formatter, TMessage[] arguments);
    }
}