using MsgPack;
using Newtonsoft.Json.Linq;

namespace Json2Msgpack
{
    internal class MsgpackToJsonConvert
    {
        private static readonly JTokenToMsgPackProcessor mJTokenToMsgPackProcessor =
            new JTokenToMsgPackProcessor();

        private static readonly MsgPackToJTokenProcessor mMsgPackToJTokenProcessor = 
            new MsgPackToJTokenProcessor();

        public static MessagePackObject ToMessagePack(JToken value)
        {
            return mJTokenToMsgPackProcessor.Process(value);
        }

        public static JToken ToJson(MessagePackObject value)
        {
            return mMsgPackToJTokenProcessor.Process(value);
        }
    }
}