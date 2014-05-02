using WampSharp.Core.Serialization;

namespace WampSharp.V2.Rpc
{
    public interface IWampRpcOperation
    {
        string Procedure { get; }

        void Invoke<TMessage>(IWampRpcOperationCallback caller, IWampFormatter<TMessage> formatter, TMessage details);

        void Invoke<TMessage>(IWampRpcOperationCallback caller, IWampFormatter<TMessage> formatter, TMessage options, TMessage[] arguments);

        void Invoke<TMessage>(IWampRpcOperationCallback caller, IWampFormatter<TMessage> formatter, TMessage options, TMessage[] arguments, TMessage argumentsKeywords);
    }

    public interface IWampRpcOperation<TMessage>
    {
        string Procedure { get; }

        void Invoke(IWampRpcOperationCallback caller, TMessage options);

        void Invoke(IWampRpcOperationCallback caller, TMessage options, TMessage[] arguments);

        void Invoke(IWampRpcOperationCallback caller, TMessage options, TMessage[] arguments, TMessage argumentsKeywords);         
    }
}