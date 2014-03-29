using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace WampSharp.RawTcp
{
    class RawServer : AppServer<RawSession, BinaryRequestInfo>
    {
        public RawServer() : base(new RawWampFilterFactory())
        {
        }

        protected override void ExecuteCommand(RawSession session, BinaryRequestInfo requestInfo)
        {
            session.OnNewMessage(requestInfo.Body);
        }
    }
}