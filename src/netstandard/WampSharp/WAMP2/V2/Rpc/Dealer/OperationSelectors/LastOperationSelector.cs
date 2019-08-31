using System.Collections.Generic;
using System.Linq;

namespace WampSharp.V2.Rpc
{
    internal class LastOperationSelector : IWampRpcOperationSelector
    {
        public IWampRpcOperation SelectOperation(IReadOnlyList<IWampRpcOperation> operations)
        {
            return operations.LastOrDefault();
        }
    }
}