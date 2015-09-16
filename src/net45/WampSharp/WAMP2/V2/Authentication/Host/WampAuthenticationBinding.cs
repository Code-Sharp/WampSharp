using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Serialization;
using WampSharp.V2.Binding;
using WampSharp.V2.Realm;

namespace WampSharp.V2.Authentication
{
    internal abstract class WampAuthenticationBinding<TMessage> : IWampRouterBinding<TMessage>
    {
        private readonly IWampBinding<TMessage> mBinding;
        private readonly IWampSessionAuthenticatorFactory mSessionAuthenticationFactory;

        protected WampAuthenticationBinding(IWampBinding<TMessage> binding,
                                         IWampSessionAuthenticatorFactory sessionAuthenticationFactory)
        {
            mBinding = binding;

            mSessionAuthenticationFactory = 
                new RestrictedSessionAuthenticationFactory(sessionAuthenticationFactory);
        }

        public IWampBindingHost CreateHost(IWampHostedRealmContainer realmContainer, IWampConnectionListener<TMessage> connectionListener)
        {
            IWampRouterBuilder wampRouterBuilder = new WampAuthenticationRouterBuilder(mSessionAuthenticationFactory);

            return new WampBindingHost<TMessage>(realmContainer,
                                                 wampRouterBuilder,
                                                 connectionListener,
                                                 mBinding);
        }

        public string Name
        {
            get
            {
                return mBinding.Name;
            }
        }

        public WampMessage<object> GetRawMessage(WampMessage<object> message)
        {
            return mBinding.GetRawMessage(message);
        }

        public IWampFormatter<TMessage> Formatter
        {
            get
            {
                return mBinding.Formatter;
            }
        }
    }
}