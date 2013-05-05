using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WampSharp.Core;
using WampSharp.Core.Contracts;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Listener;
using WampSharp.Core.Proxy;
using WampSharp.Fleck;

namespace WampSharp.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            JsonWampMessageFormatter jsonWampMessageFormatter = new JsonWampMessageFormatter();
            JsonFormatter jsonFormatter = new JsonFormatter();
            MyServer myServer = new MyServer();

            WampListener<JToken> listener =
                new WampListener<JToken>
                    (new FleckWampConnectionListener("ws://localhost:9000/",
                                                     jsonWampMessageFormatter),
                     new WampIncomingMessageHandler<JToken>
                         (new WampRequestMapper<JToken>(typeof (MyServer),
                                                        jsonFormatter),
                          new WampMethodBuilder<JToken>(myServer, jsonFormatter)),
                     new WampClientContainer<JToken>(new WampClientBuilderFactory<JToken>
                                                         (new WampSessionIdGenerator(),
                                                          new WampOutgoingRequestSerializer
                                                              <JToken>(jsonFormatter),
                                                          new JsonWampOutgoingHandlerBuilder
                                                              <JToken>())));

            listener.Start();

            Console.ReadLine();
        }
    }

    public class MyServer : IWampServer<JToken>
    {
        private int mCallNum;

        public void Prefix(IWampClient client, string prefix, string uri)
        {
        }

        public void Call(IWampClient client, string callId, string procUri, params JToken[] arguments)
        {
            mCallNum++;

            if (mCallNum%2 == 0)
            {
                client.CallError(callId, "noUri", "You called a even times number", new {x = 2});
            }
            else
            {
                int result = arguments.Select(x => (int)x).Sum();
                client.CallResult(callId, result);                
            }
        }

        public void Subscribe(IWampClient client, string topicUri)
        {
            client.Event(topicUri, new {Name = "Yosy", LastName = "Atias"});
        }

        public void Unsubscribe(IWampClient client, string topicUri)
        {
        }

        public void Publish(IWampClient client, string topicUri, JToken @event)
        {
            Publish(client, topicUri, @event, false);
        }

        public void Publish(IWampClient client, string topicUri, JToken @event, bool excludeMe)
        {
            Publish(client, topicUri, @event, new string[] {client.SessionId});
        }

        public void Publish(IWampClient client, string topicUri, JToken @event, string[] exclude)
        {
            Publish(client, topicUri, @event, exclude, new string[]{});
        }

        public void Publish(IWampClient client, string topicUri, JToken @event, string[] exclude, string[] eligible)
        {
            client.Event(topicUri, @event);
        }
    }
}
