using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public interface IWampRpcOperationProxy
    {
        string Procedure { get; }

        void Invoke<TMessage>(IWampClientRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details);

        void Invoke<TMessage>(IWampClientRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details, TMessage[] arguments);

        void Invoke<TMessage>(IWampClientRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords);
    }

    
    public interface IWampRpcOperation
    {
        string Procedure { get; }

        void Invoke<TMessage>(IWampRouterRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details);

        void Invoke<TMessage>(IWampRouterRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details, TMessage[] arguments);

        void Invoke<TMessage>(IWampRouterRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords);
    }

    public interface IWampRpcOperation<TMessage>
    {
        string Procedure { get; }

        void Invoke(IWampRouterRawRpcOperationCallback caller, InvocationDetails details);

        void Invoke(IWampRouterRawRpcOperationCallback caller, InvocationDetails details, TMessage[] arguments);

        void Invoke(IWampRouterRawRpcOperationCallback caller, InvocationDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords);         
    }
}