using System.Runtime.Serialization;
using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [DataContract]
    [WampDetailsOptions(WampMessageType.v2Invocation)]
    public class InvocationDetails : WampDetailsOptions
    {
        public InvocationDetails()
        {
        }

        public InvocationDetails(InvocationDetails details)
        {
            Timeout = details.Timeout;
            ReceiveProgress = details.ReceiveProgress;
            Caller = details.Caller;
            Procedure = details.Procedure;
            OriginalValue = details.OriginalValue;
        }

        [DataMember(Name = "timeout")]
        internal bool? Timeout { get; set; }

        /// <summary>
        /// Indicates if the callee should produce progressive results.
        /// </summary>
        [DataMember(Name = "receive_progress")]
        public bool? ReceiveProgress { get; internal set; }

        /// <summary>
        /// The WAMP session ID of the caller.
        /// </summary>
        [DataMember(Name = "caller")]
        public long? Caller { get; internal set; }

        /// <summary>
        /// The actual procedure being called.
        /// </summary>
        [DataMember(Name = "procedure")]
        public string Procedure { get; internal set; }
    }
}