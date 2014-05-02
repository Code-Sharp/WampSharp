using Newtonsoft.Json.Linq;
using WampSharp.Newtonsoft;
using WampSharp.V2.Binding.Contracts;

namespace WampSharp.Binding
{
    public class JTokenBinding : JsonBinding<JToken>
    {
        public JTokenBinding() : base(new JsonFormatter(), new JTokenMessageParser())
        {
        }
    }
}