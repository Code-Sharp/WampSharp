using System.Collections;
using System.Collections.Generic;
using WampSharp.Core.Message;

namespace WampSharp.Tests.TestHelpers
{
    public class WampMessageEqualityComparer<TMessage> : IEqualityComparer<WampMessage<TMessage>>
    {
        private readonly IEqualityComparer mRawComparer;

        public WampMessageEqualityComparer(IEqualityComparer rawComparer)
        {
            mRawComparer = rawComparer;
        }

        public bool Equals(WampMessage<TMessage> x, WampMessage<TMessage> y)
        {
            IStructuralEquatable xArguments = x.Arguments;
            IStructuralEquatable yArguments = y.Arguments;

            return x.MessageType == y.MessageType &&
                   xArguments.Equals(yArguments, mRawComparer);
        }

        public int GetHashCode(WampMessage<TMessage> obj)
        {
            IStructuralEquatable xArguments = obj.Arguments;

            return obj.MessageType.GetHashCode() ^
                xArguments.GetHashCode(mRawComparer);
        }
    }
}