using WampSharp.Core.Contracts;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    public interface IWampSubscriberError<TMessage>
    {
        [WampHandler(WampMessageType.v2Subscribe)]
        void SubscribeError(long requestId, TMessage details, string error);

        [WampHandler(WampMessageType.v2Subscribe)]
        void SubscribeError(long requestId, TMessage details, string error, TMessage[] arguments);

        [WampHandler(WampMessageType.v2Subscribe)]
        void SubscribeError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords);

        [WampHandler(WampMessageType.v2Unsubscribe)]
        void UnsubscribeError(long requestId, TMessage details, string error);

        [WampHandler(WampMessageType.v2Unsubscribe)]
        void UnsubscribeError(long requestId, TMessage details, string error, TMessage[] arguments);

        [WampHandler(WampMessageType.v2Unsubscribe)]
        void UnsubscribeError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords);
    }
}