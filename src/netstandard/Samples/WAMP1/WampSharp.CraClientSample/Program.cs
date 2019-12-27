using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Cra;
using WampSharp.Core.Serialization;
using WampSharp.Newtonsoft;
using WampSharp.V1;
using WampSharp.V1.Cra;
using WampSharp.V1.Rpc;

namespace WampSharp.CraClientSample
{
    public interface ISample
    {
        [WampRpcMethod("http://example.com/procedures/#hello")]
        string Hello(string name);
    }

    class Program
    {
        static void Main(string[] args)
        {
            //Sample modeled and compatible with Autobahn Python https://github.com/tavendo/AutobahnPython [examples/twisted/wamp1/authentication/client.py]

            JsonSerializer serializer = new JsonSerializer();
            JsonFormatter formatter = new JsonFormatter(serializer);
            DefaultWampChannelFactory channelFactory = new DefaultWampChannelFactory(serializer);
            IWampChannel<JToken> channel = channelFactory.CreateChannel("ws://127.0.0.1:9000/");

            channel.Open();

            try
            {
                IWampCraProcedures proxy = channel.GetRpcProxy<IWampCraProcedures>();

                //TODO: This authenticates as a user, or as anonymous based on a conditional.
                WampCraPermissions permissions;

                bool authenticateAsUser = true;

                if (authenticateAsUser)
                {
                    permissions = Authenticate(proxy, formatter, "foobar", null, "secret");
                }
                else
                {
                    permissions = Authenticate(proxy, formatter, null, null, null);
                }

                Console.WriteLine("permissions: {0}", formatter.Serialize(permissions));
            }
            catch (WampRpcCallException ex)
            {
                Console.Out.WriteLine("Authenticate WampRpcCallException: '{0}' to uri '{1}'", ex.Message,
                                      ex.ProcUri);
            }

            try
            {
                //Expect failure if running against default server sample and anonymous.

                ISample proxy = channel.GetRpcProxy<ISample>();
                string result = proxy.Hello("Foobar");
                Console.WriteLine(result);
            }
            catch (WampRpcCallException ex)
            {
                Console.Out.WriteLine("Hello WampRpcCallException: '{0}' to uri '{1}'", ex.Message,
                                      ex.ProcUri);
            }

            //server sample allows for subscribe for anon and foobar user.
            ISubject<string> subject = channel.GetSubject<string>("http://example.com/topics/mytopic1");
            IDisposable cancelation = subject.Subscribe(x => Console.WriteLine("mytopic1: " + x));

            //server sample does not allow for publish if anon.  Therefore, if logged in, expect to see
            // this in console, otherwise it will silently fail to publish (Wamp1 makes it silent).
            subject.OnNext("Hello World From Client!");

            Console.ReadLine();
            channel.Close();
        }

        /// <summary>
        /// Authenticate the WAMP session to server.
        /// </summary>
        /// <param name="proxy">The proxy.</param>
        /// <param name="formatter">The formatter.</param>
        /// <param name="authKey">The key of the authentication credentials, something like a user or
        /// application name.</param>
        /// <param name="authExtra">Any extra authentication information.</param>
        /// <param name="authSecret">The secret of the authentication credentials, something like the user
        /// password or application secret key.</param>
        /// <returns>The WampCraPermissions.</returns>
        static WampCraPermissions Authenticate(IWampCraProcedures proxy, IWampFormatter<JToken> formatter, string authKey, IDictionary<string, string> authExtra, string authSecret)
        {
            string challenge = proxy.AuthReq(authKey, authExtra);
            if (string.IsNullOrEmpty(authKey))
            {
                return proxy.Auth(null);
            }
            WampCraChallenge info = formatter.Deserialize<WampCraChallenge>(JObject.Parse(challenge));
            string sig = WampCraHelpers.AuthSignature(challenge, authSecret, info.authextra);
            return proxy.Auth(sig);
        }
    }
}
