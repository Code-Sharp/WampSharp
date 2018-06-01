using WampSharp.Core.Message;
using WampSharp.V2.Error;

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

        private static void WampExceptionError
            (this IWampError<object> client,
             WampMessageType messageType,
             long requestId,
             WampException exception)
        {
            IWampErrorCallback callback = new WampErrorCallback(client, messageType, requestId);

            callback.Error(exception);
        }

        public static void Error
            (this IWampError<object> client,
             WampMessageType messageType,
             long requestId,
             WampException exception)
        {
            client.WampExceptionError(messageType, requestId, exception);
        }

        public static void RegisterError<TMessage>
            (this IWampError<TMessage> client,
             long requestId,
             TMessage details,
             string error)
        {
            client.Error(WampMessageType.v2Register, requestId, details, error);
        }

        public static void RegisterError
            (this IWampError<object> client,
             long requestId,
             WampException exception)
        {
            client.Error(WampMessageType.v2Register, requestId, exception);
        }

        public static void UnregisterError<TMessage>
            (this IWampError<TMessage> client,
             long requestId,
             TMessage details,
             string error)
        {
            client.Error(WampMessageType.v2Unregister, requestId, details, error);
        }

        public static void UnregisterError
            (this IWampError<object> client,
             long requestId,
             WampException exception)
        {
            client.Error(WampMessageType.v2Unregister, requestId, exception);
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

        public static void CallError
            (this IWampError<object> client,
             long requestId,
             WampException exception)
        {
            client.Error(WampMessageType.v2Call, requestId, exception);
        }

        public static void InvocationError<TMessage>
            (this IWampError<TMessage> client,
             long requestId,
             TMessage details,
             string error)
        {
            client.Error(WampMessageType.v2Invocation, requestId, details, error);
        }

        public static void InvocationError<TMessage>
            (this IWampError<TMessage> client,
             long requestId,
             TMessage details,
             string error,
             TMessage[] arguments)
        {
            client.Error(WampMessageType.v2Invocation, requestId, details, error, arguments);
        }

        public static void InvocationError<TMessage>
            (this IWampError<TMessage> client,
             long requestId,
             TMessage details,
             string error,
             TMessage[] arguments,
             TMessage argumentsKeywords)
        {
            client.Error(WampMessageType.v2Invocation, requestId, details, error, arguments, argumentsKeywords);
        }

        public static void InvocationError
            (this IWampError<object> client,
             long requestId,
             WampException exception)
        {
            client.Error(WampMessageType.v2Invocation, requestId, exception);
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

        public static void PublishError
            (this IWampError<object> client,
             long requestId,
             WampException exception)
        {
            client.Error(WampMessageType.v2Publish, requestId, exception);
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

        public static void SubscribeError
            (this IWampError<object> client,
             long requestId,
             WampException exception)
        {
            client.Error(WampMessageType.v2Subscribe, requestId, exception);
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

        public static void UnsubscribeError
            (this IWampError<object> client,
             long requestId,
             WampException exception)
        {
            client.Error(WampMessageType.v2Unsubscribe, requestId, exception);
        }

        private class WampErrorCallback : IWampErrorCallback
        {
            private readonly IWampError<object> mCallback;
            private readonly WampMessageType mMessageType;
            private readonly long mRequestId;

            public WampErrorCallback(IWampError<object> callback, WampMessageType messageType, long requestId)
            {
                mCallback = callback;
                mMessageType = messageType;
                mRequestId = requestId;
            }

            public void Error(object details, string errorUri)
            {
                mCallback.Error(mMessageType, mRequestId, details, errorUri);
            }

            public void Error(object details, string errorUri, object[] arguments)
            {
                mCallback.Error(mMessageType, mRequestId, details, errorUri, arguments);
            }

            public void Error(object details, string errorUri, object[] arguments, object argumentsKeywords)
            {
                mCallback.Error(mMessageType, mRequestId, details, errorUri, arguments, argumentsKeywords);
            }
        }
    }
}