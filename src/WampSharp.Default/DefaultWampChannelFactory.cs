using Newtonsoft.Json.Linq;
using WampSharp.Api;
using WampSharp.Core.Serialization;
using WampSharp.Fleck;

namespace WampSharp.Default
{
    public class DefaultWampChannelFactory : WampChannelFactory<JToken>
    {
        public DefaultWampChannelFactory()
            : this(new JsonFormatter())
        {
        }

        public DefaultWampChannelFactory(IWampFormatter<JToken> formatter)
            : base(formatter)
        {
        }
    }
}