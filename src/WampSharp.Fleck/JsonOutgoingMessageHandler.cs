using System.IO;
using Fleck;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Contracts;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;

namespace WampSharp.Fleck
{
    public class JsonOutgoingMessageHandler : IWampOutgoingMessageHandler<JToken>
    {
        private readonly IWebSocketConnection mConnection;
        private readonly IWampMessageFormatter<JToken> mFormatter;

        public JsonOutgoingMessageHandler(IWebSocketConnection connection, IWampMessageFormatter<JToken> formatter)
        {
            mConnection = connection;
            mFormatter = formatter;
        }

        public void Handle(IWampClient client, WampMessage<JToken> message)
        {
            StringWriter stringWriter = new StringWriter();
            JsonWriter writer = new JsonTextWriter(stringWriter)
                                    {
                                        Formatting = Formatting.None
                                    };
            
            JToken raw = mFormatter.Format(message);

            raw.WriteTo(writer);

            mConnection.Send(stringWriter.ToString());
        }
    }
}