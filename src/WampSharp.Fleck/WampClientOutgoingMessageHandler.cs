using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;

namespace WampSharp.Fleck
{
    public class WampClientOutgoingMessageHandler<TMessage> : IWampOutgoingMessageHandler<TMessage>
    {
        private readonly IWampConnection<TMessage> mConnection;

        public WampClientOutgoingMessageHandler(IWampConnection<TMessage> connection)
        {
            mConnection = connection;
        }

        public void Handle(WampMessage<TMessage> message)
        {
            mConnection.OnNext(message);
        }
    }
}