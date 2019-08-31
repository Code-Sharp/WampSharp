using System.Collections.Generic;

namespace WampSharp.Core.Message
{
    internal class WampMessageTypeComparer : IEqualityComparer<WampMessageType>
    {
        public bool Equals(WampMessageType x, WampMessageType y)
        {
            return x == y;
        }

        public int GetHashCode(WampMessageType obj)
        {
            return (int) obj;
        }
    }
}