using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Serialization;
using WampSharp.Newtonsoft;

namespace WampSharp.V1
{
    public class DefaultWampChannelFactory : WampChannelFactory<JToken>
    {
        public DefaultWampChannelFactory()
            : this(new JsonSerializer())
        {
        }

        public DefaultWampChannelFactory(JsonSerializer serializer)
            : this(new JsonFormatter(serializer))
        {
            Serializer = serializer;
        }

        private DefaultWampChannelFactory(IWampFormatter<JToken> formatter)
            : base(formatter)
        {
        }

        public JsonSerializer Serializer { get; }
    }
}