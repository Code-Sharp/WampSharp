using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Cbor;
using WampSharp.Newtonsoft;

namespace WampSharp.Cbor
{
    public sealed class CborParser : BinaryJTokenMessageParser
    {
        public CborParser(JsonSerializer serializer) : base(serializer)
        {
        }

        protected override JsonReader GetReader(Stream stream)
        {
            return new CborDataReader(stream);
        }


        protected override JsonWriter GetWriter(Stream stream)
        {
            return new CborDataWriter(stream);
        }
    }
}