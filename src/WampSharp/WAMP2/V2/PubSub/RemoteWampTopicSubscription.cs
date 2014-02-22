using System.Reactive.Subjects;
using WampSharp.Core.Message;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Core.Listener;

namespace WampSharp.V2.PubSub
{
    public class RemoteWampTopicSubscription<TMessage> : IWampTopicSubscriber
    {
        private readonly IWampBinding<TMessage> mBinding; 
        private readonly IWampEventSerializer<TMessage> mSerializer;
        private readonly ISubject<WampMessage<TMessage>> mSubject = new Subject<WampMessage<TMessage>>();
        private readonly long mSubscriptionId;

        public RemoteWampTopicSubscription(long subscriptionId, IWampEventSerializer<TMessage> serializer, IWampBinding<TMessage> binding)
        {
            mSerializer = serializer;
            mSubscriptionId = subscriptionId;
            mBinding = binding;
        }

        public void Event(long publicationId, object details)
        {
            WampMessage<TMessage> message =
                mSerializer.Event(mSubscriptionId, publicationId, details);

            Publish(message);
        }

        public void Event(long publicationId, object details, object[] arguments)
        {
            WampMessage<TMessage> message =
                mSerializer.Event(mSubscriptionId, publicationId, details, arguments);

            Publish(message);
        }

        public void Event(long publicationId, object details, object[] arguments, object argumentsKeywords)
        {
            WampMessage<TMessage> message =
                mSerializer.Event(mSubscriptionId, publicationId, details, arguments, argumentsKeywords);

            Publish(message);
        }

        private void Publish(WampMessage<TMessage> message)
        {
            WampMessage<TMessage> raw = mBinding.GetRawMessage(message);
            mSubject.OnNext(raw);
        }
    }
}