using System.Collections.Generic;
using System.Reflection;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;
using WampSharp.Core.Utilities;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Core.Serialization
{
    internal class WampProtocol : IWampEventSerializer
    {
        private static readonly MethodInfo mChallenge2 = Method.Get((IWampClientProxy proxy) => proxy.Challenge(default(string), default(ChallengeDetails)));
        private static readonly MethodInfo mWelcome2 = Method.Get((IWampClientProxy proxy) => proxy.Welcome(default(long), default(WelcomeDetails)));
        private static readonly MethodInfo mAbort2 = Method.Get((IWampClientProxy proxy) => proxy.Abort(default(AbortDetails), default(string)));
        private static readonly MethodInfo mGoodbye2 = Method.Get((IWampClientProxy proxy) => proxy.Goodbye(default(GoodbyeDetails), default(string)));
        private static readonly MethodInfo mError4 = Method.Get((IWampClientProxy proxy) => proxy.Error(default(int), default(long), default(object), default(string)));
        private static readonly MethodInfo mError5 = Method.Get((IWampClientProxy proxy) => proxy.Error(default(int), default(long), default(object), default(string), default(object[])));
        private static readonly MethodInfo mError6 = Method.Get((IWampClientProxy proxy) => proxy.Error(default(int), default(long), default(object), default(string), default(object[]), default(object)));
        private static readonly MethodInfo mRegistered2 = Method.Get((IWampClientProxy proxy) => proxy.Registered(default(long), default(long)));
        private static readonly MethodInfo mUnregistered1 = Method.Get((IWampClientProxy proxy) => proxy.Unregistered(default(long)));
        private static readonly MethodInfo mInvocation3 = Method.Get((IWampClientProxy proxy) => proxy.Invocation(default(long), default(long), default(InvocationDetails)));
        private static readonly MethodInfo mInvocation4 = Method.Get((IWampClientProxy proxy) => proxy.Invocation(default(long), default(long), default(InvocationDetails), default(object[])));
        private static readonly MethodInfo mInvocation5 = Method.Get((IWampClientProxy proxy) => proxy.Invocation(default(long), default(long), default(InvocationDetails), default(object[]), default(IDictionary<string, object>)));
        private static readonly MethodInfo mInterrupt2 = Method.Get((IWampClientProxy proxy) => proxy.Interrupt(default(long), default(InterruptDetails)));
        private static readonly MethodInfo mResult2 = Method.Get((IWampClientProxy proxy) => proxy.Result(default(long), default(ResultDetails)));
        private static readonly MethodInfo mResult3 = Method.Get((IWampClientProxy proxy) => proxy.Result(default(long), default(ResultDetails), default(object[])));
        private static readonly MethodInfo mResult4 = Method.Get((IWampClientProxy proxy) => proxy.Result(default(long), default(ResultDetails), default(object[]), default(IDictionary<string, object>)));
        private static readonly MethodInfo mPublished2 = Method.Get((IWampClientProxy proxy) => proxy.Published(default(long), default(long)));
        private static readonly MethodInfo mSubscribed2 = Method.Get((IWampClientProxy proxy) => proxy.Subscribed(default(long), default(long)));
        private static readonly MethodInfo mUnsubscribed2 = Method.Get((IWampClientProxy proxy) => proxy.Unsubscribed(default(long)));
        private static readonly MethodInfo mEvent3 = Method.Get((IWampClientProxy proxy) => proxy.Event(default(long), default(long), default(EventDetails)));
        private static readonly MethodInfo mEvent4 = Method.Get((IWampClientProxy proxy) => proxy.Event(default(long), default(long), default(EventDetails), default(object[])));
        private static readonly MethodInfo mEvent5 = Method.Get((IWampClientProxy proxy) => proxy.Event(default(long), default(long), default(EventDetails), default(object[]), default(IDictionary<string, object>)));
        private static readonly MethodInfo mHello2 = Method.Get((IWampServerProxy proxy) => proxy.Hello(default(string), default(HelloDetails)));
        private static readonly MethodInfo mAuthenticate2 = Method.Get((IWampServerProxy proxy) => proxy.Authenticate(default(string), default(AuthenticateExtraData)));
        private static readonly MethodInfo mRegister3 = Method.Get((IWampServerProxy proxy) => proxy.Register(default(long), default(RegisterOptions), default(string)));
        private static readonly MethodInfo mUnregister2 = Method.Get((IWampServerProxy proxy) => proxy.Unregister(default(long), default(long)));
        private static readonly MethodInfo mCall3 = Method.Get((IWampServerProxy proxy) => proxy.Call(default(long), default(CallOptions), default(string)));
        private static readonly MethodInfo mCall4 = Method.Get((IWampServerProxy proxy) => proxy.Call(default(long), default(CallOptions), default(string), default(object[])));
        private static readonly MethodInfo mCall5 = Method.Get((IWampServerProxy proxy) => proxy.Call(default(long), default(CallOptions), default(string), default(object[]), default(IDictionary<string, object>)));
        private static readonly MethodInfo mCancel2 = Method.Get((IWampServerProxy proxy) => proxy.Cancel(default(long), default(CancelOptions)));
        private static readonly MethodInfo mYield2 = Method.Get((IWampServerProxy proxy) => proxy.Yield(default(long), default(YieldOptions)));
        private static readonly MethodInfo mYield3 = Method.Get((IWampServerProxy proxy) => proxy.Yield(default(long), default(YieldOptions), default(object[])));
        private static readonly MethodInfo mYield4 = Method.Get((IWampServerProxy proxy) => proxy.Yield(default(long), default(YieldOptions), default(object[]), default(IDictionary<string, object>)));
        private static readonly MethodInfo mPublish3 = Method.Get((IWampServerProxy proxy) => proxy.Publish(default(long), default(PublishOptions), default(string)));
        private static readonly MethodInfo mPublish4 = Method.Get((IWampServerProxy proxy) => proxy.Publish(default(long), default(PublishOptions), default(string), default(object[])));
        private static readonly MethodInfo mPublish5 = Method.Get((IWampServerProxy proxy) => proxy.Publish(default(long), default(PublishOptions), default(string), default(object[]), default(IDictionary<string, object>)));
        private static readonly MethodInfo mSubscribe3 = Method.Get((IWampServerProxy proxy) => proxy.Subscribe(default(long), default(SubscribeOptions), default(string)));
        private static readonly MethodInfo mUnsubscribe2 = Method.Get((IWampServerProxy proxy) => proxy.Unsubscribe(default(long), default(long)));
        private readonly IWampOutgoingRequestSerializer mSerializer;

        public WampProtocol(IWampOutgoingRequestSerializer serializer)
        {
            mSerializer = serializer;
        }

        public WampMessage<object> Challenge(string authMethod, ChallengeDetails extra)
        {
            return mSerializer.SerializeRequest(mChallenge2, new object[] { authMethod, extra });
        }

        public WampMessage<object> Welcome(long session, object details)
        {
            return mSerializer.SerializeRequest(mWelcome2, new object[] { session, details });
        }

        public WampMessage<object> Abort(object details, string reason)
        {
            return mSerializer.SerializeRequest(mAbort2, new object[] { details, reason });
        }

        public WampMessage<object> Goodbye(object details, string reason)
        {
            return mSerializer.SerializeRequest(mGoodbye2, new object[] { details, reason });
        }

        public WampMessage<object> Error(int requestType, long requestId, object details, string error)
        {
            return mSerializer.SerializeRequest(mError4, new object[] { requestType, requestId, details, error });
        }

        public WampMessage<object> Error(int requestType, long requestId, object details, string error, object[] arguments)
        {
            return mSerializer.SerializeRequest(mError5, new object[] { requestType, requestId, details, error, arguments });
        }

        public WampMessage<object> Error(int requestType, long requestId, object details, string error, object[] arguments, object argumentsKeywords)
        {
            return mSerializer.SerializeRequest(mError6, new object[] { requestType, requestId, details, error, arguments, argumentsKeywords });
        }

        public WampMessage<object> Registered(long requestId, long registrationId)
        {
            return mSerializer.SerializeRequest(mRegistered2, new object[] { requestId, registrationId });
        }

        public WampMessage<object> Unregistered(long requestId)
        {
            return mSerializer.SerializeRequest(mUnregistered1, new object[] { requestId });
        }

        public WampMessage<object> Invocation(long requestId, long registrationId, InvocationDetails details)
        {
            return mSerializer.SerializeRequest(mInvocation3, new object[] { requestId, registrationId, details });
        }

        public WampMessage<object> Invocation(long requestId, long registrationId, InvocationDetails details, object[] arguments)
        {
            return mSerializer.SerializeRequest(mInvocation4, new object[] { requestId, registrationId, details, arguments });
        }

        public WampMessage<object> Invocation(long requestId, long registrationId, InvocationDetails details, object[] arguments, IDictionary<string, object> argumentsKeywords)
        {
            return mSerializer.SerializeRequest(mInvocation5, new object[] { requestId, registrationId, details, arguments, argumentsKeywords });
        }

        public WampMessage<object> Interrupt(long requestId, object options)
        {
            return mSerializer.SerializeRequest(mInterrupt2, new object[] { requestId, options });
        }

        public WampMessage<object> Result(long requestId, ResultDetails details)
        {
            return mSerializer.SerializeRequest(mResult2, new object[] { requestId, details });
        }

        public WampMessage<object> Result(long requestId, ResultDetails details, object[] arguments)
        {
            return mSerializer.SerializeRequest(mResult3, new object[] { requestId, details, arguments });
        }

        public WampMessage<object> Result(long requestId, ResultDetails details, object[] arguments, IDictionary<string, object> argumentsKeywords)
        {
            return mSerializer.SerializeRequest(mResult4, new object[] { requestId, details, arguments, argumentsKeywords });
        }

        public WampMessage<object> Published(long requestId, long publicationId)
        {
            return mSerializer.SerializeRequest(mPublished2, new object[] { requestId, publicationId });
        }

        public WampMessage<object> Subscribed(long requestId, long subscriptionId)
        {
            return mSerializer.SerializeRequest(mSubscribed2, new object[] { requestId, subscriptionId });
        }

        public WampMessage<object> Unsubscribed(long requestId, long subscriptionId)
        {
            return mSerializer.SerializeRequest(mUnsubscribed2, new object[] { requestId, subscriptionId });
        }

        public WampMessage<object> Event(long subscriptionId, long publicationId, EventDetails details)
        {
            return mSerializer.SerializeRequest(mEvent3, new object[] { subscriptionId, publicationId, details });
        }

        public WampMessage<object> Event(long subscriptionId, long publicationId, EventDetails details, object[] arguments)
        {
            return mSerializer.SerializeRequest(mEvent4, new object[] { subscriptionId, publicationId, details, arguments });
        }

        public WampMessage<object> Event(long subscriptionId, long publicationId, EventDetails details, object[] arguments, IDictionary<string, object> argumentsKeywords)
        {
            return mSerializer.SerializeRequest(mEvent5, new object[] { subscriptionId, publicationId, details, arguments, argumentsKeywords });
        }

        public WampMessage<object> Hello(string realm, HelloDetails details)
        {
            return mSerializer.SerializeRequest(mHello2, new object[] { realm, details });
        }

        public WampMessage<object> Authenticate(string signature, object extra)
        {
            return mSerializer.SerializeRequest(mAuthenticate2, new object[] { signature, extra });
        }

        public WampMessage<object> Register(long requestId, RegisterOptions options, string procedure)
        {
            return mSerializer.SerializeRequest(mRegister3, new object[] { requestId, options, procedure });
        }

        public WampMessage<object> Unregister(long requestId, long registrationId)
        {
            return mSerializer.SerializeRequest(mUnregister2, new object[] { requestId, registrationId });
        }

        public WampMessage<object> Call(long requestId, CallOptions options, string procedure)
        {
            return mSerializer.SerializeRequest(mCall3, new object[] { requestId, options, procedure });
        }

        public WampMessage<object> Call(long requestId, CallOptions options, string procedure, object[] arguments)
        {
            return mSerializer.SerializeRequest(mCall4, new object[] { requestId, options, procedure, arguments });
        }

        public WampMessage<object> Call(long requestId, CallOptions options, string procedure, object[] arguments, IDictionary<string, object> argumentsKeywords)
        {
            return mSerializer.SerializeRequest(mCall5, new object[] { requestId, options, procedure, arguments, argumentsKeywords });
        }

        public WampMessage<object> Cancel(long requestId, CancelOptions options)
        {
            return mSerializer.SerializeRequest(mCancel2, new object[] { requestId, options });
        }

        public WampMessage<object> Yield(long requestId, YieldOptions options)
        {
            return mSerializer.SerializeRequest(mYield2, new object[] { requestId, options });
        }

        public WampMessage<object> Yield(long requestId, YieldOptions options, object[] arguments)
        {
            return mSerializer.SerializeRequest(mYield3, new object[] { requestId, options, arguments });
        }

        public WampMessage<object> Yield(long requestId, YieldOptions options, object[] arguments, IDictionary<string, object> argumentsKeywords)
        {
            return mSerializer.SerializeRequest(mYield4, new object[] { requestId, options, arguments, argumentsKeywords });
        }

        public WampMessage<object> Publish(long requestId, PublishOptions options, string topicUri)
        {
            return mSerializer.SerializeRequest(mPublish3, new object[] { requestId, options, topicUri });
        }

        public WampMessage<object> Publish(long requestId, PublishOptions options, string topicUri, object[] arguments)
        {
            return mSerializer.SerializeRequest(mPublish4, new object[] { requestId, options, topicUri, arguments });
        }

        public WampMessage<object> Publish(long requestId, PublishOptions options, string topicUri, object[] arguments, IDictionary<string, object> argumentKeywords)
        {
            return mSerializer.SerializeRequest(mPublish5, new object[] { requestId, options, topicUri, arguments, argumentKeywords });
        }

        public WampMessage<object> Subscribe(long requestId, SubscribeOptions options, string topicUri)
        {
            return mSerializer.SerializeRequest(mSubscribe3, new object[] { requestId, options, topicUri });
        }

        public WampMessage<object> Unsubscribe(long requestId, long subscriptionId)
        {
            return mSerializer.SerializeRequest(mUnsubscribe2, new object[] { requestId, subscriptionId });
        }
    }
}