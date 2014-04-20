using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Message;
using WampSharp.Tests.TestHelpers;

namespace WampSharp.Tests.Wampv2.Binding
{
    public class MockWampMessage : WampMessage<MockRaw>
    {
        public MockWampMessage()
        {
        }

        public MockWampMessage(WampMessage<MockRaw> other)
            : base(other)
        {
        }

        private static JToken Convert(MockRaw raw)
        {
            MockRaw[] array = raw.Value as MockRaw[];

            if (array != null)
            {
                return new JArray(array.Select(x => Convert(x)));
            }
            else if (raw.Value == null)
            {
                return new JValue((object)null);
            }
            else
            {
                return JToken.FromObject(raw.Value);
            }
        }

        public override string ToString()
        {
            JToken token = new JArray(this.Arguments.Select(x => Convert(x)));
            StringWriter stringWriter = new StringWriter();
            JsonTextWriter writer = new JsonTextWriter(stringWriter);
            writer.Formatting = Formatting.None;
            token.WriteTo(writer);
            return stringWriter.ToString();
        }
    }
}