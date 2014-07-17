using System.Collections.Generic;

namespace WampSharp.V2.Rpc
{
    internal interface IOperationExtractor
    {
        IEnumerable<IWampRpcOperation> ExtractOperations(object instance);
    }
}