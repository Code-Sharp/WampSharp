using WampSharp.Core.Serialization;

namespace WampSharp.V2.Client
{
    public interface IWampRawRpcOperationCallback
    {
        void Result<TMessage>(IWampFormatter<TMessage> formatter, TMessage details);

        void Result<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, TMessage[] arguments);

        void Result<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, TMessage[] arguments, TMessage argumentsKeywords);

        void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error);

        void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error, TMessage[] arguments);

        void Error<TMessage>(IWampFormatter<TMessage> formatter, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords);
    }
}