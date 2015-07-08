using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm.Binded;

namespace WampSharp.V2.Core.Proxy
{
    internal class WampClientPropertyBag<TMessage> : IWampClientProperties, IWampClientProperties<TMessage>
    {
        private readonly IWampBinding<TMessage> mBinding;

        public WampClientPropertyBag(IWampBinding<TMessage> binding)
        {
            mBinding = binding;
        }

        public IWampSessionAuthenticator Authenticator { get; set; }

        public IWampAuthorizer Authorizer
        {
            get
            {
                return Authenticator.Authorizer;
            }
        }

        public ClientRoles Roles { get; set; }

        public bool GoodbyeSent { get; set; }

        public long Session { get; set; }

        IWampBinding IWampClientProperties.Binding
        {
            get
            {
                return this.Binding;
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