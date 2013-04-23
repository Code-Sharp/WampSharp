using Newtonsoft.Json.Linq;
using WampSharp.Core.Contracts;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;

namespace WampSharp.Fleck
{
    public class JsonOutgoingMessageHandler : IWampOutgoingMessageHandler<JToken>
    {
        private readonly IWampConnection<JToken> mConnection;

        public JsonOutgoingMessageHandler(IWampConnection<JToken> connection, IWampMessageFormatter<JToken> formatter)
        {
            mConnection = connection;
        }

        public void Handle(IWampClient client, WampMessage<JToken> message)
        {
            mConnection.OnNext(message);
        }
    }
}