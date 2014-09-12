using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public interface IWampRpcOperation
    {
        string Procedure { get; }

        void Invoke<TMessage>(IWampRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details);

        void Invoke<TMessage>(IWampRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details, TMessage[] arguments);

        void Invoke<TMessage>(IWampRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords);
    }

    public interface IWampRpcOperation<TMessage>
    {
        string Procedure { get; }

        void Invoke(IWampRawRpcOperationCallback caller, InvocationDetails details);

        void Invoke(IWampRawRpcOperationCallback caller, InvocationDetails details, TMessage[] arguments);

        void Invoke(IWampRawRpcOperationCallback caller, InvocationDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords);         
    }
}