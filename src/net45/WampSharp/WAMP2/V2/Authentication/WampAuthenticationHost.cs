using System;
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

        public WampAuthenticationHost(IWampSessionAuthenticatorFactory sessionAuthenticationFactory,
                                      IWampRealmContainer realmContainer = null)
            : base(realmContainer)
        {
            if (sessionAuthenticationFactory == null)
            {
                throw new ArgumentNullException("sessionAuthenticationFactory");
            }

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
            (IWampTextBinding<TMessage> binding)
        {
            return new WampAuthenticationTextBinding<TMessage>(binding, mSessionAuthenticationFactory);
        }

        private IWampBinding CreateAuthenticationBinding<TMessage>
            (IWampBinaryBinding<TMessage> binding)
        {
            return new WampAuthenticationBinaryBinding<TMessage>(binding, mSessionAuthenticationFactory);
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

    internal abstract class WampAuthenticationBinding<TMessage> : IWampRouterBinding<TMessage>
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

    internal class WampAuthenticationBinaryBinding<TMessage> : WampAuthenticationBinding<TMessage>,
        IWampBinaryBinding<TMessage>
    {
        private readonly IWampBinaryBinding<TMessage> mBinding;

        public WampAuthenticationBinaryBinding(IWampBinaryBinding<TMessage> binding,
                                               IWampSessionAuthenticatorFactory sessionAuthenticationFactory)
            : base(binding, sessionAuthenticationFactory)
        {
            mBinding = binding;
        }

        public WampMessage<TMessage> Parse(byte[] raw)
        {
            return mBinding.Parse(raw);
        }

        public byte[] Format(WampMessage<object> message)
        {
            return mBinding.Format(message);
        }
    }

    internal class WampAuthenticationTextBinding<TMessage> : WampAuthenticationBinding<TMessage>,
        IWampTextBinding<TMessage>
    {
        private readonly IWampTextBinding<TMessage> mBinding;

        public WampAuthenticationTextBinding(IWampTextBinding<TMessage> binding,
                                             IWampSessionAuthenticatorFactory sessionAuthenticationFactory)
            : base(binding, sessionAuthenticationFactory)
        {
            mBinding = binding;
        }

        public WampMessage<TMessage> Parse(string raw)
        {
            return mBinding.Parse(raw);
        }

        public string Format(WampMessage<object> message)
        {
            return mBinding.Format(message);
        }
    }


    internal class WampAuthenticationRouterBuilder : WampRouterBuilder
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