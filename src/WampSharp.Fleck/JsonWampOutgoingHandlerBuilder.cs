using Fleck;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;

namespace WampSharp.Fleck
{
    public class JsonWampOutgoingHandlerBuilder : IWampOutgoingMessageHandlerBuilder<JToken>
    {
        private readonly IWampMessageFormatter<JToken> mFormatter;

        public JsonWampOutgoingHandlerBuilder(IWampMessageFormatter<JToken> formatter)
        {
            mFormatter = formatter;
        }

        public IWampOutgoingMessageHandler<JToken> Build(IWampConnection<JToken> connection)
        {
            return new JsonOutgoingMessageHandler(connection, mFormatter);
        }
    }
}