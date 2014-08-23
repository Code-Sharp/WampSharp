using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using WampSharp.Core.Listener;
using WampSharp.V2.Client;
using WampSharp.V2.Realm;

namespace WampSharp.V2
{
    internal class WampClientSubject : WampSubject
    {
        private readonly IWampTopicProxy mTopic;
        private readonly IObservable<IWampSerializedEvent> mObservable;

        public WampClientSubject(IWampTopicProxy topic, IWampClientConnectionMonitor monitor)
        {
            mTopic = topic;

            mObservable = CreateObservable(topic, monitor);
        }

        private static IObservable<IWampSerializedEvent> CreateObservable(IWampTopicProxy topic, IWampClientConnectionMonitor monitor)
        {
            IObservable<IWampSerializedEvent> connectionError =
                Observable.FromEventPattern<WampConnectionErrorEventArgs>
                    (x => monitor.ConnectionError += x,
                     x => monitor.ConnectionError -= x)
                          .SelectMany(x => Observable.Throw<IWampSerializedEvent>(x.EventArgs.Exception));

            IObservable<Unit> connectionComplete =
                Observable.FromEventPattern<WampSessionCloseEventArgs>
                    (x => monitor.ConnectionBroken += x,
                     x => monitor.ConnectionBroken -= x)
                          .Select(x => Unit.Default);

            ClientObservable messages = new ClientObservable(topic);

            IObservable<IWampSerializedEvent> result =
                messages.Merge(connectionError)
                        .TakeUntil(connectionComplete);

            return result;
        }

        protected override void Publish(IDictionary<string, object> options)
        {
            mTopic.Publish(options);
        }

        protected override void Publish(IDictionary<string, object> options, object[] arguments)
        {
            mTopic.Publish(options, arguments);
        }

        protected override void Publish(IDictionary<string, object> options, object[] arguments, IDictionary<string, object> argumentsKeywords)
        {
            mTopic.Publish(options, arguments, argumentsKeywords);
        }

        public override IDisposable Subscribe(IObserver<IWampSerializedEvent> observer)
        {
            return mObservable.Subscribe(observer);
        }

        private class ClientObservable : IObservable<IWampSerializedEvent>
        {
            private readonly IWampTopicProxy mTopic;

            public ClientObservable(IWampTopicProxy topic)
            {
                mTopic = topic;
            }

            public IDisposable Subscribe(IObserver<IWampSerializedEvent> observer)
            {
                Task<IDisposable> task = mTopic.Subscribe(new RawSubscriber(observer), new { });

                // TODO: think of a better solution
                task.Wait();

                return task.Result;
            }
        }
    }
}