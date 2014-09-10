using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [WampDetailsOptions(WampMessageType.v2Invocation)]
    public class InvocationDetails
    {
        [PropertyName("receive_progress")]
        public bool? ReceiveProgress { get; set; }
    }
}