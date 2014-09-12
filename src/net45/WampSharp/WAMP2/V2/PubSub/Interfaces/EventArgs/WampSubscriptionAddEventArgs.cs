using System;

namespace WampSharp.V2.PubSub
{
    public class WampSubscriptionAddEventArgs : EventArgs
    {
        private readonly ISerializedValue mOptions;
        private readonly IRemoteWampTopicSubscriber mSubscriber;

        public WampSubscriptionAddEventArgs(IRemoteWampTopicSubscriber subscriber, ISerializedValue options)
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