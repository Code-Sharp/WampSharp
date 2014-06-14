using System.Collections.Generic;

namespace WampSharp.V2
{
    public class WampEvent : IWampEvent
    {
        public IDictionary<string, object> Details { get; set; }
        public object[] Arguments { get; set; }
        public IDictionary<string, object> ArgumentsKeywords { get; set; }
    }
}