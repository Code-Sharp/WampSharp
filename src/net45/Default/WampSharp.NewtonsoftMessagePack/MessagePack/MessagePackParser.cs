using System.IO;
using Newtonsoft.Json;
using Newtonsoft.MessagePack;
using WampSharp.Newtonsoft;

namespace WampSharp.MessagePack
{
    public sealed class MessagePackParser : BinaryJTokenMessageParser
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
            return new MessagePackWriter(stream);
        }
    }
}