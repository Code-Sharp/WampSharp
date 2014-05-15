using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    public interface IWampCallerError<TMessage>
    {
        [WampHandler(WampMessageType.v2Call)]
        void CallError(long requestId, TMessage details, string error);

        [WampHandler(WampMessageType.v2Call)]
        void CallError(long requestId, TMessage details, string error, TMessage[] arguments);

        [WampHandler(WampMessageType.v2Call)]
        void CallError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords);
    }
}