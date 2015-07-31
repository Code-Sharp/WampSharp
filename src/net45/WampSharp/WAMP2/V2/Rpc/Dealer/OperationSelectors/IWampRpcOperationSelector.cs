using System.Collections.Generic;

namespace WampSharp.V2.Rpc
{
    internal interface IWampRpcOperationSelector
    {
        IWampRpcOperation SelectOperation(IReadOnlyList<IWampRpcOperation> operations);
    }
}