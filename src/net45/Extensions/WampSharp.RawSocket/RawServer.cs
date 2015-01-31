using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace WampSharp.RawSocket
{
    class RawServer : AppServer<RawSession, BinaryRequestInfo>
    {
        private ConnectionListener mConnectionListener;

        public RawServer() : base(new RawWampFilterFactory())
        {
        }

        protected override bool RegisterSession(string sessionId, RawSession appSession)
        {
            mConnectionListener.OnNewConnection(appSession);
            return true;
        }

        protected override void ExecuteCommand(RawSession session, BinaryRequestInfo requestInfo)
        {
            session.OnNewMessage(requestInfo.Body);
        }

        public void SetConnectionListener(ConnectionListener textConnectionListener)
        {
            mConnectionListener = textConnectionListener;
        }
    }
}