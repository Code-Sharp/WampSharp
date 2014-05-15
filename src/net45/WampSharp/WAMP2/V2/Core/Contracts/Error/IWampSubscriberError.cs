using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    public interface IWampSubscriberError<TMessage>
    {
        [WampErrorHandler(WampMessageType.v2Subscribe)]
        void SubscribeError(long requestId, TMessage details, string error);

        [WampErrorHandler(WampMessageType.v2Subscribe)]
        void SubscribeError(long requestId, TMessage details, string error, TMessage[] arguments);

        [WampErrorHandler(WampMessageType.v2Subscribe)]
        void SubscribeError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords);

        [WampErrorHandler(WampMessageType.v2Unsubscribe)]
        void UnsubscribeError(long requestId, TMessage details, string error);

        [WampErrorHandler(WampMessageType.v2Unsubscribe)]
        void UnsubscribeError(long requestId, TMessage details, string error, TMessage[] arguments);

        [WampErrorHandler(WampMessageType.v2Unsubscribe)]
        void UnsubscribeError(long requestId, TMessage details, string error, TMessage[] arguments, TMessage argumentsKeywords);
    }
}