using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Reflection
{
    public class WampTransportDetails : WampDetailsOptions
    {
        [PropertyName("type")]
        public string Type { get; set; }
    }
}