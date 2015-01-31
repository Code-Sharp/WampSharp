using System;
using System.Threading;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace WampSharp.RawSocket
{
    internal class RawSocketSession : AppSession<RawSocketSession, BinaryRequestInfo>
    {
        private readonly ManualResetEvent mLock = new ManualResetEvent(false);

        protected override void OnSessionStarted()
        {
            RaiseSessionStarted();
            base.OnSessionStarted();
            mLock.Set();
        }

        protected override void OnSessionClosed(CloseReason reason)
        {
            mLock.Reset();
            RaiseConnectionLost(reason);
            base.OnSessionClosed(reason);
        }

        public void OnNewMessage(byte[] message)
        {
            mLock.WaitOne();
            RaiseMessageArrived(new MessageArrivedEventArgs(message));
        }

        public event EventHandler SessionStarted;
        public event EventHandler<CloseReason> SessionClosed;
        public event EventHandler<MessageArrivedEventArgs> MessageArrived;

        protected virtual void RaiseSessionStarted()
        {
            var handler = SessionStarted;

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        protected virtual void RaiseConnectionLost(CloseReason e)
        {
            EventHandler<CloseReason> handler = SessionClosed;

            if (handler != null)
            {
                handler(this, e);
            }
        }

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