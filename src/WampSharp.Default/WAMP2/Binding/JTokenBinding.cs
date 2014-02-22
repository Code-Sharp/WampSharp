using Newtonsoft.Json.Linq;
using WampSharp.Newtonsoft;

namespace WampSharp.Binding
{
    public class JTokenBinding : JsonBinding<JToken>
    {
        public JTokenBinding() : base(new JsonFormatter(), new JTokenMessageParser())
        {
        }
    }
}