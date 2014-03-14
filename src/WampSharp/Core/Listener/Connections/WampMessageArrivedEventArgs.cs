using System;
using WampSharp.Core.Message;

namespace WampSharp.Core.Listener
{
    public class WampMessageArrivedEventArgs<TMessage> : EventArgs
    {
        private readonly WampMessage<TMessage> mMessage;

        public WampMessageArrivedEventArgs(WampMessage<TMessage> message)
        {
            mMessage = message;
        }

        public WampMessage<TMessage> Message
        {
            get
            {
                return mMessage;
            }
        }
    }
}