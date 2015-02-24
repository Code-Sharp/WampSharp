using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [WampDetailsOptions(WampMessageType.v2Result)]
    public class ResultDetails : WampDetailsOptions
    {
        [PropertyName("progress")]
        public bool? Progress { get; set; }
    }
}