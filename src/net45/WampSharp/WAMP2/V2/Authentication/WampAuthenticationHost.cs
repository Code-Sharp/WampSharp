using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Serialization;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;
using WampSharp.V2.Binding.Transports;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm;
using WampSharp.V2.Session;

namespace WampSharp.V2
{
    public class WampAuthenticationHost : WampHost
    {
        private readonly IWampSessionAuthenticatorFactory mSessionAuthenticationFactory;

        public WampAuthenticationHost(IWampSessionAuthenticatorFactory sessionAuthenticationFactory)
        {
            mSessionAuthenticationFactory = sessionAuthenticationFactory;
        }

        public override void RegisterTransport(IWampTransport transport, IEnumerable<IWampBinding> bindings)
        {
            IEnumerable<IWampBinding> authenticationBindings =
                bindings.Select(binding => CreateAuthenticationBinding((dynamic) binding))
                        .Cast<IWampBinding>()
                        .Where(x => x != null);

            base.RegisterTransport(transport, authenticationBindings);
        }

        private IWampBinding CreateAuthenticationBinding<TMessage>
            (IWampBinding<TMessage> binding)
        {
            return new WampAuthenticationBinding<TMessage>(binding, mSessionAuthenticationFactory);
        }

        /// <summary>
        /// Fallback in case that binding doesn't implement
        /// IWampBinding{TMessage}
        /// </summary>
        /// <param name="binding"></param>
        /// <returns></returns>
        private IWampBinding CreateAuthenticationBinding(IWampBinding binding)
        {
            return null;
        }
    }

    public class WampAuthenticationBinding<TMessage> : IWampRouterBinding<TMessage>
    {
        private readonly IWampBinding<TMessage> mBinding;
        private readonly IWampSessionAuthenticatorFactory mSessionAuthenticationFactory;

        public WampAuthenticationBinding(IWampBinding<TMessage> binding,
                                         IWampSessionAuthenticatorFactory sessionAuthenticationFactory)
        {
            mBinding = binding;

            // TODO: Wrap the SessionAuthenticationFactory so that
            // TODO: InternalAuthenticator is never modified
            // TODO: and so that other authorizers can't use Reserved URIs
            // TODO: see issue #84
            mSessionAuthenticationFactory = sessionAuthenticationFactory;
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

    public class WampAuthenticationRouterBuilder : WampRouterBuilder
    {
        private readonly IWampSessionAuthenticatorFactory mSessionAuthenticationFactory;

        public WampAuthenticationRouterBuilder(IWampSessionAuthenticatorFactory sessionAuthenticationFactory)
        {
            mSessionAuthenticationFactory = sessionAuthenticationFactory;
        }

        public override IWampSessionServer<TMessage> CreateSessionHandler<TMessage>
            (IWampHostedRealmContainer realmContainer,
             IWampBinding<TMessage> binding,
             IWampEventSerializer eventSerializer)
        {
            return new WampAuthenticationSessionServer<TMessage>
                (binding,
                 realmContainer,
                 this,
                 eventSerializer,
                 mSessionAuthenticationFactory);
        }

        public override IWampServer<TMessage> CreateServer<TMessage>(IWampSessionServer<TMessage> session, IWampDealer<TMessage> dealer, IWampBroker<TMessage> broker)
        {
            return new WampAuthenticationServer<TMessage>(session, dealer, broker);
        }
    }
}