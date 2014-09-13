using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Rpc
{
    public interface IWampRouterRawRpcOperationCallback : IWampRawRpcOperationCallback<YieldOptions>
    {
    }

    public interface IWampClientRawRpcOperationCallback : IWampRawRpcOperationCallback<ResultDetails>
    {
    }


    public interface IWampRawRpcOperationCallback<TDetailsOptions>
    {
        void Result<TMessage>(IWampFormatter<TMessage> formatter, TDetailsOptions details);

        void Result<TMessage>(IWampFormatter<TMessage> formatter, TDetailsOptions details, TMessage[] arguments);

        void Result<TMessage>(IWampFormatter<TMessage> formatter, TDetailsOptions details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords);

        void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error);

        void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error, TMessage[] arguments);

        void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords);
    }
}