using System;
using System.Collections.Generic;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Authentication
{
    internal class WampAuthenticationServer<TMessage> : WampServer<TMessage>
    {
        public WampAuthenticationServer(IWampSessionServer<TMessage> session, IWampDealer<TMessage> dealer, IWampBroker<TMessage> broker) : 
            base(session, dealer, broker)
        {
        }

        public override void Subscribe(IWampSubscriber subscriber, long requestId, SubscribeOptions options, string topicUri)
        {
            InnerAction(subscriber,
                        authorizer => authorizer.CanSubscribe(options, topicUri),
                        () => base.Subscribe(subscriber, requestId, options, topicUri),
                        exception => subscriber.SubscribeError(requestId, exception));
        }

        public override void Register(IWampCallee callee, long requestId, RegisterOptions options, string procedure)
        {
            InnerAction(callee,
                        authorizer => authorizer.CanRegister(options, procedure),
                        () => base.Register(callee, requestId, options, procedure),
                        exception => callee.RegisterError(requestId, exception));
        }

        private void InnerPublish(IWampPublisher publisher, Action publishAction, long requestId, PublishOptions options, string topicUri)
        {
            InnerAction(publisher,
                        authorizer => authorizer.CanPublish(options, topicUri),
                        publishAction,
                        exception =>
                        {
                            if (options.Acknowledge == true)
                            {
                                publisher.PublishError(requestId, exception);
                            }
                        });
        }

        public override void Publish(IWampPublisher publisher, long requestId, PublishOptions options, string topicUri)
        {
            Action publishAction = () => base.Publish(publisher, requestId, options, topicUri);

            InnerPublish(publisher,
                         publishAction,
                         requestId,
                         options,
                         topicUri);
        }

        public override void Publish(IWampPublisher publisher, long requestId, PublishOptions options, string topicUri, TMessage[] arguments)
        {
            Action publishAction =
                () => base.Publish(publisher, requestId, options, topicUri, arguments);

            InnerPublish(publisher,
                         publishAction,
                         requestId,
                         options,
                         topicUri);
        }

        public override void Publish(IWampPublisher publisher, long requestId, PublishOptions options, string topicUri, TMessage[] arguments,
                                     IDictionary<string, TMessage> argumentKeywords)
        {
            Action publishAction =
                () => base.Publish(publisher, requestId, options, topicUri, arguments, argumentKeywords);

            InnerPublish(publisher,
                         publishAction,
                         requestId,
                         options,
                         topicUri);
        }


        private void InnerCall(IWampCaller caller, Action callAction, long requestId, CallOptions options, string procedure)
        {
            InnerAction(caller,
                        authorizer => authorizer.CanCall(options, procedure),
                        callAction,
                        exception => caller.CallError(requestId, exception));
        }

        public override void Call(IWampCaller caller, long requestId, CallOptions options, string procedure)
        {
            Action callAction = () => base.Call(caller, requestId, options, procedure);

            InnerCall(caller,
                      callAction,
                      requestId,
                      options,
                      procedure);
        }

        public override void Call(IWampCaller caller, long requestId, CallOptions options, string procedure, TMessage[] arguments)
        {
            Action callAction = () => base.Call(caller, requestId, options, procedure, arguments);

            InnerCall(caller,
                      callAction,
                      requestId,
                      options,
                      procedure);
        }

        public override void Call(IWampCaller caller, long requestId, CallOptions options, string procedure, TMessage[] arguments,
                                  IDictionary<string, TMessage> argumentsKeywords)
        {
            Action callAction = () => base.Call(caller, requestId, options, procedure, arguments, argumentsKeywords);

            InnerCall(caller,
                      callAction,
                      requestId,
                      options,
                      procedure);
        }

        private static void InnerAction(object clientProxy,
                                        Func<IWampAuthorizer, bool> authorizationCheck,
                                        Action action,
                                        Action<WampException> reportError)
        {
            IWampClientProperties wampClientProxy = clientProxy as IWampClientProxy;
            IWampAuthorizer authorizer = wampClientProxy.Authorizer;

            try
            {
                bool isAuthorized = authorizationCheck(authorizer);

                if (isAuthorized)
                {
                    action();
                }
                else
                {
                    reportError(new WampException(WampErrors.NotAuthorized));
                }
            }
            catch (WampException ex)
            {
                reportError(ex);
            }
        }
    }
}