using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [WampDetailsOptions(WampMessageType.v2Subscribe)]
    public class SubscribeOptions : WampDetailsOptions
    {
        [IgnoreProperty]
        [PropertyName("match")]
        internal string Match { get; set; }
    }
}