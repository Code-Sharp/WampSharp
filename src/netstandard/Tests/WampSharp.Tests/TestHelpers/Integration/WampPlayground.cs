using WampSharp.Core.Listener;
using WampSharp.V1;
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
        private readonly MockConnectionListener<TMessage> mListener;
        private readonly IWampFormatter<TMessage> mFormatter;

        public WampPlayground(IWampFormatter<TMessage> wampFormatter)
            : this(wampFormatter, new MockConnectionListener<TMessage>(wampFormatter))
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
            Host = host;
            ChannelFactory = new WampChannelFactory<TMessage>(mFormatter);
        }

        public IControlledWampConnection<TMessage> CreateClientConnection()
        {
            return mListener.CreateClientConnection();
        }

        public IWampChannel<TMessage> CreateNewChannel()
        {
            return ChannelFactory.CreateChannel(CreateClientConnection());
        }

        public IWampChannelFactory<TMessage> ChannelFactory { get; }

        public IWampHost Host { get; }
    }
}