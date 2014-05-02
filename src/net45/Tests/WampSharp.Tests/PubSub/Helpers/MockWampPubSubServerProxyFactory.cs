using System;
using WampSharp.Core.Listener;
using WampSharp.Tests.TestHelpers;
using WampSharp.V1.Core.Contracts;
using WampSharp.V1.PubSub.Client;

namespace WampSharp.Tests.PubSub.Helpers
{
    public class MockWampPubSubServerProxyFactory : IWampServerProxyFactory<MockRaw>
    {
        private readonly Func<IWampPubSubClient<MockRaw>, IWampServer> mFactory;

        public MockWampPubSubServerProxyFactory(Func<IWampPubSubClient<MockRaw>, IWampServer> factory)
        {
            mFactory = factory;
        }

        public IWampServer Create(IWampPubSubClient<MockRaw> client, IWampConnection<MockRaw> connection)
        {
            return mFactory(client);
        }
    }
}