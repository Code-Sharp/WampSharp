using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WampSharp.V1;
using WampSharp.V1.Api.Server;
using WampSharp.V1.PubSub.Server;
using WampSharp.V1.PubSub.Server.Interfaces;

namespace WampSharp.PubSubServerSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // http://autobahn.ws/static/file/autobahnjs.html

            const string location = "ws://localhost:9000/";
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
