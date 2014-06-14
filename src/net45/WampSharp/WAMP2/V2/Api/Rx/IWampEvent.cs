using System.Collections.Generic;

namespace WampSharp.V2
{
    public interface IWampEvent
    {
        IDictionary<string, object> Details { get; }
        object[] Arguments { get; }
        IDictionary<string, object> ArgumentsKeywords { get; }
    }
}