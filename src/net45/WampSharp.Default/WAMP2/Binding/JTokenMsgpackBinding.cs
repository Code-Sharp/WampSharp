using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WampSharp.Msgpack;
using WampSharp.Newtonsoft;
using WampSharp.V2.Binding.Contracts;

namespace WampSharp.Binding
{
    /// <summary>
    /// Represents MsgPack binding implemented using Newtonsoft.Msgpack.
    /// </summary>
    public class JTokenMsgpackBinding : MsgPackBinding<JToken>
    {
        public JTokenMsgpackBinding() :
            base(new JsonFormatter(), new MessagePackParser())
        {
        }

        public JTokenMsgpackBinding(JsonSerializer serializer) :
            base(new JsonFormatter(serializer), new MessagePackParser())
        {
        }
    }
}