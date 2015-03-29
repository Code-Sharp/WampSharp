#if !PCL
using WampSharp.V2.Binding;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.Realm.Binded
{
    internal class WampBindedRealm<TMessage> : IWampBindedRealm<TMessage>
    {
        private readonly IWampServer<TMessage> mServer;
        private readonly IWampHostedRealm mRealm;
        private readonly IWampBinding<TMessage> mBinding;
        private readonly IWampRealmGate mRealmGate;

        public WampBindedRealm(IWampHostedRealm realm,
                               IWampSessionServer<TMessage> session,
                               IWampEventSerializer eventSerializer,
                               IWampBinding<TMessage> binding)
        {
            mRealm = realm;
            mRealmGate = realm as IWampRealmGate;
            mBinding = binding;

            IWampRpcServer<TMessage> dealer =
                new WampRpcServer<TMessage>(realm.RpcCatalog, binding);

            IWampPubSubServer<TMessage> broker =
                new WampPubSubServer<TMessage>(realm.TopicContainer,
                                               eventSerializer,
                                               binding);

            mServer = new WampServer<TMessage>(session, dealer, broker);
        }

        public IWampServer<TMessage> Server
        {
            get
            {
                return mServer;
            }
        }

        public void Hello(long session, TMessage details)
        {
            mRealmGate.Hello(mBinding.Formatter, session, details);
        }

        public void Abort(long session, TMessage details, string reason)
        {
            mRealmGate.Abort(mBinding.Formatter, session, details, reason);
        }

        public void Goodbye(long session, TMessage details, string reason)
        {
            mRealmGate.Goodbye(mBinding.Formatter, session, details, reason);
        }

        public void SessionLost(long sessionId)
        {
            mRealmGate.SessionLost(sessionId);
        }

        public string Name
        {
            get
            {
                return mRealm.Name;
            }
        }
    }
}
#endif