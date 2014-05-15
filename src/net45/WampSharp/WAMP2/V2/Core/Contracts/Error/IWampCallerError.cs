using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    public interface IWampCallerError<TMessage>
    {
        [WampErrorHandler(WampMessageType.v2Call)]
        void CallError(long requestId, TMessage details, string error);

        [WampErrorHandler(WampMessageType.v2Call)]
        void CallError(long requestId, TMessage details, string error, TMessage[] arguments);

        [WampErrorHandler(WampMessageType.v2Call)]
        void CallError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords);
    }
}