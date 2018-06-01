using System;
using System.Buffers;
using System.Runtime.InteropServices;

namespace WampSharp.AspNetCore.RawSocket
{
    internal static class ReadOnlyMemoryExtensions
    {
        public static ArraySegment<byte> ToArraySegment(this in ReadOnlySequence<byte> input)
        {
            if (input.IsSingleSegment)
            {
                bool isArray = MemoryMarshal.TryGetArray(input.First, out ArraySegment<byte> arraySegment);
                return arraySegment;
            }

            // Should be rare
            return new ArraySegment<byte>(input.ToArray());
        }
    }
}