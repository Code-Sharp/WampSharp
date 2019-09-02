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

            IObservable<IWampSerializedEvent> connectionComplete =
                Observable.FromEventPattern<WampSessionCloseEventArgs>
                    (x => monitor.ConnectionBroken += x,
                     x => monitor.ConnectionBroken -= x)
                          .SelectMany(x => Observable.Throw<IWampSerializedEvent>(new WampConnectionBrokenException(x.EventArgs)));

            ClientObservable messages = new ClientObservable(topic);

            IObservable<IWampSerializedEvent> result =
                Observable.Merge(messages,connectionError, connectionComplete);

            return result;
        }

        protected override void Publish(PublishOptions options)
        {
            mTopic.Publish(options);
        }

        protected override void Publish(PublishOptions options, object[] arguments)
        {
            mTopic.Publish(options, arguments);
        }

        protected override void Publish(PublishOptions options, object[] arguments, IDictionary<string, object> argumentsKeywords)
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
                Task<IAsyncDisposable>
                    task = mTopic.Subscribe(new RawTopicClientSubscriber(observer), new SubscribeOptions());

                FutureDisposable result = new FutureDisposable(task);

                return result;
            }

            private class FutureDisposable : IDisposable
            {
                private readonly Task<IAsyncDisposable> mDisposableTask;

                public FutureDisposable(Task<IAsyncDisposable> disposableTask)
                {
                    mDisposableTask = disposableTask;
                }

                public void Dispose()
                {
                    if (mDisposableTask.IsCompleted && 
                        (mDisposableTask.Exception == null))
                    {
                        IAsyncDisposable result = mDisposableTask.Result;
                        result.DisposeAsync().AsTask().ContinueWith(x =>
                        {
                            if (x.Exception != null)
                            {
                                // Done in order to avoid .NET 4.0 UnhandledException
                                // Nobody sees this exception anyway. I hope that we
                                // soon get a version of Reactive Extensions which is more
                                // suited for remote pub/sub, and allows to return AsyncDispoables.
                            }
                        });
                    }
                }
            }
        }
    }
}