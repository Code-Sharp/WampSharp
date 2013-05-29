using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
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
using WampSharp.PubSub;
using WampSharp.PubSub.Client;
using WampSharp.PubSub.Server;
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
            //MyServer myServer = new MyServer();

            MyServer myServer =
                new MyServer();

            WampListener<JToken> listener =
                new WampListener<JToken>
                    (new FleckWampConnectionListener("ws://localhost:9000/",
                                                     jsonWampMessageFormatter),
                     new WampIncomingMessageHandler<JToken, IWampClient>
                         (new WampRequestMapper<JToken>(myServer.GetType(),
                                                        jsonFormatter),
                          new WampMethodBuilder<JToken, IWampClient>(myServer, jsonFormatter)),
                     new WampClientContainer<JToken, IWampClient>(new WampClientBuilderFactory<JToken>
                                                                      (new WampSessionIdGenerator(),
                                                                       new WampOutgoingRequestSerializer
                                                                           <JToken>(jsonFormatter),
                                                                       new WampOutgoingMessageHandlerBuilder<JToken>())));

            listener.Start();

            Console.ReadLine();

            // RPC Client
            var connection =
                new WebSocketSharpWampConnection("ws://localhost:9000/",
                                                 new JsonWampMessageFormatter());


            var wampServerProxyFactory =
                new Rpc.WampServerProxyFactory<JToken>
                    (connection,
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


            //IAddable proxy = factory.GetClient<IAddable>();
            //int seven = proxy.Add(3, 4);

            //dynamic dynamicProxy = factory.GetDynamicClient();

            //int result = dynamicProxy.Add(3, 4);

            //Task<int> asyncResult = dynamicProxy.Add(3, 4);
            //dynamic dynamicResult = dynamicProxy.Add(3, 4);
            //var resultValue = dynamicResult.Result;
            //object objectResult = dynamicProxy.Add(3, 4);

            //IAsyncAddable asyncProxy = factory.GetClient<IAsyncAddable>();

            //Task<int> task = asyncProxy.Add(3, 4);

            //int taskResult = task.Result;

            WampPubSubClientFactory<JToken> pubsubClientFactory =
                new WampPubSubClientFactory<JToken>
                    (new PubSub.Client.WampServerProxyFactory<JToken>(connection,
                                                               new WampServerProxyBuilder
                                                                   <JToken, IWampPubSubClient<JToken>, IWampServer>(
                                                                   new WampOutgoingRequestSerializer<JToken>(
                                                                       jsonFormatter),
                                                                   new WampServerProxyOutgoingMessageHandlerBuilder
                                                                       <JToken, IWampPubSubClient<JToken>>(
                                                                       new WampServerProxyIncomingMessageHandlerBuilder
                                                                           <JToken, IWampPubSubClient<JToken>>(
                                                                           jsonFormatter)))),
                     jsonFormatter);

            ISubject<object> gargamel =
                pubsubClientFactory.GetSubject<object>("http://example.com/simple");

            var disposable =
                gargamel.Subscribe(x =>
                                   {
                                       gargamel.OnNext(x);
                                   });

            // Autobahn client
            //var server =
            //    wampServerProxyFactory.Create(new MyClient());

            //server.Subscribe(null, "http://example.com/simple");


            Console.ReadLine();
        
            disposable.Dispose();

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

    class MyServer : IWampMissingMethodContract<JToken, IWampClient>
    {
        public void Missing(IWampClient client, WampMessage<JToken> rawMessage)
        {

        }
    }

    //public class MyServer :IWampMissingMethodContract<JToken> //: IWampServer<JToken>
    //{
    //    private int mCallNum;
    //    private readonly List<WampMessage<JToken>> mMessages = new List<WampMessage<JToken>>();

    //    public List<WampMessage<JToken>> Messages
    //    {
    //        get
    //        {
    //            return mMessages;
    //        }
    //    }

    //    public void Prefix(IWampClient client, string prefix, string uri)
    //    {
    //    }

    //    public void Call(IWampClient client, string callId, string procUri, params JToken[] arguments)
    //    {
    //        mCallNum++;

    //        if (mCallNum%2 == 0)
    //        {
    //            client.CallError(callId, "noUri", "You called a even times number", new {x = 2});
    //        }
    //        else
    //        {
    //            int result = arguments.Select(x => (int)x).Sum();
    //            client.CallResult(callId, result);                
    //        }
    //    }

    //    public void Subscribe(IWampClient client, string topicUri)
    //    {
    //        client.Event(topicUri, new {Name = "Yosy", LastName = "Atias"});
    //    }

    //    public void Unsubscribe(IWampClient client, string topicUri)
    //    {
    //    }

    //    public void Publish(IWampClient client, string topicUri, JToken @event)
    //    {
    //        Publish(client, topicUri, @event, false);
    //    }

    //    public void Publish(IWampClient client, string topicUri, JToken @event, bool excludeMe)
    //    {
    //        Publish(client, topicUri, @event, new string[] {client.SessionId});
    //    }

    //    public void Publish(IWampClient client, string topicUri, JToken @event, string[] exclude)
    //    {
    //        Publish(client, topicUri, @event, exclude, new string[]{});
    //    }

    //    public void Publish(IWampClient client, string topicUri, JToken @event, string[] exclude, string[] eligible)
    //    {
    //        client.Event(topicUri, @event);
    //    }

    //    public void Missing(WampMessage<JToken> rawMessage)
    //    {
    //        Messages.Add(rawMessage);
    //    }
    //}

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

    public class TestGenerator
    {
        public string GetAllCalls(IEnumerable<WampMessage<JToken>> messages)
        {
            List<string> members = new List<string>();

            List<string> result = new List<string>();
            result.Add("public void Messages(){");

            string messageName;
            int call = 0;
            foreach (var wampMessage in messages)
            {
                messageName = "rpcCall" + call;
                var code = Serialize(messageName, wampMessage);
                result.Add(code);
                members.Add(GetName(messageName));
                call++;
            }

            result.Add("}");

            string all =
                string.Join(Environment.NewLine,
                            new string[]
                                {
                                    string.Join(Environment.NewLine, members.Select(x => string.Format("private WampMessage<JToken> {0};", x))),
                                    string.Join(Environment.NewLine, result)
                                });

            return all;
        }

        private string Serialize(string messageName, WampMessage<JToken> message)
        {
            var result = new List<string>();

            string name = GetName(messageName);
            result.Add(string.Format("{0} = new WampMessage<JToken>();",
                name));
            result.Add("{");
            result.Add(string.Format("{1}.MessageType = WampMessageType.{0};", message.MessageType, name));
            result.Add(string.Format("JToken[] arguments = new JToken[{0}];", message.Arguments.Length));

            int index = 0;
            foreach (var argument in message.Arguments)
            {
                result.Add(string.Format("arguments[{0}] = JToken.FromObject({1});",
                                         index,
                                         GetCode((dynamic)argument)));
                index++;
            }
            result.Add(string.Format("{0}.Arguments = arguments;", name));
            result.Add("}");

            var code = string.Join(Environment.NewLine, result);
            return code;
        }

        private string GetName(string messageName)
        {
            return "m" + string.Join("",
                                     messageName.Split(new string[] { "_" }, StringSplitOptions.None)
                                                .Select(x => Char.ToUpper(x[0]) + x.Substring(1).ToLower()));
        }

        private string GetCode(JValue argument)
        {
            var value = argument.Value;

            if (value == null)
            {
                return "null";
            }
            if (value is string)
            {
                return string.Format(@"""{0}""", value);
            }
            if (value is DateTime)
            {
                var s = (DateTime)value;

                object[] arguments =
                    new object[] { s.Year, s.Month, s.Day, s.Hour, s.Minute, s.Second, s.Millisecond };

                return string.Format("new DateTime({0})",
                                     string.Join(", ", arguments));
            }
            return argument.ToString().ToLower();
        }

        private string GetCode(JObject argument)
        {
            List<string> result = new List<string>();
            result.Add("new {");
            foreach (KeyValuePair<string, JToken> keyValuePair in argument)
            {
                result.Add(string.Format(@"{0} = {1},",
                                         keyValuePair.Key,
                                         GetCode((dynamic)keyValuePair.Value)));
            }
            result.Add("}");

            return string.Join(Environment.NewLine, result);
        }

        private string GetCode(JArray argument)
        {
            List<string> result = new List<string>();
            result.Add("new object[] {");

            foreach (JToken keyValuePair in argument)
            {
                result.Add(string.Format(@"{0},",
                                         GetCode((dynamic)keyValuePair)));
            }
            result.Add("}");

            return string.Join(Environment.NewLine, result);
        }

    }

    public static class AutobahnTestSuiteRpcCalls
    {
        private static readonly WampMessage<JToken> mRpccall0;
        private static readonly WampMessage<JToken> mRpccall1;
        private static readonly WampMessage<JToken> mRpccall2;
        private static readonly WampMessage<JToken> mRpccall3;
        private static readonly WampMessage<JToken> mRpccall4;
        private static readonly WampMessage<JToken> mRpccall5;
        private static readonly WampMessage<JToken> mRpccall6;
        private static readonly WampMessage<JToken> mRpccall7;
        private static readonly WampMessage<JToken> mRpccall8;
        private static readonly WampMessage<JToken> mRpccall9;

        static AutobahnTestSuiteRpcCalls()
        {
            mRpccall0 = new WampMessage<JToken>();
            {
                mRpccall0.MessageType = WampMessageType.v1Prefix;
                JToken[] arguments = new JToken[2];
                arguments[0] = JToken.FromObject("calc");
                arguments[1] = JToken.FromObject("http://example.com/simple/calc#");
                mRpccall0.Arguments = arguments;
            }
            mRpccall1 = new WampMessage<JToken>();
            {
                mRpccall1.MessageType = WampMessageType.v1Call;
                JToken[] arguments = new JToken[3];
                arguments[0] = JToken.FromObject("0.gmf103gm25efjemi");
                arguments[1] = JToken.FromObject("calc:square");
                arguments[2] = JToken.FromObject(23);
                mRpccall1.Arguments = arguments;
            }
            mRpccall2 = new WampMessage<JToken>();
            {
                mRpccall2.MessageType = WampMessageType.v1Call;
                JToken[] arguments = new JToken[4];
                arguments[0] = JToken.FromObject("0.2qpscjivpf58w7b9");
                arguments[1] = JToken.FromObject("calc:add");
                arguments[2] = JToken.FromObject(23);
                arguments[3] = JToken.FromObject(7);
                mRpccall2.Arguments = arguments;
            }
            mRpccall3 = new WampMessage<JToken>();
            {
                mRpccall3.MessageType = WampMessageType.v1Call;
                JToken[] arguments = new JToken[3];
                arguments[0] = JToken.FromObject("0.407ldfwznk10dx6r");
                arguments[1] = JToken.FromObject("calc:sum");
                arguments[2] = JToken.FromObject(new object[]
                                                     {
                                                         1,
                                                         2,
                                                         3,
                                                         4,
                                                         5,
                                                     });
                mRpccall3.Arguments = arguments;
            }
            mRpccall4 = new WampMessage<JToken>();
            {
                mRpccall4.MessageType = WampMessageType.v1Call;
                JToken[] arguments = new JToken[3];
                arguments[0] = JToken.FromObject("0.d9bcva2fszjpds4i");
                arguments[1] = JToken.FromObject("calc:square");
                arguments[2] = JToken.FromObject(23);
                mRpccall4.Arguments = arguments;
            }
            mRpccall5 = new WampMessage<JToken>();
            {
                mRpccall5.MessageType = WampMessageType.v1Call;
                JToken[] arguments = new JToken[3];
                arguments[0] = JToken.FromObject("0.h1r0hxik62a3v7vi");
                arguments[1] = JToken.FromObject("calc:sqrt");
                arguments[2] = JToken.FromObject(-1);
                mRpccall5.Arguments = arguments;
            }
            mRpccall6 = new WampMessage<JToken>();
            {
                mRpccall6.MessageType = WampMessageType.v1Call;
                JToken[] arguments = new JToken[3];
                arguments[0] = JToken.FromObject("0.bbmk6lzxl6vibe29");
                arguments[1] = JToken.FromObject("calc:square");
                arguments[2] = JToken.FromObject(1001);
                mRpccall6.Arguments = arguments;
            }
            mRpccall7 = new WampMessage<JToken>();
            {
                mRpccall7.MessageType = WampMessageType.v1Call;
                JToken[] arguments = new JToken[3];
                arguments[0] = JToken.FromObject("0.8ddkhnsgjwtgldi");
                arguments[1] = JToken.FromObject("calc:asum");
                arguments[2] = JToken.FromObject(new object[]
                                                     {
                                                         1,
                                                         2,
                                                         3,
                                                     });
                mRpccall7.Arguments = arguments;
            }
            mRpccall8 = new WampMessage<JToken>();
            {
                mRpccall8.MessageType = WampMessageType.v1Call;
                JToken[] arguments = new JToken[3];
                arguments[0] = JToken.FromObject("0.nzdto9xdn6vl5wmi");
                arguments[1] = JToken.FromObject("calc:sum");
                arguments[2] = JToken.FromObject(new object[]
                                                     {
                                                         4,
                                                         5,
                                                         6,
                                                     });
                mRpccall8.Arguments = arguments;
            }
            mRpccall9 = new WampMessage<JToken>();
            {
                mRpccall9.MessageType = WampMessageType.v1Call;
                JToken[] arguments = new JToken[3];
                arguments[0] = JToken.FromObject("0.tuoo16bh5pix80k9");
                arguments[1] = JToken.FromObject("calc:pickySum");
                arguments[2] = JToken.FromObject(new object[]
                                                     {
                                                         0,
                                                         1,
                                                         2,
                                                         3,
                                                         4,
                                                         5,
                                                     });
                mRpccall9.Arguments = arguments;
            }
        }

        public static IEnumerable<WampMessage<JToken>> Calls
        {
            get
            {
                yield return mRpccall0;
                yield return mRpccall1;
                yield return mRpccall2;
                yield return mRpccall3;
                yield return mRpccall4;
                yield return mRpccall5;
                yield return mRpccall6;
                yield return mRpccall7;
                yield return mRpccall8;
                yield return mRpccall9;
            }
        }
    }
}
