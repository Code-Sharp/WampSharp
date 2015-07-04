using System.Collections.Generic;
using WampSharp.Core.Listener;
using WampSharp.Tests.TestHelpers;
using WampSharp.V1.Core.Contracts;

namespace WampSharp.Tests.Proxy.Helpers
{
    public class MockClientContainer : IWampClientContainer<MockRaw, IWampClient>
    {
        public IWampClient GetClient(IWampConnection<MockRaw> connection)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<IWampClient> GetAllClients()
        {
            throw new System.NotImplementedException();
        }

        public void RemoveClient(IWampConnection<MockRaw> connection)
        {
            throw new System.NotImplementedException();
        }

        public bool TryGetClient(IWampConnection<MockRaw> connection, out IWampClient client)
        {
            throw new System.NotImplementedException();
        }

        public object GenerateClientId(IWampClient client)
        {
            return "v59mbCGDXZ7WTyxB";
        }
    }
}