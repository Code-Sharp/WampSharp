using System;

namespace WampSharp.V1.Rpc.Client
{
    /// <summary>
    /// Represents a WAMP rpc call.
    /// </summary>
    public class WampRpcCall
    {
        /// <summary>
        /// The call id of the this call.
        /// </summary>
        public string CallId { get; set; }

        /// <summary>
        /// The proc uri to call.
        /// </summary>
        public string ProcUri { get; set; }

        /// <summary>
        /// The arguments sent with the call.
        /// </summary>
        public object[] Arguments { get; set; }

        /// <summary>
        /// The expected return type of the call.
        /// </summary>
        public Type ReturnType { get; set; }
    }
}