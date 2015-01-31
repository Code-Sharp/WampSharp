using System;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace WampSharp.RawSocket
{
    internal class RawSession : AppSession<RawSession, BinaryRequestInfo>
    {
        public void OnNewMessage(byte[] message)
        {
            RaiseMessageArrived(new MessageArrivedEventArgs(message));
        }

        public event EventHandler<MessageArrivedEventArgs> MessageArrived;

        protected virtual void RaiseMessageArrived(MessageArrivedEventArgs e)
        {
            EventHandler<MessageArrivedEventArgs> handler = MessageArrived;
            
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}