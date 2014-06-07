using System;

namespace WampSharp.V2.PubSub
{
    public class SubscriptionAddEventArgs : EventArgs
    {
        private readonly object mOptions;
        private readonly IRemoteWampTopicSubscriber mSubscriber;

        public SubscriptionAddEventArgs(IRemoteWampTopicSubscriber subscriber, object options)
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

        public object Options
        {
            get
            {
                return mOptions;
            }
        }
    }
}