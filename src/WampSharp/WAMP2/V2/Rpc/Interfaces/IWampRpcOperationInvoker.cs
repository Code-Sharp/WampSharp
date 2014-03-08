using WampSharp.Core.Serialization;

namespace WampSharp.V2.Rpc
{
    public interface IWampRpcOperationInvoker
    {
        void Invoke<TMessage>(IWampRpcOperationCallback caller, IWampFormatter<TMessage> formatter, TMessage options, string procedure) where TMessage : class;

        void Invoke<TMessage>(IWampRpcOperationCallback caller, IWampFormatter<TMessage> formatter, TMessage options, string procedure, TMessage[] arguments) where TMessage : class;

        void Invoke<TMessage>(IWampRpcOperationCallback caller, IWampFormatter<TMessage> formatter, TMessage options, string procedure, TMessage[] arguments, TMessage argumentsKeywords) where TMessage : class;    
    }
}