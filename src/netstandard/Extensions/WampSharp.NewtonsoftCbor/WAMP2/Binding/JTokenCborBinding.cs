using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WampSharp.Cbor;
using WampSharp.Newtonsoft;
using WampSharp.V2.Binding.Contracts;

namespace WampSharp.Binding
{
    /// <summary>
    /// Represents Cbor binding implemented using Newtonsoft.Json.Cbor.
    /// </summary>
    public class JTokenCborBinding : CborBinding<JToken>
    {
        public JTokenCborBinding() :
            this(new JsonSerializer())
        {
        }

        public JTokenCborBinding(JsonSerializer serializer) :
            base(new JsonFormatter(serializer), new CborParser(serializer))
        {
        }
    }
}