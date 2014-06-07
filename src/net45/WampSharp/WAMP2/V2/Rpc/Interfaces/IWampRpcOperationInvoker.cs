using WampSharp.Core.Serialization;
using WampSharp.V2.Client;

namespace WampSharp.V2.Rpc
{
    public interface IWampRpcOperationInvoker
    {
        void Invoke<TMessage>(IWampRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, TMessage options, string procedure);

        void Invoke<TMessage>(IWampRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, TMessage options, string procedure, TMessage[] arguments);

        void Invoke<TMessage>(IWampRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, TMessage options, string procedure, TMessage[] arguments, TMessage argumentsKeywords);    
    }
}