using Newtonsoft.Json.Linq;
using WampSharp.Binding;
using WampSharp.Core.Listener;
using WampSharp.Tests.TestHelpers;
using WampSharp.Tests.TestHelpers.Integration;
using WampSharp.Tests.Wampv2.Binding;
using WampSharp.V2;
using WampSharp.V2.Binding;
using WampSharp.V2.Client;

namespace WampSharp.Tests.Wampv2.TestHelpers.Integration
{
    public class WampPlayground : WampPlayground<JToken>
    {
        public WampPlayground() : base(new JTokenBinding())
        {
        }

        protected WampPlayground(MockConnectionListener<JToken> listener) :
            base(new JTokenBinding(), listener)
        {
        }

        protected WampPlayground(MockConnectionListener<JToken> listener, IWampHost host) : 
            base(new JTokenBinding(), listener, host)
        {
        }
    }

    public class WampPlayground<TMessage>
    {
        private readonly IWampHost mHost;
        private readonly MockConnectionListener<TMessage> mListener;

        private readonly IWampChannelFactory mChannelFactory;
        private readonly IWampBinding<TMessage> mBinding;

        public WampPlayground(IWampBinding<TMessage> binding)
            : this(binding, new MockConnectionListener<TMessage>())
        {
        }

        protected WampPlayground(IWampBinding<TMessage> binding,
                                 MockConnectionListener<TMessage> listener)
            : this(binding, listener, GetWampHost(binding, listener))
        {
        }

        private static IWampHost GetWampHost(IWampBinding<TMessage> binding, MockConnectionListener<TMessage> listener)
        {
            WampHost host = new WampHost();
            host.RegisterTransport(new MockTransport<TMessage>(listener), binding);
            return host;
        }

        protected WampPlayground(IWampBinding<TMessage> binding, 
            MockConnectionListener<TMessage> listener,
            IWampHost host)
        {
            mBinding = binding;
            mListener = listener;
            mHost = host;
            mChannelFactory = new WampChannelFactory();;
        }

        public IControlledWampConnection<TMessage> CreateClientConnection()
        {
            return mListener.CreateClientConnection();
        }

        public IWampChannel CreateNewChannel(string realm)
        {
            return mChannelFactory.CreateChannel(realm,
                                                 CreateClientConnection(),
                                                 mBinding);
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
    }
}