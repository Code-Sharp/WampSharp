using System.Reactive.Subjects;

namespace WampSharp.V1.Rpc.Client
{
    /// <summary>
    /// Represents a RPC request that is currently
    /// in progress.
    /// </summary>
    public class WampRpcRequest
    {
        /// <summary>
        /// A task used in order to notify when
        /// result arrived.
        /// </summary>
        public ISubject<object> Task { get; set; }

        /// <summary>
        /// The original request.
        /// </summary>
        public WampRpcCall Request { get; set; } 
    }
}