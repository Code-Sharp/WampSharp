using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using SuperSocket.Common;
using SuperSocket.Facility.Protocol;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace WampSharp.RawTcp
{
    class RawWampFilterFactory : IReceiveFilterFactory<BinaryRequestInfo>
    {
        public IReceiveFilter<BinaryRequestInfo> CreateFilter(IAppServer appServer, IAppSession appSession, IPEndPoint remoteEndPoint)
        {
            return new RawWampFilter();
        }
    }

    internal class RawWampFilter : FixedHeaderReceiveFilter<BinaryRequestInfo> 
    {
        public RawWampFilter()
            : base(sizeof(int))
        {
        }

        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        {
            IEnumerable<byte> bytes = header.Skip(offset).Take(length);

            if (BitConverter.IsLittleEndian)
            {
                bytes = bytes.Reverse();
            }

            byte[] array = bytes.ToArray();

            int result = BitConverter.ToInt32(array, 0);

            return result;
        }

        protected override BinaryRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset,
                                                                int length)
        {
            return new BinaryRequestInfo(null, bodyBuffer.CloneRange(offset, length));
        }
    }
}