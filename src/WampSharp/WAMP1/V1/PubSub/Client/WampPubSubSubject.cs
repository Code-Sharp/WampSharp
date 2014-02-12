using System;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using WampSharp.Core.Listener;
using WampSharp.Core.Serialization;
using WampSharp.V1.Core.Contracts;

namespace WampSharp.V1.PubSub.Client
{
    /// <summary>
    /// Represents a WAMP topic of a given topic uri.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <typeparam name="TEvent"></typeparam>
    public class WampPubSubSubject<TMessage, TEvent> : ISubject<TEvent>
    {
        private readonly Subject<TEvent> mSubject = new Subject<TEvent>(); 
        private readonly string mTopicUri;
        private readonly IWampFormatter<TMessage> mFormatter;
        private readonly object mLock = new object();
        private readonly IWampServer mServerProxy;
        private bool mSubscribed;

        /// <summary>
        /// Initializes a new instance of <see cref="WampPubSubSubject{TMessage,TEvent}"/>.
        /// </summary>
        /// <param name="topicUri">The topic uri of the current topic.</param>
        /// <param name="serverProxyFactory">The server proxy factory used to get
        /// callbacks from the server.</param>
        /// <param name="connection">The underlying connection this subject uses
        /// in order to send/receive messages.</param>
        /// <param name="formatter">The formatter used in order to serialize/deserialize
        /// the messages sent.</param>
        public WampPubSubSubject(string topicUri, IWampServerProxyFactory<TMessage> serverProxyFactory, IWampConnection<TMessage> connection, IWampFormatter<TMessage> formatter)
        {
            mTopicUri = topicUri;
            mFormatter = formatter;
            mServerProxy = serverProxyFactory.Create(new WampPubSubClient(this), connection);
        }

        public void OnNext(TEvent value)
        {
            mServerProxy.Publish(null, mTopicUri, value, false);
        }

        public void OnError(Exception error)
        {
            // Won't happen :)
        }

        public void OnCompleted()
        {
            mSubject.OnCompleted();
            OnUnsubscribe();
        }

        public IDisposable Subscribe(IObserver<TEvent> observer)
        {
            lock (mLock)
            {
                bool hasObservers = mSubject.HasObservers;
                
                IDisposable disposable = mSubject.Subscribe(observer);

                if (!hasObservers && mSubject.HasObservers)
                {
                    mServerProxy.Subscribe(null, mTopicUri);
                    mSubscribed = true;
                }

                return new CompositeDisposable(disposable,
                                               Disposable.Create(() => OnUnsubscribe()));
            }
        }

        private void OnUnsubscribe()
        {
            lock (mLock)
            {
                if (!mSubject.HasObservers)
                {
                    if (mSubscribed)
                    {
                        mServerProxy.Unsubscribe(null, mTopicUri);
                        mSubscribed = false;
                    }
                }                
            }
        }

        private void EventArrived(TMessage @event)
        {
            TEvent deserialized = mFormatter.Deserialize<TEvent>(@event);
            mSubject.OnNext(deserialized);
        }

        private class WampPubSubClient : IWampPubSubClient<TMessage>
        {
            private readonly WampPubSubSubject<TMessage, TEvent> mParent;

            public WampPubSubClient(WampPubSubSubject<TMessage, TEvent> parent)
            {
                mParent = parent;
            }

            public void Event(string topicUri, TMessage @event)
            {
                if (topicUri == mParent.mTopicUri)
                {
                    mParent.EventArrived(@event);
                }
            }
        }
    }
}