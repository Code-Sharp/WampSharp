using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Serialization;
using WampSharp.Newtonsoft;

namespace WampSharp.V1
{
    public class DefaultWampChannelFactory : WampChannelFactory<JToken>
    {
        private readonly JsonSerializer mSerializer;

        public DefaultWampChannelFactory()
            : this(new JsonSerializer())
        {
        }

        public DefaultWampChannelFactory(JsonSerializer serializer)
            : this(new JsonFormatter(serializer))
        {
            mSerializer = serializer;
        }

        private DefaultWampChannelFactory(IWampFormatter<JToken> formatter)
            : base(formatter)
        {
        }

        public JsonSerializer Serializer
        {
            get
            {
                return mSerializer;
            }
        }
    }
}