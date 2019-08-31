using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Client
{
    internal class WampClient<TMessage> : IWampClient<TMessage>,
                                          IWampSessionClientExtended,
                                          IWampCalleeError<TMessage>,
                                          IWampCallerError<TMessage>,
                                          IWampPublisherError<TMessage>,
                                          IWampSubscriberError<TMessage>
    {

        #region Fields

        #endregion

        #region Constructor

        public WampClient(IWampRealmProxyFactory<TMessage> realmFactory)
        {
            Realm = realmFactory.Build(this);
            ErrorHandler = new ErrorForwarder<TMessage>(this);
        }

        #endregion

        #region Properties

        public IWampRealmProxy Realm { get; }

        public IWampCallee<TMessage> Callee => this.Realm.RpcCatalog as IWampCallee<TMessage>;

        public IWampCaller<TMessage> Caller => this.Realm.RpcCatalog as IWampCaller<TMessage>;

        public IWampCalleeError<TMessage> CalleeError => this.Realm.RpcCatalog as IWampCalleeError<TMessage>;

        public IWampCallerError<TMessage> CallerError => this.Realm.RpcCatalog as IWampCallerError<TMessage>;

        public IWampPublisher<TMessage> Publisher => Realm.TopicContainer as IWampPublisher<TMessage>;

        public IWampSubscriber<TMessage> Subscriber => Realm.TopicContainer as IWampSubscriber<TMessage>;

        public IWampPublisherError<TMessage> PublisherError => Realm.TopicContainer as IWampPublisherError<TMessage>;

        public IWampSubscriberError<TMessage> SubscriberError => Realm.TopicContainer as IWampSubscriberError<TMessage>;

        public IWampError<TMessage> ErrorHandler { get; }

        public IWampSessionClientExtended SessionClient => Realm.Monitor as IWampSessionClientExtended;

        #endregion

        #region Delegating Members

        public Task OpenTask => SessionClient.OpenTask;

        public void Challenge(string authMethod, ChallengeDetails extra)
        {
            SessionClient.Challenge(authMethod, extra);
        }

        public void Welcome(long session, WelcomeDetails details)
        {
            SessionClient.Welcome(session, details);
        }

        public void Abort(AbortDetails details, string reason)
        {
            SessionClient.Abort(details, reason);
        }

        public void Goodbye(GoodbyeDetails details, string reason)
        {
            SessionClient.Goodbye(details, reason);
        }

        public long Session => SessionClient.Session;

        public Task<GoodbyeMessage> Close(string reason, GoodbyeDetails details)
        {
            return SessionClient.Close(reason, details);
        }

        public void OnConnectionOpen()
        {
            SessionClient.OnConnectionOpen();
        }

        public void OnConnectionClosed()
        {
            SessionClient.OnConnectionClosed();
        }

        public void OnConnectionError(Exception exception)
        {
            SessionClient.OnConnectionError(exception);
        }

        public void Registered(long requestId, long registrationId)
        {
            Callee.Registered(requestId, registrationId);
        }

        public void Unregistered(long requestId)
        {
            Callee.Unregistered(requestId);
        }

        public void Invocation(long requestId, long registrationId, InvocationDetails details)
        {
            Callee.Invocation(requestId, registrationId, details);
        }

        public void Invocation(long requestId, long registrationId, InvocationDetails details, TMessage[] arguments)
        {
            Callee.Invocation(requestId, registrationId, details, arguments);
        }

        public void Invocation(long requestId, long registrationId, InvocationDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            Callee.Invocation(requestId, registrationId, details, arguments, argumentsKeywords);
        }

        public void Interrupt(long requestId, InterruptDetails details)
        {
            Callee.Interrupt(requestId, details);
        }

        public void Subscribed(long requestId, long subscriptionId)
        {
            Subscriber.Subscribed(requestId, subscriptionId);
        }

        public void Unsubscribed(long requestId)
        {
            Subscriber.Unsubscribed(requestId);
        }

        public void Event(long subscriptionId, long publicationId, EventDetails details)
        {
            Subscriber.Event(subscriptionId, publicationId, details);
        }

        public void Event(long subscriptionId, long publicationId, EventDetails details, TMessage[] arguments)
        {
            Subscriber.Event(subscriptionId, publicationId, details, arguments);
        }

        public void Event(long subscriptionId, long publicationId, EventDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            Subscriber.Event(subscriptionId, publicationId, details, arguments, argumentsKeywords);
        }

        public void Result(long requestId, ResultDetails details)
        {
            Caller.Result(requestId, details);
        }

        public void Result(long requestId, ResultDetails details, TMessage[] arguments)
        {
            Caller.Result(requestId, details, arguments);
        }

        public void Result(long requestId, ResultDetails details, TMessage[] arguments, IDictionary<string, TMessage> argumentsKeywords)
        {
            Caller.Result(requestId, details, arguments, argumentsKeywords);
        }

        public void Published(long requestId, long publicationId)
        {
            Publisher.Published(requestId, publicationId);
        }

        public void Error(int requestType, long requestId, TMessage details, string error)
        {
            ErrorHandler.Error(requestType, requestId, details, error);
        }

        public void Error(int requestType, long requestId, TMessage details, string error, TMessage[] arguments)
        {
            ErrorHandler.Error(requestType, requestId, details, error, arguments);
        }

        public void Error(int requestType, long requestId, TMessage details, string error, TMessage[] arguments,
                          TMessage argumentsKeywords)
        {
            ErrorHandler.Error(requestType, requestId, details, error, arguments, argumentsKeywords);
        }

        public void SubscribeError(long requestId, TMessage details, string error)
        {
            SubscriberError.SubscribeError(requestId, details, error);
        }

        public void SubscribeError(long requestId, TMessage details, string error, TMessage[] arguments)
        {
            SubscriberError.SubscribeError(requestId, details, error, arguments);
        }

        public void SubscribeError(long requestId, TMessage details, string error, TMessage[] arguments,
                                   TMessage argumentsKeywords)
        {
            SubscriberError.SubscribeError(requestId, details, error, arguments, argumentsKeywords);
        }

        public void UnsubscribeError(long requestId, TMessage details, string error)
        {
            SubscriberError.UnsubscribeError(requestId, details, error);
        }

        public void UnsubscribeError(long requestId, TMessage details, string error, TMessage[] arguments)
        {
            SubscriberError.UnsubscribeError(requestId, details, error, arguments);
        }

        public void UnsubscribeError(long requestId, TMessage details, string error, TMessage[] arguments,
                                     TMessage argumentsKeywords)
        {
            SubscriberError.UnsubscribeError(requestId, details, error, arguments, argumentsKeywords);
        }

        public void RegisterError(long requestId, TMessage details, string error)
        {
            CalleeError.RegisterError(requestId, details, error);
        }

        public void RegisterError(long requestId, TMessage details, string error, TMessage[] arguments)
        {
            CalleeError.RegisterError(requestId, details, error, arguments);
        }

        public void RegisterError(long requestId, TMessage details, string error, TMessage[] arguments,
                                  TMessage argumentsKeywords)
        {
            CalleeError.RegisterError(requestId, details, error, arguments, argumentsKeywords);
        }

        public void UnregisterError(long requestId, TMessage details, string error)
        {
            CalleeError.UnregisterError(requestId, details, error);
        }

        public void UnregisterError(long requestId, TMessage details, string error, TMessage[] arguments)
        {
            CalleeError.UnregisterError(requestId, details, error, arguments);
        }

        public void UnregisterError(long requestId, TMessage details, string error, TMessage[] arguments,
                                    TMessage argumentsKeywords)
        {
            CalleeError.UnregisterError(requestId, details, error, arguments, argumentsKeywords);
        }

        public void PublishError(long requestId, TMessage details, string error)
        {
            PublisherError.PublishError(requestId, details, error);
        }

        public void PublishError(long requestId, TMessage details, string error, TMessage[] arguments)
        {
            PublisherError.PublishError(requestId, details, error, arguments);
        }

        public void PublishError(long requestId, TMessage details, string error, TMessage[] arguments,
                                 TMessage argumentsKeywords)
        {
            PublisherError.PublishError(requestId, details, error, arguments, argumentsKeywords);
        }

        public void CallError(long requestId, TMessage details, string error)
        {
            CallerError.CallError(requestId, details, error);
        }

        public void CallError(long requestId, TMessage details, string error, TMessage[] arguments)
        {
            CallerError.CallError(requestId, details, error, arguments);
        }

        public void CallError(long requestId, TMessage details, string error, TMessage[] arguments,
                              TMessage argumentsKeywords)
        {
            CallerError.CallError(requestId, details, error, arguments, argumentsKeywords);
        }

        #endregion
    }
}