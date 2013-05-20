using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WampSharp.Core;
using WampSharp.Core.Client;
using WampSharp.Core.Contracts;
using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Listener;
using WampSharp.Core.Listener.V1;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;
using WampSharp.Fleck;
using WampSharp.Rpc;

namespace WampSharp.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //FleckLog.Level = LogLevel.Debug;

            // Server
            // See client at: http://autobahn.ws/static/file/autobahnjs.html
            JsonWampMessageFormatter jsonWampMessageFormatter = new JsonWampMessageFormatter();
            JsonFormatter jsonFormatter = new JsonFormatter();
            MyServer myServer = new MyServer();

            WampListener<JToken> listener =
                new WampListener<JToken>
                    (new FleckWampConnectionListener("ws://localhost:9000/",
                                                     jsonWampMessageFormatter),
                     new WampIncomingMessageHandler<JToken, IWampClient>
                         (new WampRequestMapper<JToken>(typeof(MyServer),
                                                        jsonFormatter),
                          new WampMethodBuilder<JToken, IWampClient>(myServer, jsonFormatter)),
                     new WampClientContainer<JToken, IWampClient>(new WampClientBuilderFactory<JToken>
                                                                      (new WampSessionIdGenerator(),
                                                                       new WampOutgoingRequestSerializer
                                                                           <JToken>(jsonFormatter),
                                                                       new WampOutgoingMessageHandlerBuilder<JToken>())));

            listener.Start();


            // RPC Client
            var wampServerProxyFactory =
                new WampServerProxyFactory<JToken>
                    (new WebSocketSharpWampConnection("ws://localhost:9000/",
                                                      new JsonWampMessageFormatter()),
                     new WampServerProxyBuilder<JToken, IWampRpcClient<JToken>, IWampServer>
                         (new WampOutgoingRequestSerializer<JToken>
                              (jsonFormatter),
                          new WampServerProxyOutgoingMessageHandlerBuilder<JToken, IWampRpcClient<JToken>>
                              (new WampServerProxyIncomingMessageHandlerBuilder<JToken, IWampRpcClient<JToken>>
                                   (jsonFormatter))));

            WampRpcClientFactory factory =
                new WampRpcClientFactory
                    (new WampRpcSerializer(new WampDelegateProcUriMapper(x => x.Name)),
                     new WampRpcClientHandlerBuilder<JToken>(jsonFormatter,
                                                             wampServerProxyFactory));


            IAddable proxy = factory.GetClient<IAddable>();
            int seven = proxy.Add(3, 4);

            dynamic dynamicProxy = factory.GetDynamicClient();

            int result = dynamicProxy.Add(3, 4);
            //Task<int> asyncResult = dynamicProxy.Add(3, 4);
            //dynamic dynamicResult = dynamicProxy.Add(3, 4);
            //var resultValue = dynamicResult.Result;
            //object objectResult = dynamicProxy.Add(3, 4);
           
            //IAsyncAddable asyncProxy = factory.GetClient<IAsyncAddable>();

            //Task<int> task = asyncProxy.Add(3, 4);

            //int taskResult = task.Result;

            // Autobahn client
            //var server =
            //    wampServerProxyFactory.Create(new MyClient());

            //server.Subscribe(null, "http://example.com/simple");


            Console.ReadLine();        
        }
    }



    public interface IAsyncAddable
    {
        Task<int> Add(int x, int y);         
    }

    public interface IAddable
    {
        int Add(int x, int y);
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

    class MyClient : IWampAuxiliaryClient, IWampRpcClient<JToken>, IWampMissingMethodContract<JToken>
    {
        private string mSessionId;

        public void Welcome(string sessionId, int protocolVersion, string serverIdent)
        {
            mSessionId = sessionId;
        }

        public void CallResult(string callId, JToken result)
        {
        }

        public void CallError(string callId, string errorUri, string errorDesc)
        {
        }

        public void CallError(string callId, string errorUri, string errorDesc, JToken errorDetails)
        {
        }

        public string SessionId
        {
            get
            {
                return mSessionId;
            }
        }

        public void Missing(WampMessage<JToken> rawMessage)
        {
        }
    }
}
