using System;

namespace WampSharp.RawSocket
{
    internal static class ArraySegmentExtensions
    {
        public static T ElementAt<T>(this ArraySegment<T> segment, int index)
        {
            return segment.Array[segment.Offset + index];
        }
    }
}