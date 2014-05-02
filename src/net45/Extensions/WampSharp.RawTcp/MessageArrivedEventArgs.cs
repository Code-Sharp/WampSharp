using System;

namespace WampSharp.RawTcp
{
    internal class MessageArrivedEventArgs : EventArgs
    {
        private readonly byte[] mMessage;

        public MessageArrivedEventArgs(byte[] message)
        {
            mMessage = message;
        }

        public byte[] Message
        {
            get { return mMessage; }
        }
    }
}