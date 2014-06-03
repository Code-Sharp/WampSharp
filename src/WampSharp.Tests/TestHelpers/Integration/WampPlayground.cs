using WampSharp.Core.Listener;
using WampSharp.Core.Serialization;

namespace WampSharp.Tests.TestHelpers.Integration
{
    public class WampPlayground : WampPlayground<MockRaw>
    {
        public WampPlayground() : base(new MockRawFormatter())
        {
        }
    }

    public class WampPlayground<TMessage>
    {
        private readonly IWampHost mHost;
        private readonly MockConnectionListener<TMessage> mListener;

        private readonly IWampChannelFactory<TMessage> mChannelFactory;
        private readonly IWampFormatter<TMessage> mFormatter;

        public WampPlayground(IWampFormatter<TMessage> wampFormatter)
            : this(wampFormatter, new MockConnectionListener<TMessage>())
        {
        }

        protected WampPlayground(IWampFormatter<TMessage> formatter,
                                 MockConnectionListener<TMessage> listener)
            : this(formatter, listener, new WampHost<TMessage>(listener, formatter))
        {
        }

        protected WampPlayground(IWampFormatter<TMessage> formatter, 
            MockConnectionListener<TMessage> listener,
            IWampHost host)
        {
            mFormatter = formatter;
            mListener = listener;
            mHost = host;
            mChannelFactory = new WampChannelFactory<TMessage>(mFormatter);
        }

        public IControlledWampConnection<TMessage> CreateClientConnection()
        {
            return mListener.CreateClientConnection();
        }

        public IWampChannel<TMessage> CreateNewChannel()
        {
            return mChannelFactory.CreateChannel(CreateClientConnection());
        }

        public IWampChannelFactory<TMessage> ChannelFactory
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