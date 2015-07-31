using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WampSharp.Newtonsoft;
using WampSharp.V2.Binding.Contracts;

namespace WampSharp.Binding
{
    /// <summary>
    /// Represents JSON binding implemented using Newtonsoft.Json.
    /// </summary>
    public class JTokenJsonBinding : JsonBinding<JToken>
    {
        public JTokenJsonBinding() : this(new JsonSerializer())
        {
        }

        public JTokenJsonBinding(JsonSerializer serializer) :
            base(new JsonFormatter(serializer), new JTokenMessageParser(serializer))
        {
        }
    }
}