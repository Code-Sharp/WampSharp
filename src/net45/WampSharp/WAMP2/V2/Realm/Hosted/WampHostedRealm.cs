using System;
using WampSharp.Core.Serialization;
using WampSharp.V2.Core;
using WampSharp.V2.PubSub;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.Realm
{
    internal class WampHostedRealm : IWampHostedRealm, IWampRealmGate
    {
        private readonly IWampRealm mUnderlyingRealm;

        public WampHostedRealm(IWampRealm underlyingRealm)
        {
            mUnderlyingRealm = underlyingRealm;
        }

        public string Name
        {
            get { return mUnderlyingRealm.Name; }
        }

        public IWampRpcOperationCatalog RpcCatalog
        {
            get { return mUnderlyingRealm.RpcCatalog; }
        }

        public IWampTopicContainer TopicContainer
        {
            get { return mUnderlyingRealm.TopicContainer; }
        }

        public IWampRealmServiceProvider Services
        {
            get
            {
                // Should not be called.
                throw new NotSupportedException();
            }
        }

        public event EventHandler<WampSessionEventArgs> SessionCreated;

        public event EventHandler<WampSessionCloseEventArgs> SessionClosed;

        public void Hello<TMessage>(IWampFormatter<TMessage> formatter, long sessionId, TMessage details)
        {
            RaiseSessionCreated(new WampSessionEventArgs(sessionId, new SerializedValue<TMessage>(formatter, details)));
        }

        public void Goodbye<TMessage>(IWampFormatter<TMessage> formatter, long session, TMessage details, string reason)
        {
            RaiseSessionClosed(SessionCloseType.Goodbye, formatter, session, details, reason);
        }

        public void Abort<TMessage>(IWampFormatter<TMessage> formatter, long session, TMessage details, string reason)
        {
            RaiseSessionClosed(SessionCloseType.Abort, formatter, session, details, reason);
        }

        public void SessionLost(long sessionId)
        {
            RaiseSessionClosed(SessionCloseType.Disconnection, WampObjectFormatter.Value, sessionId, null, null);
        }

        private void RaiseSessionClosed<TMessage>(SessionCloseType sessionCloseType, IWampFormatter<TMessage> formatter, long session, TMessage details, string reason)
        {
            RaiseSessionClosed
                (new WampSessionCloseEventArgs(sessionCloseType, session,
                                               new SerializedValue<TMessage>(formatter, details), reason));
        }

        protected virtual void RaiseSessionCreated(WampSessionEventArgs e)
        {
            EventHandler<WampSessionEventArgs> handler = SessionCreated;
            if (handler != null) handler(this, e);
        }

        protected virtual void RaiseSessionClosed(WampSessionCloseEventArgs e)
        {
            EventHandler<WampSessionCloseEventArgs> handler = SessionClosed;
            if (handler != null) handler(this, e);
        }
    }
}