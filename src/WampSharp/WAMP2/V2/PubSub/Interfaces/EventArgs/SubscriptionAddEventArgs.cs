using System;

namespace WampSharp.V2.PubSub
{
    public class SubscriptionAddEventArgs : EventArgs
    {
        private readonly object mOptions;
        private readonly IWampTopicSubscriber mSubscriber;

        public SubscriptionAddEventArgs(IWampTopicSubscriber subscriber, object options)
        {
            mOptions = options;
            mSubscriber = subscriber;
        }

        public IWampTopicSubscriber Subscriber
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