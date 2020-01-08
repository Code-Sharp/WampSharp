using System.Reflection;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;

// ReSharper disable once CheckNamespace
namespace WampSharp.V2
{
    public class SubscriberRegistrationInterceptor : ISubscriberRegistrationInterceptor
    {
        private readonly SubscribeOptions mSubscriptionOptions;

        public SubscriberRegistrationInterceptor() : this(new SubscribeOptions())
        {
        }

        public SubscriberRegistrationInterceptor(SubscribeOptions subscriptionOptions)
        {
            mSubscriptionOptions = subscriptionOptions;
        }

        public virtual bool IsSubscriberHandler(MethodInfo method)
        {
            return method.IsDefined(typeof (WampTopicAttribute));
        }

        public virtual string GetTopicUri(MethodInfo method)
        {
            return method.GetCustomAttribute<WampTopicAttribute>().Topic;
        }

        public virtual SubscribeOptions GetSubscribeOptions(MethodInfo method)
        {
            return mSubscriptionOptions;
        }
    }
}
