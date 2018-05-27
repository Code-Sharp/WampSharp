using WampSharp.Core.Listener;
using WampSharp.Core.Utilities;
using WampSharp.V1.Core.Contracts;

namespace WampSharp.V1.Core.Listener
{
    public class WampClientContainer<TMessage> : WampClientContainer<TMessage, IWampClient>
    {
        private readonly SessionIdMapper mMapper = new SessionIdMapper(); 

        public WampClientContainer(IWampClientBuilderFactory<TMessage, IWampClient> clientBuilderFactory) : 
            base(clientBuilderFactory)
        {
        }

        public override object GenerateClientId(IWampClient client)
        {
            return mMapper.Add(client);
        }

        public override void RemoveClient(IWampConnection<TMessage> connection)
        {

            bool clientFound = base.TryGetClient(connection, out IWampClient clientProxy);

            base.RemoveClient(connection);

            if (clientFound)
            {
                mMapper.TryRemove(clientProxy.SessionId, out clientProxy);
            }
        }

        private class SessionIdMapper : IdMapperBase<string, IWampClient>
        {
            private readonly WampSessionIdGenerator mSessionIdGenerator = new WampSessionIdGenerator();

            protected override string GenerateId()
            {
                return mSessionIdGenerator.Generate();
            }
        }
    }
}