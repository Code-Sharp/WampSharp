using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Reflection
{
    public class TransportDetails : WampDetailsOptions
    {
        [PropertyName("type")]
        public string Type { get; set; }
    }
}