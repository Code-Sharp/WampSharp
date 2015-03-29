using System.Collections.Generic;
using WampSharp.Core.Client;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Client
{
    internal class ManualWampServerProxyBuilder<TMessage> :
        IWampServerProxyBuilder<TMessage, WampClient<TMessage>, IWampServerProxy>
    {
        private readonly IWampFormatter<TMessage> mFormatter;

        public ManualWampServerProxyBuilder(IWampFormatter<TMessage> formatter)
        {
            mFormatter = formatter;
        }

        private class WampClientIncomingMessageHandler<TMessage>
        {
            private readonly IWampFormatter<TMessage> mFormatter;
            private readonly IWampClient<TMessage> mClient;

            public WampClientIncomingMessageHandler(IWampFormatter<TMessage> formatter, IWampClient<TMessage> client)
            {
                mFormatter = formatter;
                mClient = client;
            }

            public void HandleMessage(WampMessage<TMessage> message)
            {
                switch (message.MessageType)
                {
                    case WampMessageType.v2Challenge:
                        {
                            HandleChallenge(message);
                            break;
                        }
                    case WampMessageType.v2Welcome:
                        {
                            HandleWelcome(message);
                            break;
                        }
                    case WampMessageType.v2Abort:
                        {
                            HandleAbort(message);
                            break;
                        }
                    case WampMessageType.v2Goodbye:
                        {
                            HandleGoodbye(message);
                            break;
                        }
                    case WampMessageType.v2Heartbeat:
                        {
                            HandleHeartbeat(message);
                            break;
                        }
                    case WampMessageType.v2Error:
                        {
                            HandleError(message);
                            break;
                        }
                    case WampMessageType.v2Registered:
                        {
                            HandleRegistered(message);
                            break;
                        }
                    case WampMessageType.v2Unregistered:
                        {
                            HandleUnregistered(message);
                            break;
                        }
                    case WampMessageType.v2Invocation:
                        {
                            HandleInvocation(message);
                            break;
                        }
                    case WampMessageType.v2Interrupt:
                        {
                            HandleInterrupt(message);
                            break;
                        }
                    case WampMessageType.v2Result:
                        {
                            HandleResult(message);
                            break;
                        }
                    case WampMessageType.v2Published:
                        {
                            HandlePublished(message);
                            break;
                        }
                    case WampMessageType.v2Subscribed:
                        {
                            HandleSubscribed(message);
                            break;
                        }
                    case WampMessageType.v2Unsubscribed:
                        {
                            HandleUnsubscribed(message);
                            break;
                        }
                    case WampMessageType.v2Event:
                        {
                            HandleEvent(message);
                            break;
                        }
                    default:
                        {
                            HandleMissingMessage(message);
                            break;
                        }
                }
            }

            protected virtual void HandleMissingMessage(WampMessage<TMessage> message)
            {
            }

            protected virtual void HandleInvalidMessage(WampMessage<TMessage> message)
            {
            }

            private void HandleChallenge(WampMessage<TMessage> message)
            {
                TMessage[] messageArguments = message.Arguments;
                int messageArgumentsLength = messageArguments.Length;

                if (messageArgumentsLength == 2)
                {
                    string authMethod = mFormatter.Deserialize<string>(messageArguments[0]);
                    ChallengeDetails extra = mFormatter.Deserialize<ChallengeDetails>(messageArguments[1]);

                    mClient.Challenge(authMethod, extra);
                }
                else
                {
                    HandleInvalidMessage(message);
                }
            }

            private void HandleWelcome(WampMessage<TMessage> message)
            {
                TMessage[] messageArguments = message.Arguments;
                int messageArgumentsLength = messageArguments.Length;

                if (messageArgumentsLength == 2)
                {
                    long session = mFormatter.Deserialize<long>(messageArguments[0]);
                    TMessage details = mFormatter.Deserialize<TMessage>(messageArguments[1]);

                    mClient.Welcome(session, details);
                }
                else
                {
                    HandleInvalidMessage(message);
                }
            }

            private void HandleAbort(WampMessage<TMessage> message)
            {
                TMessage[] messageArguments = message.Arguments;
                int messageArgumentsLength = messageArguments.Length;

                if (messageArgumentsLength == 2)
                {
                    TMessage details = mFormatter.Deserialize<TMessage>(messageArguments[0]);
                    string reason = mFormatter.Deserialize<string>(messageArguments[1]);

                    mClient.Abort(details, reason);
                }
                else
                {
                    HandleInvalidMessage(message);
                }
            }

            private void HandleGoodbye(WampMessage<TMessage> message)
            {
                TMessage[] messageArguments = message.Arguments;
                int messageArgumentsLength = messageArguments.Length;

                if (messageArgumentsLength == 2)
                {
                    TMessage details = mFormatter.Deserialize<TMessage>(messageArguments[0]);
                    string reason = mFormatter.Deserialize<string>(messageArguments[1]);

                    mClient.Goodbye(details, reason);
                }
                else
                {
                    HandleInvalidMessage(message);
                }
            }

            private void HandleHeartbeat(WampMessage<TMessage> message)
            {
                TMessage[] messageArguments = message.Arguments;
                int messageArgumentsLength = messageArguments.Length;

                if (messageArgumentsLength == 2)
                {
                    int incomingSeq = mFormatter.Deserialize<int>(messageArguments[0]);
                    int outgoingSeq = mFormatter.Deserialize<int>(messageArguments[1]);

                    mClient.Heartbeat(incomingSeq, outgoingSeq);
                }
                else if (messageArgumentsLength == 3)
                {
                    int incomingSeq = mFormatter.Deserialize<int>(messageArguments[0]);
                    int outgoingSeq = mFormatter.Deserialize<int>(messageArguments[1]);
                    string discard = mFormatter.Deserialize<string>(messageArguments[2]);

                    mClient.Heartbeat(incomingSeq, outgoingSeq, discard);
                }
                else
                {
                    HandleInvalidMessage(message);
                }
            }

            private void HandleError(WampMessage<TMessage> message)
            {
                TMessage[] messageArguments = message.Arguments;
                int messageArgumentsLength = messageArguments.Length;

                if (messageArgumentsLength == 4)
                {
                    int requestType = mFormatter.Deserialize<int>(messageArguments[0]);
                    long requestId = mFormatter.Deserialize<long>(messageArguments[1]);
                    TMessage details = mFormatter.Deserialize<TMessage>(messageArguments[2]);
                    string error = mFormatter.Deserialize<string>(messageArguments[3]);

                    mClient.Error(requestType, requestId, details, error);
                }
                else if (messageArgumentsLength == 5)
                {
                    int requestType = mFormatter.Deserialize<int>(messageArguments[0]);
                    long requestId = mFormatter.Deserialize<long>(messageArguments[1]);
                    TMessage details = mFormatter.Deserialize<TMessage>(messageArguments[2]);
                    string error = mFormatter.Deserialize<string>(messageArguments[3]);
                    TMessage[] arguments = mFormatter.Deserialize<TMessage[]>(messageArguments[4]);

                    mClient.Error(requestType, requestId, details, error, arguments);
                }
                else if (messageArgumentsLength == 6)
                {
                    int requestType = mFormatter.Deserialize<int>(messageArguments[0]);
                    long requestId = mFormatter.Deserialize<long>(messageArguments[1]);
                    TMessage details = mFormatter.Deserialize<TMessage>(messageArguments[2]);
                    string error = mFormatter.Deserialize<string>(messageArguments[3]);
                    TMessage[] arguments = mFormatter.Deserialize<TMessage[]>(messageArguments[4]);
                    TMessage argumentsKeywords = mFormatter.Deserialize<TMessage>(messageArguments[5]);

                    mClient.Error(requestType, requestId, details, error, arguments, argumentsKeywords);
                }
                else
                {
                    HandleInvalidMessage(message);
                }
            }

            private void HandleRegistered(WampMessage<TMessage> message)
            {
                TMessage[] messageArguments = message.Arguments;
                int messageArgumentsLength = messageArguments.Length;

                if (messageArgumentsLength == 2)
                {
                    long requestId = mFormatter.Deserialize<long>(messageArguments[0]);
                    long registrationId = mFormatter.Deserialize<long>(messageArguments[1]);

                    mClient.Registered(requestId, registrationId);
                }
                else
                {
                    HandleInvalidMessage(message);
                }
            }

            private void HandleUnregistered(WampMessage<TMessage> message)
            {
                TMessage[] messageArguments = message.Arguments;
                int messageArgumentsLength = messageArguments.Length;

                if (messageArgumentsLength == 1)
                {
                    long requestId = mFormatter.Deserialize<long>(messageArguments[0]);

                    mClient.Unregistered(requestId);
                }
                else
                {
                    HandleInvalidMessage(message);
                }
            }

            private void HandleInvocation(WampMessage<TMessage> message)
            {
                TMessage[] messageArguments = message.Arguments;
                int messageArgumentsLength = messageArguments.Length;

                if (messageArgumentsLength == 3)
                {
                    long requestId = mFormatter.Deserialize<long>(messageArguments[0]);
                    long registrationId = mFormatter.Deserialize<long>(messageArguments[1]);
                    InvocationDetails details = mFormatter.Deserialize<InvocationDetails>(messageArguments[2]);

                    mClient.Invocation(requestId, registrationId, details);
                }
                else if (messageArgumentsLength == 4)
                {
                    long requestId = mFormatter.Deserialize<long>(messageArguments[0]);
                    long registrationId = mFormatter.Deserialize<long>(messageArguments[1]);
                    InvocationDetails details = mFormatter.Deserialize<InvocationDetails>(messageArguments[2]);
                    TMessage[] arguments = mFormatter.Deserialize<TMessage[]>(messageArguments[3]);

                    mClient.Invocation(requestId, registrationId, details, arguments);
                }
                else if (messageArgumentsLength == 5)
                {
                    long requestId = mFormatter.Deserialize<long>(messageArguments[0]);
                    long registrationId = mFormatter.Deserialize<long>(messageArguments[1]);
                    InvocationDetails details = mFormatter.Deserialize<InvocationDetails>(messageArguments[2]);
                    TMessage[] arguments = mFormatter.Deserialize<TMessage[]>(messageArguments[3]);
                    IDictionary<string, TMessage> argumentsKeywords = mFormatter.Deserialize<IDictionary<string, TMessage>>(messageArguments[4]);

                    mClient.Invocation(requestId, registrationId, details, arguments, argumentsKeywords);
                }
                else
                {
                    HandleInvalidMessage(message);
                }
            }

            private void HandleInterrupt(WampMessage<TMessage> message)
            {
                TMessage[] messageArguments = message.Arguments;
                int messageArgumentsLength = messageArguments.Length;

                if (messageArgumentsLength == 2)
                {
                    long requestId = mFormatter.Deserialize<long>(messageArguments[0]);
                    TMessage options = mFormatter.Deserialize<TMessage>(messageArguments[1]);

                    mClient.Interrupt(requestId, options);
                }
                else
                {
                    HandleInvalidMessage(message);
                }
            }

            private void HandleResult(WampMessage<TMessage> message)
            {
                TMessage[] messageArguments = message.Arguments;
                int messageArgumentsLength = messageArguments.Length;

                if (messageArgumentsLength == 2)
                {
                    long requestId = mFormatter.Deserialize<long>(messageArguments[0]);
                    ResultDetails details = mFormatter.Deserialize<ResultDetails>(messageArguments[1]);

                    mClient.Result(requestId, details);
                }
                else if (messageArgumentsLength == 3)
                {
                    long requestId = mFormatter.Deserialize<long>(messageArguments[0]);
                    ResultDetails details = mFormatter.Deserialize<ResultDetails>(messageArguments[1]);
                    TMessage[] arguments = mFormatter.Deserialize<TMessage[]>(messageArguments[2]);

                    mClient.Result(requestId, details, arguments);
                }
                else if (messageArgumentsLength == 4)
                {
                    long requestId = mFormatter.Deserialize<long>(messageArguments[0]);
                    ResultDetails details = mFormatter.Deserialize<ResultDetails>(messageArguments[1]);
                    TMessage[] arguments = mFormatter.Deserialize<TMessage[]>(messageArguments[2]);
                    IDictionary<string, TMessage> argumentsKeywords = mFormatter.Deserialize<IDictionary<string, TMessage>>(messageArguments[3]);

                    mClient.Result(requestId, details, arguments, argumentsKeywords);
                }
                else
                {
                    HandleInvalidMessage(message);
                }
            }

            private void HandlePublished(WampMessage<TMessage> message)
            {
                TMessage[] messageArguments = message.Arguments;
                int messageArgumentsLength = messageArguments.Length;

                if (messageArgumentsLength == 2)
                {
                    long requestId = mFormatter.Deserialize<long>(messageArguments[0]);
                    long publicationId = mFormatter.Deserialize<long>(messageArguments[1]);

                    mClient.Published(requestId, publicationId);
                }
                else
                {
                    HandleInvalidMessage(message);
                }
            }

            private void HandleSubscribed(WampMessage<TMessage> message)
            {
                TMessage[] messageArguments = message.Arguments;
                int messageArgumentsLength = messageArguments.Length;

                if (messageArgumentsLength == 2)
                {
                    long requestId = mFormatter.Deserialize<long>(messageArguments[0]);
                    long subscriptionId = mFormatter.Deserialize<long>(messageArguments[1]);

                    mClient.Subscribed(requestId, subscriptionId);
                }
                else
                {
                    HandleInvalidMessage(message);
                }
            }

            private void HandleUnsubscribed(WampMessage<TMessage> message)
            {
                TMessage[] messageArguments = message.Arguments;
                int messageArgumentsLength = messageArguments.Length;

                if (messageArgumentsLength == 2)
                {
                    long requestId = mFormatter.Deserialize<long>(messageArguments[0]);
                    long subscriptionId = mFormatter.Deserialize<long>(messageArguments[1]);

                    mClient.Unsubscribed(requestId, subscriptionId);
                }
                else
                {
                    HandleInvalidMessage(message);
                }
            }

            private void HandleEvent(WampMessage<TMessage> message)
            {
                TMessage[] messageArguments = message.Arguments;
                int messageArgumentsLength = messageArguments.Length;

                if (messageArgumentsLength == 3)
                {
                    long subscriptionId = mFormatter.Deserialize<long>(messageArguments[0]);
                    long publicationId = mFormatter.Deserialize<long>(messageArguments[1]);
                    EventDetails details = mFormatter.Deserialize<EventDetails>(messageArguments[2]);

                    mClient.Event(subscriptionId, publicationId, details);
                }
                else if (messageArgumentsLength == 4)
                {
                    long subscriptionId = mFormatter.Deserialize<long>(messageArguments[0]);
                    long publicationId = mFormatter.Deserialize<long>(messageArguments[1]);
                    EventDetails details = mFormatter.Deserialize<EventDetails>(messageArguments[2]);
                    TMessage[] arguments = mFormatter.Deserialize<TMessage[]>(messageArguments[3]);

                    mClient.Event(subscriptionId, publicationId, details, arguments);
                }
                else if (messageArgumentsLength == 5)
                {
                    long subscriptionId = mFormatter.Deserialize<long>(messageArguments[0]);
                    long publicationId = mFormatter.Deserialize<long>(messageArguments[1]);
                    EventDetails details = mFormatter.Deserialize<EventDetails>(messageArguments[2]);
                    TMessage[] arguments = mFormatter.Deserialize<TMessage[]>(messageArguments[3]);
                    IDictionary<string, TMessage> argumentsKeywords = mFormatter.Deserialize<IDictionary<string, TMessage>>(messageArguments[4]);

                    mClient.Event(subscriptionId, publicationId, details, arguments, argumentsKeywords);
                }
                else
                {
                    HandleInvalidMessage(message);
                }
            }
        }

        private class WampProtocol
        {
            public WampMessage<object> Challenge(string authMethod, ChallengeDetails extra)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Challenge;

                result.Arguments = new object[] { authMethod, extra };

                return result;
            }

            public WampMessage<object> Welcome<T>(long session, T details)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Welcome;

                result.Arguments = new object[] { session, details };

                return result;
            }

            public WampMessage<object> Abort<T>(T details, string reason)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Abort;

                result.Arguments = new object[] { details, reason };

                return result;
            }

            public WampMessage<object> Goodbye<T>(T details, string reason)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Goodbye;

                result.Arguments = new object[] { details, reason };

                return result;
            }

            public WampMessage<object> Heartbeat(int incomingSeq, int outgoingSeq)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Heartbeat;

                result.Arguments = new object[] { incomingSeq, outgoingSeq };

                return result;
            }

            public WampMessage<object> Heartbeat(int incomingSeq, int outgoingSeq, string discard)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Heartbeat;

                result.Arguments = new object[] { incomingSeq, outgoingSeq, discard };

                return result;
            }

            public WampMessage<object> Error<T>(int requestType, long requestId, T details, string error)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Error;

                result.Arguments = new object[] { requestType, requestId, details, error };

                return result;
            }

            public WampMessage<object> Error<T>(int requestType, long requestId, T details, string error, T[] arguments)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Error;

                result.Arguments = new object[] { requestType, requestId, details, error, arguments };

                return result;
            }

            public WampMessage<object> Error<T>(int requestType, long requestId, T details, string error, T[] arguments,
                T argumentsKeywords)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Error;

                result.Arguments = new object[] { requestType, requestId, details, error, arguments, argumentsKeywords };

                return result;
            }

            public WampMessage<object> Registered(long requestId, long registrationId)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Registered;

                result.Arguments = new object[] { requestId, registrationId };

                return result;
            }

            public WampMessage<object> Unregistered(long requestId)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Unregistered;

                result.Arguments = new object[] { requestId };

                return result;
            }

            public WampMessage<object> Invocation(long requestId, long registrationId, InvocationDetails details)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Invocation;

                result.Arguments = new object[] { requestId, registrationId, details };

                return result;
            }

            public WampMessage<object> Invocation<T>(long requestId, long registrationId, InvocationDetails details,
                T[] arguments)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Invocation;

                result.Arguments = new object[] { requestId, registrationId, details, arguments };

                return result;
            }

            public WampMessage<object> Invocation<T>(long requestId, long registrationId, InvocationDetails details,
                T[] arguments, IDictionary<string, object> argumentsKeywords)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Invocation;

                result.Arguments = new object[] { requestId, registrationId, details, arguments, argumentsKeywords };

                return result;
            }

            public WampMessage<object> Interrupt<T>(long requestId, T options)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Interrupt;

                result.Arguments = new object[] { requestId, options };

                return result;
            }

            public WampMessage<object> Result(long requestId, ResultDetails details)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Result;

                result.Arguments = new object[] { requestId, details };

                return result;
            }

            public WampMessage<object> Result<T>(long requestId, ResultDetails details, T[] arguments)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Result;

                result.Arguments = new object[] { requestId, details, arguments };

                return result;
            }

            public WampMessage<object> Result<T>(long requestId, ResultDetails details, T[] arguments,
                IDictionary<string, object> argumentsKeywords)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Result;

                result.Arguments = new object[] { requestId, details, arguments, argumentsKeywords };

                return result;
            }

            public WampMessage<object> Published(long requestId, long publicationId)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Published;

                result.Arguments = new object[] { requestId, publicationId };

                return result;
            }

            public WampMessage<object> Subscribed(long requestId, long subscriptionId)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Subscribed;

                result.Arguments = new object[] { requestId, subscriptionId };

                return result;
            }

            public WampMessage<object> Unsubscribed(long requestId, long subscriptionId)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Unsubscribed;

                result.Arguments = new object[] { requestId, subscriptionId };

                return result;
            }

            public WampMessage<object> Event(long subscriptionId, long publicationId, EventDetails details)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Event;

                result.Arguments = new object[] { subscriptionId, publicationId, details };

                return result;
            }

            public WampMessage<object> Event<T>(long subscriptionId, long publicationId, EventDetails details, T[] arguments)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Event;

                result.Arguments = new object[] { subscriptionId, publicationId, details, arguments };

                return result;
            }

            public WampMessage<object> Event<T>(long subscriptionId, long publicationId, EventDetails details, T[] arguments,
                IDictionary<string, object> argumentsKeywords)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Event;

                result.Arguments = new object[] { subscriptionId, publicationId, details, arguments, argumentsKeywords };

                return result;
            }

            public WampMessage<object> Hello<T>(string realm, T details)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Hello;

                result.Arguments = new object[] { realm, details };

                return result;
            }

            public WampMessage<object> Authenticate<T>(string signature, T extra)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Authenticate;

                result.Arguments = new object[] { signature, extra };

                return result;
            }

            public WampMessage<object> Register(long requestId, RegisterOptions options, string procedure)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Register;

                result.Arguments = new object[] { requestId, options, procedure };

                return result;
            }

            public WampMessage<object> Unregister(long requestId, long registrationId)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Unregister;

                result.Arguments = new object[] { requestId, registrationId };

                return result;
            }

            public WampMessage<object> Call(long requestId, CallOptions options, string procedure)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Call;

                result.Arguments = new object[] { requestId, options, procedure };

                return result;
            }

            public WampMessage<object> Call<T>(long requestId, CallOptions options, string procedure, T[] arguments)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Call;

                result.Arguments = new object[] { requestId, options, procedure, arguments };

                return result;
            }

            public WampMessage<object> Call<T>(long requestId, CallOptions options, string procedure, T[] arguments,
                IDictionary<string, object> argumentsKeywords)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Call;

                result.Arguments = new object[] { requestId, options, procedure, arguments, argumentsKeywords };

                return result;
            }

            public WampMessage<object> Cancel(long requestId, CancelOptions options)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Cancel;

                result.Arguments = new object[] { requestId, options };

                return result;
            }

            public WampMessage<object> Yield(long requestId, YieldOptions options)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Yield;

                result.Arguments = new object[] { requestId, options };

                return result;
            }

            public WampMessage<object> Yield<T>(long requestId, YieldOptions options, T[] arguments)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Yield;

                result.Arguments = new object[] { requestId, options, arguments };

                return result;
            }

            public WampMessage<object> Yield<T>(long requestId, YieldOptions options, T[] arguments,
                IDictionary<string, object> argumentsKeywords)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Yield;

                result.Arguments = new object[] { requestId, options, arguments, argumentsKeywords };

                return result;
            }

            public WampMessage<object> Publish(long requestId, PublishOptions options, string topicUri)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Publish;

                result.Arguments = new object[] { requestId, options, topicUri };

                return result;
            }

            public WampMessage<object> Publish<T>(long requestId, PublishOptions options, string topicUri, T[] arguments)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Publish;

                result.Arguments = new object[] { requestId, options, topicUri, arguments };

                return result;
            }

            public WampMessage<object> Publish<T>(long requestId, PublishOptions options, string topicUri, T[] arguments,
                IDictionary<string, object> argumentKeywords)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Publish;

                result.Arguments = new object[] { requestId, options, topicUri, arguments, argumentKeywords };

                return result;
            }

            public WampMessage<object> Subscribe(long requestId, SubscribeOptions options, string topicUri)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Subscribe;

                result.Arguments = new object[] { requestId, options, topicUri };

                return result;
            }

            public WampMessage<object> Unsubscribe(long requestId, long subscriptionId)
            {
                WampMessage<object> result = new WampMessage<object>();

                result.MessageType = WampMessageType.v2Unsubscribe;

                result.Arguments = new object[] { requestId, subscriptionId };

                return result;
            }
        }

        private class WampServerProxy<TMessage> : IWampServerProxy
        {
            private readonly IWampConnection<TMessage> mConnection;
            private readonly WampProtocol mProtocol;

            public WampServerProxy(IWampConnection<TMessage> connection, WampProtocol protocol)
            {
                mConnection = connection;
                mProtocol = protocol;
            }

            public void Publish(long requestId, PublishOptions options, string topicUri)
            {
                WampMessage<object> messageToSend = mProtocol.Publish(requestId, options, topicUri);
                mConnection.Send(messageToSend);
            }

            public void Publish(long requestId, PublishOptions options, string topicUri, object[] arguments)
            {
                WampMessage<object> messageToSend = mProtocol.Publish(requestId, options, topicUri, arguments);
                mConnection.Send(messageToSend);
            }

            public void Publish(long requestId, PublishOptions options, string topicUri, object[] arguments, IDictionary<string, object> argumentKeywords)
            {
                WampMessage<object> messageToSend = mProtocol.Publish(requestId, options, topicUri, arguments, argumentKeywords);
                mConnection.Send(messageToSend);
            }

            public void Subscribe(long requestId, SubscribeOptions options, string topicUri)
            {
                WampMessage<object> messageToSend = mProtocol.Subscribe(requestId, options, topicUri);
                mConnection.Send(messageToSend);
            }

            public void Unsubscribe(long requestId, long subscriptionId)
            {
                WampMessage<object> messageToSend = mProtocol.Unsubscribe(requestId, subscriptionId);
                mConnection.Send(messageToSend);
            }

            public void Register(long requestId, RegisterOptions options, string procedure)
            {
                WampMessage<object> messageToSend = mProtocol.Register(requestId, options, procedure);
                mConnection.Send(messageToSend);
            }

            public void Unregister(long requestId, long registrationId)
            {
                WampMessage<object> messageToSend = mProtocol.Unregister(requestId, registrationId);
                mConnection.Send(messageToSend);
            }

            public void Call(long requestId, CallOptions options, string procedure)
            {
                WampMessage<object> messageToSend = mProtocol.Call(requestId, options, procedure);
                mConnection.Send(messageToSend);
            }

            public void Call(long requestId, CallOptions options, string procedure, object[] arguments)
            {
                WampMessage<object> messageToSend = mProtocol.Call(requestId, options, procedure, arguments);
                mConnection.Send(messageToSend);
            }

            public void Call(long requestId, CallOptions options, string procedure, object[] arguments, IDictionary<string, object> argumentsKeywords)
            {
                WampMessage<object> messageToSend = mProtocol.Call(requestId, options, procedure, arguments, argumentsKeywords);
                mConnection.Send(messageToSend);
            }

            public void Cancel(long requestId, CancelOptions options)
            {
                WampMessage<object> messageToSend = mProtocol.Cancel(requestId, options);
                mConnection.Send(messageToSend);
            }

            public void Yield(long requestId, YieldOptions options)
            {
                WampMessage<object> messageToSend = mProtocol.Yield(requestId, options);
                mConnection.Send(messageToSend);
            }

            public void Yield(long requestId, YieldOptions options, object[] arguments)
            {
                WampMessage<object> messageToSend = mProtocol.Yield(requestId, options, arguments);
                mConnection.Send(messageToSend);
            }

            public void Yield(long requestId, YieldOptions options, object[] arguments, IDictionary<string, object> argumentsKeywords)
            {
                WampMessage<object> messageToSend = mProtocol.Yield(requestId, options, arguments, argumentsKeywords);
                mConnection.Send(messageToSend);
            }

            public void Hello(string realm, object details)
            {
                WampMessage<object> messageToSend = mProtocol.Hello(realm, details);
                mConnection.Send(messageToSend);
            }

            public void Abort(AbortDetails details, string reason)
            {
                WampMessage<object> messageToSend = mProtocol.Abort(details, reason);
                mConnection.Send(messageToSend);
            }

            public void Authenticate(string signature, IDictionary<string, object> extra)
            {
                WampMessage<object> messageToSend = mProtocol.Authenticate(signature, extra);
                mConnection.Send(messageToSend);
            }

            public void Goodbye(object details, string reason)
            {
                WampMessage<object> messageToSend = mProtocol.Goodbye(details, reason);
                mConnection.Send(messageToSend);
            }

            public void Heartbeat(int incomingSeq, int outgoingSeq)
            {
                WampMessage<object> messageToSend = mProtocol.Heartbeat(incomingSeq, outgoingSeq);
                mConnection.Send(messageToSend);
            }

            public void Heartbeat(int incomingSeq, int outgoingSeq, string discard)
            {
                WampMessage<object> messageToSend = mProtocol.Heartbeat(incomingSeq, outgoingSeq, discard);
                mConnection.Send(messageToSend);
            }

            public void Error(int requestType, long requestId, object details, string error)
            {
                WampMessage<object> messageToSend = mProtocol.Error(requestType, requestId, details, error);
                mConnection.Send(messageToSend);
            }

            public void Error(int requestType, long requestId, object details, string error, object[] arguments)
            {
                WampMessage<object> messageToSend = mProtocol.Error(requestType, requestId, details, error, arguments);
                mConnection.Send(messageToSend);
            }

            public void Error(int requestType, long requestId, object details, string error, object[] arguments, object argumentsKeywords)
            {
                WampMessage<object> messageToSend = mProtocol.Error(requestType, requestId, details, error, arguments, argumentsKeywords);
                mConnection.Send(messageToSend);
            }
        }

        private class WampServerProxyImpl<TMessage> : WampServerProxy<TMessage>
        {
            private readonly WampClientIncomingMessageHandler<TMessage> mIncomingMessageHandler;

            public WampServerProxyImpl(IWampConnection<TMessage> connection, WampProtocol protocol, WampClient<TMessage> client, IWampFormatter<TMessage> formatter) : 
                base(connection, protocol)
            {
                connection.MessageArrived += OnMessageArrived;
                mIncomingMessageHandler = new WampClientIncomingMessageHandler<TMessage>(formatter, client);
            }

            private void OnMessageArrived(object sender, WampMessageArrivedEventArgs<TMessage> e)
            {
                mIncomingMessageHandler.HandleMessage(e.Message);
            }
        }

        public IWampServerProxy Create(WampClient<TMessage> client, IWampConnection<TMessage> connection)
        {
            return new WampServerProxyImpl<TMessage>(connection, new WampProtocol(), client, mFormatter);
        }
    }
}