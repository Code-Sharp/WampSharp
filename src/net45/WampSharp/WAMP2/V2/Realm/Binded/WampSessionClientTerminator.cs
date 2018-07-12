using System;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Realm.Binded
{
    internal class WampSessionClientTerminator : IWampSessionTerminator
    {
        private readonly IWampClientProxy mClientProxy;
        private readonly long mSessionId;

        public WampSessionClientTerminator(long sessionId)
        {
            mSessionId = sessionId;
        }

        public WampSessionClientTerminator(IWampClientProxy clientProxy)
        {
            mClientProxy = clientProxy;
            mSessionId = mClientProxy.Session;
        }

        public void Disconnect(GoodbyeDetails details, string reason)
        {
            using (mClientProxy as IDisposable)
            {
                mClientProxy.Goodbye(details, reason);
                mClientProxy.GoodbyeSent = true;
            }
        }

        protected bool Equals(WampSessionClientTerminator other)
        {
            return mSessionId == other.mSessionId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((WampSessionClientTerminator) obj);
        }

        public override int GetHashCode()
        {
            return mSessionId.GetHashCode();
        }
    }
}