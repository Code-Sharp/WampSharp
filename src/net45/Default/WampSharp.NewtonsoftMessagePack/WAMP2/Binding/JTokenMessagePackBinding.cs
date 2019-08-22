using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WampSharp.MessagePack;
using WampSharp.Newtonsoft;
using WampSharp.V2.Binding.Contracts;

namespace WampSharp.Binding
{
    /// <summary>
    /// Represents MsgPack binding implemented using Newtonsoft.Msgpack.
    /// </summary>
    public class JTokenMessagePackBinding : MsgPackBinding<JToken>
    {
        public JTokenMessagePackBinding() :
            this(new JsonSerializer())
        {
        }

        public JTokenMessagePackBinding(JsonSerializer serializer) :
            base(new JsonFormatter(serializer), new MessagePackParser(serializer))
        {
        }
    }
}