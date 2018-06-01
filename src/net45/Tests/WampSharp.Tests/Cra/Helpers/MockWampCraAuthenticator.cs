using System;
using System.Collections.Generic;
using WampSharp.Core.Serialization;
using WampSharp.V1.Cra;
using WampSharp.V1.PubSub.Server;
using WampSharp.V1.Rpc.Server;

namespace WampSharp.Tests.Cra.Helpers
{
    public class MockWampCraAuthenticator<TMessage> : WampCraAuthenticator<TMessage>
    {
        public MockWampCraAuthenticator(string clientSessionId, IWampFormatter<TMessage> formatter, IWampRpcMetadataCatalog metadataCatalog, IWampTopicContainer topicContainer)
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
            if (RequireSalted)
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
            if (string.Equals(authKey, "foobar", StringComparison.Ordinal))
            {
                return "secret";
            }

            return null;
        }

        public override bool AllowAnonymous => AllowAnonymousAccess;

        public bool RequireSalted { get; set; }

        public bool AllowAnonymousAccess { get; set; }
    }
}