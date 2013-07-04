using System;
using WampSharp.Core.Contracts.V1;
using WampSharp.Core.Listener;
using WampSharp.PubSub.Client;
using WampSharp.Tests.TestHelpers;

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