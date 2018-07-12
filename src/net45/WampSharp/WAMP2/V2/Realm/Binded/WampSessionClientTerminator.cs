using System;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Realm.Binded
{
    internal class WampSessionClientTerminator<TMessage> : WampSessionClientTerminator
    {
        private readonly IWampClientProxy<TMessage> mClientProxy;
        private readonly long mSessionId;

        public WampSessionClientTerminator(IWampClientProxy<TMessage> clientProxy) :
            base(clientProxy.Session)
        {
            mClientProxy = clientProxy;
        }

        public override void Disconnect(GoodbyeDetails details, string reason)
        {
            using (mClientProxy as IDisposable)
            {
                mClientProxy.Goodbye(details, reason);
                mClientProxy.GoodbyeSent = true;
                mClientProxy.Realm.Goodbye(mClientProxy.Session, details, reason);
            }
        }
    }

    internal class WampSessionClientTerminator : IWampSessionTerminator
    {
        private readonly long mSessionId;

        public WampSessionClientTerminator(long sessionId)
        {
            mSessionId = sessionId;
        }

        public virtual void Disconnect(GoodbyeDetails details, string reason)
        {
            throw new NotImplementedException();
        }

        protected bool Equals(WampSessionClientTerminator other)
        {
            return mSessionId == other.mSessionId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (!(obj is WampSessionClientTerminator casted)) return false;
            return Equals(casted);
        }

        public override int GetHashCode()
        {
            return mSessionId.GetHashCode();
        }
    }
}