using MsgPack;
using MsgPack.Serialization;
using Newtonsoft.Json.Linq;

namespace Json2Msgpack
{
    internal class JTokenMessagePackSerializer<T> : MessagePackSerializer<T>
        where T : JToken
    {
        protected override void PackToCore(Packer packer, T objectTree)
        {
            MessagePackObject message =
                MsgpackToJsonConvert.ToMessagePack(objectTree);

            message.PackToMessage(packer, new PackingOptions());
        }

        protected override T UnpackFromCore(Unpacker unpacker)
        {
            MessagePackObject message = unpacker.LastReadData;

            T result =
                MsgpackToJsonConvert.ToJson(message) as T;

            return result;
        }
    }
}