using System.Reactive.Subjects;
using WampSharp.V2.Client;
using WampSharp.V2.PubSub;

namespace WampSharp.V2
{
    internal static class WampTopicExtensions
    {
        public static IWampSubject ToSubject(this IWampTopic topic)
        {
            return new WampRouterSubject(topic);
        }

        public static IWampSubject ToSubject(this IWampTopicProxy topic, IWampClientConnectionMonitor monitor)
        {
            return new WampClientSubject(topic, monitor);
        }

        public static ISubject<TEvent> ToSubject<TEvent>(this IWampTopic topic)
        {
            IWampSubject subject = topic.ToSubject();

            return new WampTopicSubject<TEvent>(subject);
        }

        public static ISubject<TEvent> ToSubject<TEvent>(this IWampTopicProxy topic,
                                                         IWampClientConnectionMonitor monitor)
        {
            IWampSubject subject = topic.ToSubject(monitor);

            return new WampTopicSubject<TEvent>(subject);
        }
    }
}