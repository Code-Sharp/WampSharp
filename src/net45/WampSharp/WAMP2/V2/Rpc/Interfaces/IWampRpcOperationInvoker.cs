using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public interface IWampRpcOperationInvoker
    {
        void Invoke<TMessage>(IWampRouterRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details, string procedure);

        void Invoke<TMessage>(IWampRouterRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details, string procedure, TMessage[] arguments);

        void Invoke<TMessage>(IWampRouterRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details, string procedure, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords);    

        void Invoke<TMessage>(IWampClientRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details, string procedure);

        void Invoke<TMessage>(IWampClientRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details, string procedure, TMessage[] arguments);

        void Invoke<TMessage>(IWampClientRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails details, string procedure, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords);    
    }
}