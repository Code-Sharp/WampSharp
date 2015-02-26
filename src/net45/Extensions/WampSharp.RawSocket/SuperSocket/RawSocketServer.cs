using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace WampSharp.RawSocket
{
    internal class RawSocketServer : AppServer<RawSocketSession, BinaryRequestInfo>
    {
        private ConnectionListener mConnectionListener;

        public RawSocketServer() : base(new RawSocketFilterFactory())
        {
        }

        protected override bool RegisterSession(string sessionId, RawSocketSession appSession)
        {
            mConnectionListener.OnNewConnection(appSession);
            return true;
        }

        protected override void ExecuteCommand(RawSocketSession session, BinaryRequestInfo requestInfo)
        {
            session.OnNewMessage(requestInfo.Body);
        }

        public void SetConnectionListener(ConnectionListener textConnectionListener)
        {
            mConnectionListener = textConnectionListener;
        }
    }
}