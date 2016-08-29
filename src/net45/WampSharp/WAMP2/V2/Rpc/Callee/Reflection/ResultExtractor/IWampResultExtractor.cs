using System.Collections.Generic;

namespace WampSharp.V2.Rpc
{
    internal interface IWampResultExtractor
    {
        object[] GetArguments(object result);
        IDictionary<string, object> GetArgumentKeywords(object result);
    }
}