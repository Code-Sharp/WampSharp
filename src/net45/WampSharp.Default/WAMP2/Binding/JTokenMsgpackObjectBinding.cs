using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WampSharp.Msgpack;
using WampSharp.Newtonsoft;
using WampSharp.V2.Binding.Contracts;

namespace WampSharp.Binding
{
    public class JTokenMsgpackObjectBinding : MsgPackBinding<JToken>
    {
        public JTokenMsgpackObjectBinding() :
            base(new JsonFormatter(), new MessagePackParser())
        {
        }

        public JTokenMsgpackObjectBinding(JsonSerializer serializer) :
            base(new JsonFormatter(serializer), new MessagePackParser())
        {
        }
    }
}