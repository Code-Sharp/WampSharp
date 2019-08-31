using WampSharp.Core.Message;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Client
{
    internal class ErrorForwarder<TMessage> : IWampError<TMessage>
    {
        private readonly WampClient<TMessage> mInstance;

        public ErrorForwarder(WampClient<TMessage> instance)
        {
            mInstance = instance;
        }

        // No reflection this time.
        public void Error(int requestType, long requestId, TMessage details, string error)
        {
            WampMessageType messageType = (WampMessageType) requestType;

            switch (messageType)
            {
                case WampMessageType.v2Register:
                    {
                        mInstance.RegisterError(requestId, details, error);
                        break;
                    }
                case WampMessageType.v2Unregister:
                    {
                        mInstance.UnregisterError(requestId, details, error);
                        break;
                    }
                case WampMessageType.v2Call:
                    {
                        mInstance.CallError(requestId, details, error);
                        break;
                    }
                case WampMessageType.v2Subscribe:
                    {
                        mInstance.SubscribeError(requestId, details, error);
                        break;
                    }
                case WampMessageType.v2Unsubscribe:
                    {
                        mInstance.UnsubscribeError(requestId, details, error);
                        break;
                    }
                case WampMessageType.v2Publish:
                    {
                        mInstance.PublishError(requestId, details, error);
                        break;
                    }
            }
        }

        public void Error(int requestType, long requestId, TMessage details, string error, TMessage[] arguments)
        {
            WampMessageType messageType = (WampMessageType) requestType;

            switch (messageType)
            {
                case WampMessageType.v2Register:
                    {
                        mInstance.RegisterError(requestId, details, error, arguments);
                        break;
                    }
                case WampMessageType.v2Unregister:
                    {
                        mInstance.UnregisterError(requestId, details, error, arguments);
                        break;
                    }
                case WampMessageType.v2Call:
                    {
                        mInstance.CallError(requestId, details, error, arguments);
                        break;
                    }
                case WampMessageType.v2Subscribe:
                    {
                        mInstance.SubscribeError(requestId, details, error, arguments);
                        break;
                    }
                case WampMessageType.v2Unsubscribe:
                    {
                        mInstance.UnsubscribeError(requestId, details, error, arguments);
                        break;
                    }
                case WampMessageType.v2Publish:
                    {
                        mInstance.PublishError(requestId, details, error, arguments);
                        break;
                    }
            }
        }

        public void Error(int requestType, long requestId, TMessage details, string error, TMessage[] arguments,
                          TMessage argumentsKeywords)
        {
            WampMessageType messageType = (WampMessageType) requestType;

            switch (messageType)
            {
                case WampMessageType.v2Register:
                    {
                        mInstance.RegisterError(requestId, details, error, arguments, argumentsKeywords);
                        break;
                    }
                case WampMessageType.v2Unregister:
                    {
                        mInstance.UnregisterError(requestId, details, error, arguments, argumentsKeywords);
                        break;
                    }
                case WampMessageType.v2Call:
                    {
                        mInstance.CallError(requestId, details, error, arguments, argumentsKeywords);
                        break;
                    }
                case WampMessageType.v2Subscribe:
                    {
                        mInstance.SubscribeError(requestId, details, error, arguments, argumentsKeywords);
                        break;
                    }
                case WampMessageType.v2Unsubscribe:
                    {
                        mInstance.UnsubscribeError(requestId, details, error, arguments, argumentsKeywords);
                        break;
                    }
                case WampMessageType.v2Publish:
                    {
                        mInstance.PublishError(requestId, details, error, arguments, argumentsKeywords);
                        break;
                    }
            }
        }
    }
}