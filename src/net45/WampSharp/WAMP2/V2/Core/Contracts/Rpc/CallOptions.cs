using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [WampDetailsOptions(WampMessageType.v2Call)]
    public class CallOptions : WampDetailsOptions
    {
        [PropertyName("timeout")]
        public int? TimeoutMili { get; set; }

        [PropertyName("receive_progress")]
        public bool? ReceiveProgress { get; set; }

        [PropertyName("disclose_me")]
        public bool? DiscloseMe { get; set; }
    }
}