using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsgPack;
using Newtonsoft.Json.Linq;

namespace Json2Msgpack
{
    internal class MsgpackToJsonConvert
    {
        public static MessagePackObject ToMessagePack(JToken value)
        {
            JTokenToMsgPackProcessor processor = new JTokenToMsgPackProcessor();
            return processor.Process(value);
        }

        public static JToken ToJson(MessagePackObject value)
        {
            MsgPackToJTokenProcessor processor = new MsgPackToJTokenProcessor();
            return processor.Process(value);
        }
    }
}
