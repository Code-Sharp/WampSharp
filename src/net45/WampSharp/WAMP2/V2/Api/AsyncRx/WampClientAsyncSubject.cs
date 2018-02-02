using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using WampSharp.Core.Listener;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm;

namespace WampSharp.V2
{
    internal class WampClientAsyncSubject : WampAsyncSubject
    {
        private readonly IWampTopicProxy mTopic;
        private readonly IAsyncObservable<IWampSerializedEvent> mObservable;

        public WampClientAsyncSubject(IWampTopicProxy topic, IWampClientConnectionMonitor monitor)
        {
            mTopic = topic;

            mObservable = CreateObservable(topic, monitor);
        }

        private static IAsyncObservable<IWampSerializedEvent> CreateObservable(IWampTopicProxy topic, IWampClientConnectionMonitor monitor)
        {
            IObservable<IWampSerializedEvent> connectionError =
                Observable.FromEventPattern<WampConnectionErrorEventArgs>
                    (x => monitor.ConnectionError += x,
                     x => monitor.ConnectionError -= x)
                          .SelectMany(x => Observable.Throw<IWampSerializedEvent>(x.EventArgs.Exception));

            IObservable<IWampSerializedEvent> connectionComplete =
                Observable.FromEventPattern<WampSessionCloseEventArgs>
                    (x => monitor.ConnectionBroken += x,
                     x => monitor.ConnectionBroken -= x)
                          .SelectMany(x => Observable.Throw<IWampSerializedEvent>(new WampConnectionBrokenException(x.EventArgs)));

            ClientAsyncObservable messages = new ClientAsyncObservable(topic);

            IObservable<IWampSerializedEvent> connectionNotifications =
                Observable.Merge(connectionError, connectionComplete);

            IAsyncObservable<IWampSerializedEvent> asyncNotifications =
                AsyncObservable.ToAsyncObservable(connectionNotifications);

            //IAsyncObservable<IWampSerializedEvent> result = 
            //    AsyncObservable.Merge(asyncNotifications, messages);

            IAsyncObservable<IWampSerializedEvent> result =
                new[] {asyncNotifications, messages}.ToObservable()
                    .ToAsyncObservable()
                    .Merge();

            return result;
        }

        protected override Task Publish(PublishOptions options)
        {
            return mTopic.Publish(options);
        }

        protected override Task Publish(PublishOptions options, object[] arguments)
        {
            return mTopic.Publish(options, arguments);
        }

        protected override Task Publish(PublishOptions options, object[] arguments,
            IDictionary<string, object> argumentsKeywords)
        {
            return mTopic.Publish(options, arguments, argumentsKeywords);
        }

        public override Task<IAsyncDisposable> SubscribeAsync(IAsyncObserver<IWampSerializedEvent> observer)
        {
            return mObservable.SubscribeAsync(observer);
        }

        private class ClientAsyncObservable : IAsyncObservable<IWampSerializedEvent>
        {
            private readonly IWampTopicProxy mTopic;

            public ClientAsyncObservable(IWampTopicProxy topic)
            {
                mTopic = topic;
            }

            public Task<IAsyncDisposable> SubscribeAsync(IAsyncObserver<IWampSerializedEvent> observer)
            {
                Task<IAsyncDisposable>
                    task = mTopic.Subscribe(new RawTopicClientAsyncSubscriber(observer), new SubscribeOptions());

                return task;
            }
        }
    }
}