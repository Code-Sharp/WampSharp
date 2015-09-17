using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Serialization;
using WampSharp.V2.Binding;
using WampSharp.V2.Core;
using WampSharp.V2.Realm;

namespace WampSharp.V2.Authentication
{
    internal abstract class WampAuthenticationBinding<TMessage> : IWampRouterBinding<TMessage>
    {
        private readonly IWampBinding<TMessage> mBinding;
        private readonly IWampSessionAuthenticatorFactory mSessionAuthenticationFactory;
        private readonly IWampUriValidator mUriValidator;

        protected WampAuthenticationBinding(IWampBinding<TMessage> binding,
                                         IWampSessionAuthenticatorFactory sessionAuthenticationFactory, 
                                         IWampUriValidator uriValidator)
        {
            mBinding = binding;
            mUriValidator = uriValidator;

            mSessionAuthenticationFactory = 
                new RestrictedSessionAuthenticationFactory(sessionAuthenticationFactory);
        }

        public IWampBindingHost CreateHost(IWampHostedRealmContainer realmContainer, IWampConnectionListener<TMessage> connectionListener)
        {
            IWampRouterBuilder wampRouterBuilder = new WampAuthenticationRouterBuilder(mSessionAuthenticationFactory, mUriValidator);

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