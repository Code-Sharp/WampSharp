using System;
using System.Collections.Generic;
using System.Reflection;
using WampSharp.Core.Client;
using WampSharp.Core.Listener;
using WampSharp.Core.Proxy;
using WampSharp.Core.Utilities;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Client
{
    public class ManualWampServerProxyBuilder<TMessage> :
        IWampServerProxyBuilder<TMessage, IWampClient<TMessage>, IWampServerProxy>
    {
        private readonly IWampOutgoingRequestSerializer mSerializer;
        private readonly IWampServerProxyOutgoingMessageHandlerBuilder<TMessage, IWampClient<TMessage>> mOutgoingHandlerBuilder;

        public ManualWampServerProxyBuilder(IWampOutgoingRequestSerializer serializer,
                                            IWampServerProxyOutgoingMessageHandlerBuilder<TMessage, IWampClient<TMessage>> outgoingHandlerBuilder)
        {
            mSerializer = serializer;
            mOutgoingHandlerBuilder = outgoingHandlerBuilder;
        }

        public IWampServerProxy Create(IWampClient<TMessage> client, IWampConnection<TMessage> connection)
        {
            IWampOutgoingMessageHandler outgoingMessageHandler = mOutgoingHandlerBuilder.Build(client, connection);

            WampServerProxy result = 
                new WampServerProxy(outgoingMessageHandler, mSerializer, connection);

            return result;
        }

        private class WampServerProxy : ProxyBase, IWampServerProxy, IDisposable
        {
            private static readonly MethodInfo mPublish3 = Method.Get((IWampServerProxy proxy) => proxy.Publish(default(long), default(PublishOptions), default(string)));
            private static readonly MethodInfo mPublish4 = Method.Get((IWampServerProxy proxy) => proxy.Publish(default(long), default(PublishOptions), default(string), default(object[])));
            private static readonly MethodInfo mPublish5 = Method.Get((IWampServerProxy proxy) => proxy.Publish(default(long), default(PublishOptions), default(string), default(object[]), default(IDictionary<string, object>)));
            private static readonly MethodInfo mSubscribe3 = Method.Get((IWampServerProxy proxy) => proxy.Subscribe(default(long), default(SubscribeOptions), default(string)));
            private static readonly MethodInfo mUnsubscribe2 = Method.Get((IWampServerProxy proxy) => proxy.Unsubscribe(default(long), default(long)));
            private static readonly MethodInfo mRegister3 = Method.Get((IWampServerProxy proxy) => proxy.Register(default(long), default(RegisterOptions), default(string)));
            private static readonly MethodInfo mUnregister2 = Method.Get((IWampServerProxy proxy) => proxy.Unregister(default(long), default(long)));
            private static readonly MethodInfo mCall3 = Method.Get((IWampServerProxy proxy) => proxy.Call(default(long), default(CallOptions), default(string)));
            private static readonly MethodInfo mCall4 = Method.Get((IWampServerProxy proxy) => proxy.Call(default(long), default(CallOptions), default(string), default(object[])));
            private static readonly MethodInfo mCall5 = Method.Get((IWampServerProxy proxy) => proxy.Call(default(long), default(CallOptions), default(string), default(object[]), default(IDictionary<string, object>)));
            private static readonly MethodInfo mCancel2 = Method.Get((IWampServerProxy proxy) => proxy.Cancel(default(long), default(CancelOptions)));
            private static readonly MethodInfo mYield2 = Method.Get((IWampServerProxy proxy) => proxy.Yield(default(long), default(YieldOptions)));
            private static readonly MethodInfo mYield3 = Method.Get((IWampServerProxy proxy) => proxy.Yield(default(long), default(YieldOptions), default(object[])));
            private static readonly MethodInfo mYield4 = Method.Get((IWampServerProxy proxy) => proxy.Yield(default(long), default(YieldOptions), default(object[]), default(IDictionary<string, object>)));
            private static readonly MethodInfo mHello2 = Method.Get((IWampServerProxy proxy) => proxy.Hello(default(string), default(HelloDetails)));
            private static readonly MethodInfo mAbort2 = Method.Get((IWampServerProxy proxy) => proxy.Abort(default(AbortDetails), default(string)));
            private static readonly MethodInfo mAuthenticate2 = Method.Get((IWampServerProxy proxy) => proxy.Authenticate(default(string), default(AuthenticateExtraData)));
            private static readonly MethodInfo mGoodbye2 = Method.Get((IWampServerProxy proxy) => proxy.Goodbye(default(GoodbyeDetails), default(string)));
            private static readonly MethodInfo mError4 = Method.Get((IWampServerProxy proxy) => proxy.Error(default(int), default(long), default(object), default(string)));
            private static readonly MethodInfo mError5 = Method.Get((IWampServerProxy proxy) => proxy.Error(default(int), default(long), default(object), default(string), default(object[])));
            private static readonly MethodInfo mError6 = Method.Get((IWampServerProxy proxy) => proxy.Error(default(int), default(long), default(object), default(string), default(object[]), default(object)));
            private readonly IDisposable mDisposable;

            public WampServerProxy(IWampOutgoingMessageHandler messageHandler, IWampOutgoingRequestSerializer requestSerializer, IDisposable disposable)
                : base(messageHandler, requestSerializer)
            {
                mDisposable = disposable;
            }

            public void Publish(long requestId, PublishOptions options, string topicUri)
            {
                Send(mPublish3, requestId, options, topicUri);
            }

            public void Publish(long requestId, PublishOptions options, string topicUri, object[] arguments)
            {
                Send(mPublish4, requestId, options, topicUri, arguments);
            }

            public void Publish(long requestId, PublishOptions options, string topicUri, object[] arguments, IDictionary<string, object> argumentKeywords)
            {
                Send(mPublish5, requestId, options, topicUri, arguments, argumentKeywords);
            }

            public void Subscribe(long requestId, SubscribeOptions options, string topicUri)
            {
                Send(mSubscribe3, requestId, options, topicUri);
            }

            public void Unsubscribe(long requestId, long subscriptionId)
            {
                Send(mUnsubscribe2, requestId, subscriptionId);
            }

            public void Register(long requestId, RegisterOptions options, string procedure)
            {
                Send(mRegister3, requestId, options, procedure);
            }

            public void Unregister(long requestId, long registrationId)
            {
                Send(mUnregister2, requestId, registrationId);
            }

            public void Call(long requestId, CallOptions options, string procedure)
            {
                Send(mCall3, requestId, options, procedure);
            }

            public void Call(long requestId, CallOptions options, string procedure, object[] arguments)
            {
                Send(mCall4, requestId, options, procedure, arguments);
            }

            public void Call(long requestId, CallOptions options, string procedure, object[] arguments, IDictionary<string, object> argumentsKeywords)
            {
                Send(mCall5, requestId, options, procedure, arguments, argumentsKeywords);
            }

            public void Cancel(long requestId, CancelOptions options)
            {
                Send(mCancel2, requestId, options);
            }

            public void Yield(long requestId, YieldOptions options)
            {
                Send(mYield2, requestId, options);
            }

            public void Yield(long requestId, YieldOptions options, object[] arguments)
            {
                Send(mYield3, requestId, options, arguments);
            }

            public void Yield(long requestId, YieldOptions options, object[] arguments, IDictionary<string, object> argumentsKeywords)
            {
                Send(mYield4, requestId, options, arguments, argumentsKeywords);
            }

            public void Hello(string realm, HelloDetails details)
            {
                Send(mHello2, realm, details);
            }

            public void Abort(AbortDetails details, string reason)
            {
                Send(mAbort2, details, reason);
            }

            public void Authenticate(string signature, AuthenticateExtraData extra)
            {
                Send(mAuthenticate2, signature, extra);
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

            public void Dispose()
            {
                mDisposable.Dispose();
            }
        }
    }
}