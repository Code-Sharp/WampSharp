using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Message;
using WampSharp.Tests.TestHelpers;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.Tests.Wampv2.Binding
{
    internal static class WampMessagePrinter
    {
        private interface IWampAll : IWampClient, IWampServerProxy
        {
        }

        private static readonly WampRequestMapper<MockRaw> mRequestMapper =
            new WampRequestMapper<MockRaw>(typeof(IWampAll), new MockRawFormatter());

        public static string ToString(WampMessage<MockRaw> message)
        {
            WampMethodInfo method = mRequestMapper.Map(message);
            
            var dictionary = 
                method.Parameters.Zip(message.Arguments,
                                      (parameter, argument) => new { parameter.Name, 
                                                                     Argument = Convert(argument) })
                      .ToDictionary(x => x.Name, x => x.Argument);

            JToken token = JToken.FromObject(dictionary);
            StringWriter stringWriter = new StringWriter();
            JsonTextWriter writer = new JsonTextWriter(stringWriter);
            writer.Formatting = Formatting.None;
            token.WriteTo(writer);
            return stringWriter.ToString();            
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
    }
}