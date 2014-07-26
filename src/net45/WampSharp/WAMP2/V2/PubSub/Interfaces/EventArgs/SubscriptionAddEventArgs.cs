using System;

namespace WampSharp.V2.PubSub
{
    public class SubscriptionAddEventArgs : EventArgs
    {
        private readonly ISerializedValue mOptions;
        private readonly IRemoteWampTopicSubscriber mSubscriber;

        public SubscriptionAddEventArgs(IRemoteWampTopicSubscriber subscriber, ISerializedValue options)
        {
            mOptions = options;
            mSubscriber = subscriber;
        }

        public IRemoteWampTopicSubscriber Subscriber
        {
            get
            {
                return mSubscriber;
            }
        }

        public ISerializedValue Options
        {
            get
            {
                return mOptions;
            }
        }
    }
}