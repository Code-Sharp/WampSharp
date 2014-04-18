using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MsgPack;
using Newtonsoft.Json.Linq;
using WampSharp.Binding;
using WampSharp.V2;
using WampSharp.V2.Client;
using WampSharp.V2.PubSub;
using WampSharp.V2.Rpc;
using WampSharp.WebSocket4Net;

namespace WampSharp.RpcServerSample
{
    public interface ICalculator
    {
        [WampProcedure("com.arguments.add2")]
        int Add(int x, int y);
    }

    internal class Calculator : ICalculator
    {
        public int Add(int x, int y)
        {
            return x + y;
        }
    }
   
    class Program
    {
        public static void Main(string[] args)
        {
            // http://autobahn.ws/static/file/autobahnjs.html

            const string location = "ws://localhost:9090/ws";
            using (DefaultWampHost host = new DefaultWampHost(location))
            {
                host.Open();

                Calculator instance = new Calculator();

                var binding = new JTokenBinding();

                WampChannelFactory<JToken> channelFactory = 
                    new WampChannelFactory<JToken>(binding);

                WampChannel<JToken> channel =
                    channelFactory.CreateChannel
                        ("realm1",
                         new WebSocket4NetTextConnection<JToken>("ws://localhost:9090/ws",
                                                                 binding));

                channel.Open().Wait();

                Task register =
                    channel.RealmProxy.RpcCatalog.Register
                        (new SyncMethodInfoRpcOperation(instance,
                                                        typeof (ICalculator).GetMethod("Add")),
                         new {});

                register.Wait();


                var messagePackObjectBinding = new MessagePackObjectBinding();

                WampChannelFactory<MessagePackObject> channelFactory2 =
                    new WampChannelFactory<MessagePackObject>
                        (messagePackObjectBinding);

                var channel2 =
                    channelFactory2.CreateChannel
                        ("realm1",
                         new WebSocket4NetBinaryConnection<MessagePackObject>("ws://localhost:9090/ws",
                                                                 messagePackObjectBinding));

                channel2.Open().Wait();

                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                Task<IDisposable> disposable1 =
                    channel2.RealmProxy.TopicContainer.GetTopic("com.myapp.topic2")
                    .Subscribe(new MySubscriber("Subscriber 1:"), dictionary);

                Task<IDisposable> disposable2 =
                    channel2.RealmProxy.TopicContainer.GetTopic("com.myapp.topic2")
                            .Subscribe(new MySubscriber("Subscriber 2:"), dictionary);

                Task<IDisposable> disposable3 =
                    channel2.RealmProxy.TopicContainer.GetTopic("com.myapp.topic2")
                            .Subscribe(new MySubscriber("Subscriber 3:"), dictionary);

                Console.ReadLine();

                disposable1.Result.Dispose();

                Console.ReadLine();

                disposable2.Result.Dispose();
                Console.ReadLine();

                disposable3.Result.Dispose();
                //channel2.RealmProxy.RpcCatalog.Invoke
                //    (new MyCallback(),
                //     new Dictionary<string, string>(),
                //     "com.arguments.add2",
                //     new object[]{1024,768});
                       
                Console.WriteLine("Server is running on " + location);
                Console.ReadLine();
            }
        }
    }

    internal class MySubscriber : IWampTopicSubscriber
    {
        private string mName;

        public MySubscriber(string name)
        {
            mName = name;
        }

        public void Event(long publicationId, object details)
        {
            
        }

        public void Event(long publicationId, object details, object[] arguments)
        {
        }

        public void Event(long publicationId, object details, object[] arguments, object argumentsKeywords)
        {
            Console.WriteLine(mName + publicationId);
        }
    }

    internal class MyCallback : IWampRpcOperationCallback
    {
        public void Result(object details)
        {
        }

        public void Result(object details, object[] arguments)
        {
        }

        public void Result(object details, object[] arguments, object argumentsKeywords)
        {
        }

        public void Error(object details, string error)
        {
        }

        public void Error(object details, string error, object[] arguments)
        {
        }

        public void Error(object details, string error, object[] arguments, object argumentsKeywords)
        {
        }
    }
}