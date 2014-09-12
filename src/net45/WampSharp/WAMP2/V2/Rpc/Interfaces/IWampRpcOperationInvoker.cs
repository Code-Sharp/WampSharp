using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public interface IWampRpcOperationInvoker
    {
        void Invoke<TMessage>(IWampRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails options, string procedure);

        void Invoke<TMessage>(IWampRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails options, string procedure, TMessage[] arguments);

        void Invoke<TMessage>(IWampRawRpcOperationCallback caller, IWampFormatter<TMessage> formatter, InvocationDetails options, string procedure, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords);    
    }
}