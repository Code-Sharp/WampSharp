using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [WampDetailsOptions(WampMessageType.v2Yield)]
    public class YieldOptions : WampDetailsOptions
    {
        [PropertyName("progress")]
        public bool? Progress { get; set; }
    }
}