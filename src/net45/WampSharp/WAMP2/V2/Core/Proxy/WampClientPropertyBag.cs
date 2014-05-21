using WampSharp.V2.Binding;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm;

namespace WampSharp.V2.Core.Proxy
{
    internal class WampClientPropertyBag<TMessage> : IWampClientProperties, IWampClientProperties<TMessage>
    {
        private readonly IWampBinding<TMessage> mBinding;
        private readonly long mSession;

        public WampClientPropertyBag(long session, IWampBinding<TMessage> binding)
        {
            mBinding = binding;
            mSession = session;
        }

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

        public IWampRealm<TMessage> Realm
        {
            get; 
            set;
        }

        public IWampBinding<TMessage> Binding
        {
            get
            {
                return mBinding;
            }
        }
    }
}