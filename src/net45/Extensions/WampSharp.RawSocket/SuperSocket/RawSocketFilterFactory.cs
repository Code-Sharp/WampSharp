using System.Net;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace WampSharp.RawSocket
{
    internal class RawSocketFilterFactory : IReceiveFilterFactory<BinaryRequestInfo>
    {
        public IReceiveFilter<BinaryRequestInfo> CreateFilter(IAppServer appServer, IAppSession appSession, IPEndPoint remoteEndPoint)
        {
            return new RawSocketFilter();
        }
    }
}