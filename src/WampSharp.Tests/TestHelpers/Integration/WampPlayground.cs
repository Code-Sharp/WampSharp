﻿using WampSharp.Core.Listener;

namespace WampSharp.Tests.TestHelpers.Integration
{
    public class WampPlayground
    {
        private readonly IWampHost mHost;
        private readonly MockConnectionListener mListener;

        private readonly IWampChannelFactory<MockRaw> mChannelFactory;
        private readonly MockRawFormatter mFormatter;

        public WampPlayground()
        {
            mFormatter = new MockRawFormatter();
            mChannelFactory = new WampChannelFactory<MockRaw>(mFormatter);
            mListener = new MockConnectionListener();
            mHost = new WampHost<MockRaw>(new DefaultWampHostBuilder<MockRaw>(), mListener, mFormatter);
        }

        public IControlledWampConnection<MockRaw> CreateClientConnection()
        {
            return mListener.CreateClientConnection();
        }

        public IWampChannel<MockRaw> CreateNewChannel()
        {
            return mChannelFactory.CreateChannel(CreateClientConnection());
        }

        public IWampChannelFactory<MockRaw> ChannelFactory
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