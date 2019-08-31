using System.Collections.Generic;
using WampSharp.Core.Contracts;
using WampSharp.Core.Message;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2
{
    public class WampServer<TMessage> : IWampServer<TMessage>, IWampMissingMethodContract<TMessage, IWampClientProxy>
    {
        private readonly IWampSessionServer<TMessage> mSession;
        private readonly IWampDealer<TMessage> mDealer;
        private readonly IWampBroker<TMessage> mBroker;

        public WampServer(IWampSessionServer<TMessage> session,
                          IWampDealer<TMessage> dealer,
                          IWampBroker<TMessage> broker)
        {
            mSession = session;
            mDealer = dealer;
            mBroker = broker;
        }

        public virtual void Publish(IWampPublisher publisher, long requestId, PublishOptions options, string topicUri)
        {
            mBroker.Publish(publisher, requestId, options, topicUri);
        }

        public virtual void Publish(IWampPublisher publisher, long requestId, PublishOptions options, string topicUri, TMessage[] arguments)
        {
            mBroker.Publish(publisher, requestId, options, topicUri, arguments);
        }

        public virtual void Publish(IWampPublisher publisher, long requestId, PublishOptions options, string topicUri, TMessage[] arguments, IDictionary<string, TMessage> argumentKeywords)
        {
            mBroker.Publish(publisher, requestId, options, topicUri, arguments, argumentKeywords);
        }

        public virtual void Subscribe(IWampSubscriber subscriber, long requestId, SubscribeOptions options, string topicUri)
        {
            mBroker.Subscribe(subscriber, requestId, options, topicUri);
        }

        public virtual void Unsubscribe(IWampSubscriber subscriber, long requestId, long subscriptionId)
        {
            mBroker.Unsubscribe(subscriber, requestId, subscriptionId);
        }

        public virtual void Register(IWampCallee callee, long requestId, RegisterOptions options, string procedure)
        {
            mDealer.Register(callee, requestId, options, procedure);
        }

        public virtual void Unregister(IWampCallee callee, long requestId, long registrationId)
        {
            mDealer.Unregister(callee, requestId, registrationId);
        }

        public virtual void Call(IWampCaller caller, long requestId, CallOptions options, string procedure)
        {
            mDealer.Call(caller, requestId, options, procedure);
        }

        public virtual void Call(IWampCaller caller, long requestId, CallOptions options, string procedure, TMessage[] arguments)
        {
            mDealer.Call(caller, requestId, options, procedure, arguments);
        }

        public virtual void Call(IWampCaller caller, long requestId, CallOptions options, string procedure, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            mDealer.Call(caller, requestId, options, procedure, arguments, argumentsKeywords);
        }

        public virtual void Cancel(IWampCaller caller, long requestId, CancelOptions options)
        {
            mDealer.Cancel(caller, requestId, options);
        }

        public virtual void Yield(IWampCallee callee, long requestId, YieldOptions options)
        {
            mDealer.Yield(callee, requestId, options);
        }

        public virtual void Yield(IWampCallee callee, long requestId, YieldOptions options, TMessage[] arguments)
        {
            mDealer.Yield(callee, requestId, options, arguments);
        }

        public virtual void Yield(IWampCallee callee, long requestId, YieldOptions options, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            mDealer.Yield(callee, requestId, options, arguments, argumentsKeywords);
        }

        public virtual void Error(IWampClientProxy client, int requestType, long requestId, TMessage details, string error)
        {
            mDealer.Error(client, requestType, requestId, details, error);
        }

        public virtual void Error(IWampClientProxy client, int requestType, long requestId, TMessage details, string error, TMessage[] arguments)
        {
            mDealer.Error(client, requestType, requestId, details, error, arguments);
        }

        public virtual void Error(IWampClientProxy client, int requestType, long requestId, TMessage details, string error, TMessage[] arguments,
                          TMessage argumentsKeywords)
        {
            mDealer.Error(client, requestType, requestId, details, error, arguments, argumentsKeywords);
        }

        public virtual void Hello(IWampSessionClient client, string realm, HelloDetails details)
        {
            mSession.Hello(client, realm, details);
        }

        public virtual void Abort(IWampSessionClient client, AbortDetails details, string reason)
        {
            mSession.Abort(client, details, reason);
        }

        public virtual void Authenticate(IWampSessionClient client, string signature, AuthenticateExtraData extra)
        {
            mSession.Authenticate(client, signature, extra);
        }

        public virtual void Goodbye(IWampSessionClient client, GoodbyeDetails details, string reason)
        {
            mSession.Goodbye(client, details, reason);
        }

        public void OnNewClient(IWampClientProxy<TMessage> client)
        {
            mSession.OnNewClient(client);
        }

        public void OnClientDisconnect(IWampClientProxy<TMessage> client)
        {
            mSession.OnClientDisconnect(client);
        }

        public void Missing(IWampClientProxy client, WampMessage<TMessage> rawMessage)
        {
        }
    }
}