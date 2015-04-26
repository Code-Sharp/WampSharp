using System;
using WampSharp.V1;
using WampSharp.V1.PubSub.Server;

namespace WampSharp.PubSubServerSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // http://autobahn.ws/static/file/autobahnjs.html
            const string location = "ws://127.0.0.1:9000/";
            using (IWampHost host = new DefaultWampHost(location))
            {
                host.Open();
                // Use this in order to publish events to subscribers.
                IWampTopicContainer topicContainer = host.TopicContainer;
                Console.WriteLine("Server is running on " + location);
                Console.ReadLine();
            }

        }
   }
}
