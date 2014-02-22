using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    // Not the best way to do this, but it should do the trick.
    public static class WampErrorExtensions
    {
        public static void Error<TMessage>
            (this IWampError<TMessage> client,
             WampMessageType messageType,
             long requestId,
             TMessage details,
             string error)
        {
            client.Error((int) messageType, requestId, details, error);
        }

        public static void Error<TMessage>
            (this IWampError<TMessage> client,
             WampMessageType messageType,
             long requestId,
             TMessage details,
             string error,
             TMessage[] arguments)
        {
            client.Error((int) messageType, requestId, details, error, arguments);
        }

        public static void Error<TMessage>
            (this IWampError<TMessage> client,
             WampMessageType messageType,
             long requestId,
             TMessage details,
             string error,
             TMessage[] arguments,
             TMessage argumentsKeywords)
        {
            client.Error((int) messageType, requestId, details, error, arguments, argumentsKeywords);
        }

        public static void RegisterError<TMessage>
            (this IWampError<TMessage> client,
             long requestId,
             TMessage details,
             string error)
        {
            client.Error(WampMessageType.v2Register, requestId, details, error);
        }

        public static void UnregisterError<TMessage>
            (this IWampError<TMessage> client,
             long requestId,
             TMessage details,
             string error)
        {
            client.Error(WampMessageType.v2Unregister, requestId, details, error);
        }

        public static void CallError<TMessage>
            (this IWampError<TMessage> client,
             long requestId,
             TMessage details,
             string error)
        {
            client.Error(WampMessageType.v2Call, requestId, details, error);
        }

        public static void CallError<TMessage>
            (this IWampError<TMessage> client,
             long requestId,
             TMessage details,
             string error,
             TMessage[] arguments)
        {
            client.Error(WampMessageType.v2Call, requestId, details, error, arguments);
        }

        public static void CallError<TMessage>
            (this IWampError<TMessage> client,
             long requestId,
             TMessage details,
             string error,
             TMessage[] arguments,
             TMessage argumentsKeywords)
        {
            client.Error(WampMessageType.v2Call, requestId, details, error, arguments, argumentsKeywords);
        }

        public static void PublishError<TMessage>
            (this IWampError<TMessage> client,
             long requestId,
             TMessage details,
             string error)
        {
            client.Error(WampMessageType.v2Publish, requestId, details, error);
        }

        public static void PublishError<TMessage>
            (this IWampError<TMessage> client,
             long requestId,
             TMessage details,
             string error,
             TMessage[] arguments)
        {
            client.Error(WampMessageType.v2Publish, requestId, details, error, arguments);
        }

        public static void PublishError<TMessage>
            (this IWampError<TMessage> client,
             long requestId,
             TMessage details,
             string error,
             TMessage[] arguments,
             TMessage argumentsKeywords)
        {
            client.Error(WampMessageType.v2Publish, requestId, details, error, arguments, argumentsKeywords);
        }

        public static void SubscribeError<TMessage>
            (this IWampError<TMessage> client,
             long requestId,
             TMessage details,
             string error)
        {
            client.Error(WampMessageType.v2Subscribe, requestId, details, error);
        }

        public static void SubscribeError<TMessage>
            (this IWampError<TMessage> client,
             long requestId,
             TMessage details,
             string error,
             TMessage[] arguments)
        {
            client.Error(WampMessageType.v2Subscribe, requestId, details, error, arguments);
        }

        public static void SubscribeError<TMessage>
            (this IWampError<TMessage> client,
             long requestId,
             TMessage details,
             string error,
             TMessage[] arguments,
             TMessage argumentsKeywords)
        {
            client.Error(WampMessageType.v2Subscribe, requestId, details, error, arguments, argumentsKeywords);
        }

        public static void UnsubscribeError<TMessage>
            (this IWampError<TMessage> client,
             long requestId,
             TMessage details,
             string error)
        {
            client.Error(WampMessageType.v2Unsubscribe, requestId, details, error);
        }

        public static void UnsubscribeError<TMessage>
            (this IWampError<TMessage> client,
             long requestId,
             TMessage details,
             string error,
             TMessage[] arguments)
        {
            client.Error(WampMessageType.v2Unsubscribe, requestId, details, error, arguments);
        }

        public static void UnsubscribeError<TMessage>
            (this IWampError<TMessage> client,
             long requestId,
             TMessage details,
             string error,
             TMessage[] arguments,
             TMessage argumentsKeywords)
        {
            client.Error(WampMessageType.v2Unsubscribe, requestId, details, error, arguments, argumentsKeywords);
        }
    }
}