using System;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    public class WampSubscriptionAddEventArgs : EventArgs
    {
        private readonly SubscribeOptions mOptions;
        private readonly IRemoteWampTopicSubscriber mSubscriber;

        public WampSubscriptionAddEventArgs(IRemoteWampTopicSubscriber subscriber, SubscribeOptions options)
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

        public SubscribeOptions Options
        {
            get
            {
                return mOptions;
            }
        }
    }
}