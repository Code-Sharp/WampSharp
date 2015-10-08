using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using WampSharp.Binding;
using WampSharp.Core.Client;
using WampSharp.Core.Listener;
using WampSharp.Core.Proxy;
using WampSharp.Core.Serialization;
using WampSharp.Tests.TestHelpers.Integration;
using WampSharp.V2;
using WampSharp.V2.Binding;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.Tests.Wampv2.TestHelpers.Integration
{
    public class WampPlayground : WampPlayground<JToken>
    {
        public WampPlayground() : base(new JTokenJsonBinding())
        {
        }
    }

    public class WampPlayground<TMessage>
    {
        private readonly IEqualityComparer<TMessage> mEqualityComparer;
        private readonly IWampHost mHost;
        private readonly MockConnectionListener<TMessage> mListener;

        private readonly IWampChannelFactory mChannelFactory;
        private readonly IWampBinding<TMessage> mBinding;
        private readonly WampServerProxyBuilder<TMessage, IWampClient<TMessage>, IWampServerProxy> mProxyFactory;

        public WampPlayground(IWampBinding<TMessage> binding)
            : this(binding, new MockConnectionListener<TMessage>(binding.Formatter),
                   EqualityComparer<TMessage>.Default)
        {
        }

        protected WampPlayground(IWampBinding<TMessage> binding,
                                 MockConnectionListener<TMessage> listener,
                                 IEqualityComparer<TMessage> equalityComparer)
            : this(binding, listener, GetWampHost(binding, listener), equalityComparer)
        {
        }

        private static IWampHost GetWampHost(IWampBinding<TMessage> binding, MockConnectionListener<TMessage> listener)
        {
            WampHost host = new WampHost();
            host.RegisterTransport(new MockTransport<TMessage>(listener), binding);
            return host;
        }

        protected WampPlayground(IWampBinding<TMessage> binding, MockConnectionListener<TMessage> listener,
                                 IWampHost host, IEqualityComparer<TMessage> equalityComparer)
        {
            mBinding = binding;
            mListener = listener;
            mHost = host;
            mChannelFactory = new WampChannelFactory();
            mEqualityComparer = equalityComparer;

            IWampFormatter<TMessage> formatter = binding.Formatter;

            mProxyFactory =
                new WampServerProxyBuilder<TMessage, IWampClient<TMessage>, IWampServerProxy>(
                    new WampOutgoingRequestSerializer<TMessage>(formatter),
                    new WampServerProxyOutgoingMessageHandlerBuilder<TMessage, IWampClient<TMessage>>
                        (new WampServerProxyIncomingMessageHandlerBuilder<TMessage, IWampClient<TMessage>>(formatter)));
        }

        public IControlledWampConnection<TMessage> CreateClientConnection()
        {
            return mListener.CreateClientConnection();
        }

        public IWampChannel CreateNewChannel(string realm)
        {
            return mChannelFactory.CreateChannel(realm,
                                                 CreateClientConnection(),
                                                 Binding);
        }

        public IWampChannel CreateNewChannel(string realm,
                                             IWampClientAuthenticator authenticator)
        {
            return mChannelFactory.CreateChannel(realm,
                                                 CreateClientConnection(),
                                                 Binding,
                                                 authenticator);
        }

        public IWampServerProxy CreateRawConnection(IWampClient<TMessage> client)
        {
            IControlledWampConnection<TMessage> connection = CreateClientConnection();
            connection.Connect();
            return mProxyFactory.Create(client, connection);
        }

        public IWampChannelFactory ChannelFactory
        {
            get
            {
                return mChannelFactory;
            }
        }

        public IWampHost Host
        {
            get
            {
                return mHost;
            }
        }

        public IWampBinding<TMessage> Binding
        {
            get
            {
                return mBinding;
            }
        }

        public IEqualityComparer<TMessage> EqualityComparer
        {
            get { return mEqualityComparer; }
        }
    }
}