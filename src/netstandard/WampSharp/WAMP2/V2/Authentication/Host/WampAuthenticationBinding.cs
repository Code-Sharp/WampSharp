using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Serialization;
using WampSharp.V2.Binding;
using WampSharp.V2.Core;
using WampSharp.V2.Realm;

namespace WampSharp.V2.Authentication
{
    internal class WampAuthenticationBinding<TMessage> : IWampRouterBinding<TMessage>
    {
        private readonly IWampBinding<TMessage> mBinding;
        private readonly IWampSessionAuthenticatorFactory mSessionAuthenticationFactory;
        private readonly IWampUriValidator mUriValidator;

        public WampAuthenticationBinding(IWampBinding<TMessage> binding,
                                         IWampSessionAuthenticatorFactory sessionAuthenticationFactory, 
                                         IWampUriValidator uriValidator)
        {
            mBinding = binding;
            mUriValidator = uriValidator;

            mSessionAuthenticationFactory = 
                new RestrictedSessionAuthenticationFactory(sessionAuthenticationFactory);
        }

        public IWampBindingHost CreateHost(IWampHostedRealmContainer realmContainer,
                                           IWampConnectionListener<TMessage> connectionListener,
                                           IWampSessionMapper sessionIdMap)
        {
            IWampRouterBuilder wampRouterBuilder = new WampAuthenticationRouterBuilder(mSessionAuthenticationFactory, mUriValidator);

            return new WampBindingHost<TMessage>(realmContainer,
                                                 wampRouterBuilder,
                                                 connectionListener,
                                                 mBinding,
                                                 sessionIdMap);
        }

        public string Name => mBinding.Name;

        public WampMessage<object> GetRawMessage(WampMessage<object> message)
        {
            return mBinding.GetRawMessage(message);
        }

        public IWampFormatter<TMessage> Formatter => mBinding.Formatter;
    }
}