using System.Collections.Generic;
using System.Linq;

namespace WampSharp.V2.Rpc
{
    internal class FirstOperationSelector : IWampRpcOperationSelector
    {
        public IWampRpcOperation SelectOperation(IReadOnlyList<IWampRpcOperation> operations)
        {
            return operations.FirstOrDefault();
        }
    }
}