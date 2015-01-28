using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [WampDetailsOptions(WampMessageType.v2Call)]
    public class CallOptions : WampDetailsOptions
    {
        public CallOptions()
        {
        }

        public CallOptions(CallOptions other)
        {
            TimeoutMili = other.TimeoutMili;
            ReceiveProgress = other.ReceiveProgress;
            DiscloseMe = other.DiscloseMe;
        }

        [IgnoreProperty]
        [PropertyName("timeout")]
        internal int? TimeoutMili { get; set; }

        [IgnoreProperty]
        [PropertyName("receive_progress")]
        internal bool? ReceiveProgress { get; set; }

        [PropertyName("disclose_me")]
        public bool? DiscloseMe { get; set; }
    }
}