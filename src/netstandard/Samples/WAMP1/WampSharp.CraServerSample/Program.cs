using System;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Serialization;
using WampSharp.V1;
using WampSharp.V1.Cra;
using WampSharp.V1.PubSub.Server;
using WampSharp.V1.Rpc;
using WampSharp.V1.Rpc.Server;

namespace WampSharp.CraServerSample
{
    public interface ISample
    {
        [WampRpcMethod("http://example.com/procedures/#hello")]
        string Hello(string name);
    }

    internal class Sample : ISample
    {
        public string Hello(string name)
        {
            //NOTE: You could do this to determine the caller for logging or whatever you need.
            Console.Out.WriteLine("Hello called by user with authKey '{0}'", WampRequestContext.Current.Authenticator.AuthKey);

            return $"Hello back {name}!";
        }
    }

    public class SampleWampCraAuthenticaticationBuilder<TMessage> : WampCraAuthenticaticatorBuilder<TMessage>
    {
        public override WampCraAuthenticator<TMessage> BuildAuthenticator(string clientSessionId)
        {
            return new SampleWampCraAuthenticator<TMessage>(clientSessionId, Formatter, RpcMetadataCatalog, TopicContainer);
        }
    }

    public class SampleWampCraAuthenticator<TMessage> : WampCraAuthenticator<TMessage>
    {
        public SampleWampCraAuthenticator(string clientSessionId, IWampFormatter<TMessage> formatter, IWampRpcMetadataCatalog metadataCatalog, IWampTopicContainer topicContainer)
            : base(clientSessionId, formatter, metadataCatalog, topicContainer)
        {
        }

        public override void OnAuthenticated(string authKey, WampCraPermissions permissions)
        {
            // In the real world, You'd want to possibly base this set of permissions on the 
            // user defined in authKey

            if (string.Equals(authKey, "foobar", StringComparison.Ordinal))
            {
                // For the sample, set the permissions for 'foobar' to allow all currently available RPC methods.
                foreach (IWampRpcMethod rpcMethod in this.GetAllRpcMethods())
                {
                    // You can do some logic here to determine if this client has permissions or not, like finding some attribute
                    // with an auth level and checking that against the client's auth key.
                    permissions.rpc.Add(new WampRpcPermissions(rpcMethod.ProcUri, true));
                }

                permissions.pubsub.Add(new WampPubSubPermissions("http://example.com/topics/", true, true, true));

                //Could also do something like this:
                //foreach (var topic in this.GetTopicContainer().Topics)
                //    permissions.pubsub.Add(new WampPubSubPermissions(topic.TopicUri, false, false, true));
            }
            else
            {
                //An anonymous user will only get this permission (no rpc)
                permissions.pubsub.Add(new WampPubSubPermissions("http://example.com/topics/mytopic1", false, false, true));
            }
        }

        public override WampCraPermissions GetAuthPermissions(string authKey, IDictionary<string, string> extra)
        {
            //set false to not require salted WAMP-CRA
            if(true)
            {
                //NOTE: In the real world, you may want to look at what is in 'extra' (which is what the client sent)
                //      and either respect that or not.  Any mods you make to 'extra' here is what is/must be actually
                //      used to sign the challenge.

                extra["salt"] = "RANDOM SALT";
                extra["keylen"] = "32";
                extra["iterations"] = "1000";
            }

            //NOTE: I don't like the idea of giving all endpoints as part of the challenge.  The last opportunity to
            // set permissions is in OnAuthenticated(), so I add the endpoints there.  You'd want to possibly base this
            // set of permissions on the user defined in authKey
            return new WampCraPermissions();
        }

        public override string GetAuthSecret(string authKey)
        {
            // in the real world, you'd look these up from a DB.
            if (string.Equals(authKey,"foobar", StringComparison.Ordinal))
                return "secret";
            return null;
        }

        public override bool AllowAnonymous => true;
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Sample modeled and compatible with Autobahn Python https://github.com/tavendo/AutobahnPython [examples/twisted/wamp1/authentication/server.py]

            const string location = "ws://127.0.0.1:9000/";
            using (IWampHost host = new DefaultWampCraHost(location, new SampleWampCraAuthenticaticationBuilder<JToken>()))
            {
                ISample instance = new Sample();
                host.HostService(instance);

                host.Open();

                Console.WriteLine("Server is running on " + location);

                //publish just for kicks.
                IWampTopic topic = host.TopicContainer.GetOrCreateTopicByUri("http://example.com/topics/mytopic1", true);
                while (true)
                {
                    Thread.Sleep(2000);
                    if (topic.HasObservers)
                        topic.OnNext("Hello, world!");
                }
            }
        }
    }
}
