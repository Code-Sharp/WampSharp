using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Message;
using WampSharp.Tests.TestHelpers;

namespace WampSharp.Tests.Wampv2
{
    internal static class WampMessagePrinter
    {
        private static readonly RequestMapper mRequestMapper = new RequestMapper();

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

            if (raw.Value is MockRaw[] array)
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