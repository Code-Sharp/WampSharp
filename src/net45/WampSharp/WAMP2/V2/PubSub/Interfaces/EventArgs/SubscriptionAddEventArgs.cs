using System;
using WampSharp.Core.Serialization;

namespace WampSharp.V2.PubSub
{
    public abstract class SubscriptionAddEventArgs : EventArgs
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

        public abstract T DeserializeOptions<T>();
    }

    public class SubscriptionAddEventArgs<TMessage> : SubscriptionAddEventArgs
    {
        private readonly TMessage mOptions;
        private readonly IWampFormatter<TMessage> mFormatter;

        public SubscriptionAddEventArgs(IRemoteWampTopicSubscriber subscriber, TMessage options, IWampFormatter<TMessage> formatter) : base(subscriber, options)
        {
            mOptions = options;
            mFormatter = formatter;
        }

        public override T DeserializeOptions<T>()
        {
            return mFormatter.Deserialize<T>(mOptions);
        }
    }
}