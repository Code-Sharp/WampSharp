using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Msgpack;
using WampSharp.Newtonsoft;

namespace WampSharp.Msgpack
{
    public class MessagePackParser : BinaryJTokenMessageParser
    {
        public MessagePackParser(JsonSerializer serializer) : base(serializer)
        {
        }

        protected override JsonReader GetReader(Stream stream)
        {
            return new MessagePackReader(stream);
        }


        protected override JsonWriter GetWriter(Stream stream)
        {
            return new MessagePackWriter(stream) {WriteDateTimeAsString = true};
        }
    }
}