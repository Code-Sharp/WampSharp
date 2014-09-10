using WampSharp.Core.Message;

namespace WampSharp.V2.Core.Contracts
{
    [WampDetailsOptions(WampMessageType.v2Publish)]
    public class EventDetails : WampDetailsOptions
    {
        [PropertyName("publisher")]
        public long? Publisher { get; set; }
    }
}