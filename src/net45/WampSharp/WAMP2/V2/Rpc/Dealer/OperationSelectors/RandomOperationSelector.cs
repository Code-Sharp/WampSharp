using System;
using System.Collections.Generic;
using System.Linq;

namespace WampSharp.V2.Rpc
{
    internal class RandomOperationSelector : IWampRpcOperationSelector
    {
        private readonly Random mRandom = new Random();

        public IWampRpcOperation SelectOperation(IReadOnlyList<IWampRpcOperation> operations)
        {
            int index = mRandom.Next(operations.Count);

            return operations.ElementAtOrDefault(index);
        }
    }
}