using System.Collections.Generic;
using System.Reflection;
using WampSharp.Core.Proxy;
using WampSharp.Core.Utilities;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Core.Listener.ClientBuilder
{
    internal abstract class WampClientProxyBase : ProxyBase
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

        public WampClientProxyBase(IWampOutgoingMessageHandler messageHandler, IWampOutgoingRequestSerializer requestSerializer)
            : base(messageHandler, requestSerializer)
        {
        }

        public void Challenge(string authMethod, ChallengeDetails extra)
        {
            Send(mChallenge2, authMethod, extra);
        }

        public void Welcome(long session, WelcomeDetails details)
        {
            Send(mWelcome2, session, details);
        }

        public void Abort(AbortDetails details, string reason)
        {
            Send(mAbort2, details, reason);
        }

        public void Goodbye(GoodbyeDetails details, string reason)
        {
            Send(mGoodbye2, details, reason);
        }

        public void Error(int requestType, long requestId, object details, string error)
        {
            Send(mError4, requestType, requestId, details, error);
        }

        public void Error(int requestType, long requestId, object details, string error, object[] arguments)
        {
            Send(mError5, requestType, requestId, details, error, arguments);
        }

        public void Error(int requestType, long requestId, object details, string error, object[] arguments, object argumentsKeywords)
        {
            Send(mError6, requestType, requestId, details, error, arguments, argumentsKeywords);
        }

        public void Registered(long requestId, long registrationId)
        {
            Send(mRegistered2, requestId, registrationId);
        }

        public void Unregistered(long requestId)
        {
            Send(mUnregistered1, requestId);
        }

        public void Invocation(long requestId, long registrationId, InvocationDetails details)
        {
            Send(mInvocation3, requestId, registrationId, details);
        }

        public void Invocation(long requestId, long registrationId, InvocationDetails details, object[] arguments)
        {
            Send(mInvocation4, requestId, registrationId, details, arguments);
        }

        public void Invocation(long requestId, long registrationId, InvocationDetails details, object[] arguments, IDictionary<string, object> argumentsKeywords)
        {
            Send(mInvocation5, requestId, registrationId, details, arguments, argumentsKeywords);
        }

        public void Interrupt(long requestId, InterruptDetails details)
        {
            Send(mInterrupt2, requestId, details);
        }

        public void Result(long requestId, ResultDetails details)
        {
            Send(mResult2, requestId, details);
        }

        public void Result(long requestId, ResultDetails details, object[] arguments)
        {
            Send(mResult3, requestId, details, arguments);
        }

        public void Result(long requestId, ResultDetails details, object[] arguments, IDictionary<string, object> argumentsKeywords)
        {
            Send(mResult4, requestId, details, arguments, argumentsKeywords);
        }

        public void Published(long requestId, long publicationId)
        {
            Send(mPublished2, requestId, publicationId);
        }

        public void Subscribed(long requestId, long subscriptionId)
        {
            Send(mSubscribed2, requestId, subscriptionId);
        }

        public void Unsubscribed(long requestId)
        {
            Send(mUnsubscribed2, requestId);
        }

        public void Event(long subscriptionId, long publicationId, EventDetails details)
        {
            Send(mEvent3, subscriptionId, publicationId, details);
        }

        public void Event(long subscriptionId, long publicationId, EventDetails details, object[] arguments)
        {
            Send(mEvent4, subscriptionId, publicationId, details, arguments);
        }

        public void Event(long subscriptionId, long publicationId, EventDetails details, object[] arguments, IDictionary<string, object> argumentsKeywords)
        {
            Send(mEvent5, subscriptionId, publicationId, details, arguments, argumentsKeywords);
        }
    }
}