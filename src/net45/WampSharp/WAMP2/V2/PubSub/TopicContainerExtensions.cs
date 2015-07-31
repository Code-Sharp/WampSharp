using System.Collections.Generic;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.PubSub
{
    internal static class TopicContainerExtensions
    {
        public static long Publish
            (this IWampTopicContainer container, 
             PublishOptions options, 
             string topicUri, 
             params object[] arguments)
        {
            return Publish(container, options, topicUri, arguments, null);
        }

        public static long Publish
            (this IWampTopicContainer container, 
            PublishOptions options, 
            string topicUri, 
            object[] arguments, 
            IDictionary<string, object> argumentKeywords)
        {
            if (arguments == null)
            {
                return container.Publish(WampObjectFormatter.Value,
                                         options,
                                         topicUri);
            }
            else if (argumentKeywords == null)
            {
                return container.Publish(WampObjectFormatter.Value,
                                         options,
                                         topicUri,
                                         arguments);
            }
            else
            {
                return container.Publish(WampObjectFormatter.Value,
                                         options,
                                         topicUri,
                                         arguments,
                                         argumentKeywords);
            }
        }
    }
}