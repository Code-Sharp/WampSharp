using System.Collections.Generic;

namespace WampSharp.V1.Cra
{
    /// <summary>
    /// This type is returned to the client from a successful call to IWampCra.auth().
    /// </summary>
    /// <remarks>As this is defined as part of WAMP-CRA (v1), it should not be changed.</remarks>
    public class WampCraPermissions
    {
        public List<WampRpcPermissions> rpc { get; private set; }

        public List<WampPubSubPermissions> pubsub { get; private set; }

        public WampCraPermissions()
        {
            rpc = new List<WampRpcPermissions>();
            pubsub = new List<WampPubSubPermissions>();
        }

        public WampCraPermissions(List<WampRpcPermissions> rpc, List<WampPubSubPermissions> pubsub)
        {
            this.rpc = rpc;
            this.pubsub = pubsub;
        }
    }
}