using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [WampDetailsOptions(WampMessageType.v2Yield)]
    public class YieldOptions : WampDetailsOptions
    {
        [IgnoreProperty]
        [PropertyName("progress")]
        internal bool? Progress { get; set; }
    }
}