using System.Collections.Generic;
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
            base(new JTokenBinding(), listener, new JTokenEqualityComparer())
        {
        }

        protected WampPlayground(MockConnectionListener<JToken> listener, IWampHost host) : 
            base(new JTokenBinding(), listener, host, new JTokenEqualityComparer())
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

        public WampPlayground(IWampBinding<TMessage> binding)
            : this(binding, new MockConnectionListener<TMessage>(),
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

        protected WampPlayground(IWampBinding<TMessage> binding, MockConnectionListener<TMessage> listener, IWampHost host, IEqualityComparer<TMessage> equalityComparer)
        {
            mBinding = binding;
            mListener = listener;
            mHost = host;
            mChannelFactory = new WampChannelFactory();;
            mEqualityComparer = equalityComparer;
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