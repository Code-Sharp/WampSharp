using System.Collections.Generic;
using System.Linq;

namespace WampSharp.V2.Rpc
{
    internal class RoundrobinOperationSelector : IWampRpcOperationSelector
    {
        private int mIndex = 0;

        public IWampRpcOperation SelectOperation(IReadOnlyList<IWampRpcOperation> operations)
        {
            int count = operations.Count;
            int index = mIndex%count;

            mIndex = (mIndex + 1)%count;

            return operations.ElementAtOrDefault(index);
        }
    }
}