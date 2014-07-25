using System;
using WampSharp.Core.Serialization;

namespace WampSharp.V2.Realm
{
    public abstract class WampSessionEventArgs : EventArgs
    {
        private readonly long mSessionId;

        public WampSessionEventArgs(long sessionId)
        {
            mSessionId = sessionId;
        }

        public long SessionId
        {
            get { return mSessionId; }
        }

        public abstract T DeserializeDetails<T>();
    }

    public class WampSessionEventArgs<TMessage> : WampSessionEventArgs
    {
        private readonly IWampFormatter<TMessage> mFormatter;
        private readonly TMessage mDetails;

        public WampSessionEventArgs(IWampFormatter<TMessage> formatter, long sessionId, TMessage details) : 
            base(sessionId)
        {
            mFormatter = formatter;
            mDetails = details;
        }

        public override T DeserializeDetails<T>()
        {
            return mFormatter.Deserialize<T>(mDetails);
        }
    }
}