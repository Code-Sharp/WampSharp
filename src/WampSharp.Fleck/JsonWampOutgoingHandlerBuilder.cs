using Fleck;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;

namespace WampSharp.Fleck
{
    public class JsonWampOutgoingHandlerBuilder<TMessage> : IWampOutgoingMessageHandlerBuilder<TMessage>
    {
        public IWampOutgoingMessageHandler<TMessage> Build(IWampConnection<TMessage> connection)
        {
            return new WampClientOutgoingMessageHandler<TMessage>(connection);
        }
    }
}