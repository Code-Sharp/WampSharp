using WampSharp.V2.Binding;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm;
using WampSharp.V2.Realm.Binded;
using WampSharp.V2.Reflection;

namespace WampSharp.V2.Core.Proxy
{
    internal class WampClientPropertyBag<TMessage> : IWampClientProperties, IWampClientProperties<TMessage>
    {
        private readonly IWampBinding<TMessage> mBinding;
        private readonly long mSession;
        private readonly WampTransportDetails mTransportDetails;

        public WampClientPropertyBag(long session, IWampBinding<TMessage> binding, WampTransportDetails transportDetails)
        {
            mBinding = binding;
            mTransportDetails = transportDetails;
            mSession = session;
        }

        public bool GoodbyeSent { get; set; }

        public long Session
        {
            get
            {
                return mSession;
            }
        }

        IWampBinding IWampClientProperties.Binding
        {
            get
            {
                return this.Binding;
            }
        }

        public WampTransportDetails TransportDetails
        {
            get
            {
                return mTransportDetails;
            }
        }

        public IWampBindedRealm<TMessage> Realm { get; set; }

        public IWampBinding<TMessage> Binding
        {
            get
            {
                return mBinding;
            }
        }
    }
}