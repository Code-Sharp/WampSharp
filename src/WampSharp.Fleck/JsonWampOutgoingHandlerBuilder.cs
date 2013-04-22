using Fleck;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;

namespace WampSharp.Fleck
{
    public class JsonWampOutgoingHandlerBuilder : IWampOutgoingMessageHandlerBuilder<JToken, IWebSocketConnection>
    {
        private readonly IWampMessageFormatter<JToken> mFormatter;

        public JsonWampOutgoingHandlerBuilder(IWampMessageFormatter<JToken> formatter)
        {
            mFormatter = formatter;
        }

        public IWampOutgoingMessageHandler<JToken> Build(IWebSocketConnection connection)
        {
            return new JsonOutgoingMessageHandler(connection, mFormatter);
        }
    }
}