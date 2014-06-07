using WampSharp.Core.Serialization;
using WampSharp.V2.Client;

namespace WampSharp.V2.Rpc
{
    public interface IWampRpcOperation
    {
        string Procedure { get; }

        void Invoke<TMessage>(IWampRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, TMessage details);

        void Invoke<TMessage>(IWampRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, TMessage options, TMessage[] arguments);

        void Invoke<TMessage>(IWampRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, TMessage options, TMessage[] arguments, TMessage argumentsKeywords);
    }

    public interface IWampRpcOperation<TMessage>
    {
        string Procedure { get; }

        void Invoke(IWampRawRpcOperationCallback caller, TMessage options);

        void Invoke(IWampRawRpcOperationCallback caller, TMessage options, TMessage[] arguments);

        void Invoke(IWampRawRpcOperationCallback caller, TMessage options, TMessage[] arguments, TMessage argumentsKeywords);         
    }
}