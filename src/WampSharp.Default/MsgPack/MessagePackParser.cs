using System.Linq;
using MsgPack;
using MsgPack.Serialization;
using WampSharp.Core.Message;
using WampSharp.Core.Serialization;
using WampSharp.V2.Binding.Parsers;

namespace WampSharp.MsgPack
{
    public class MessagePackParser :
        IWampBinaryMessageParser<MessagePackObject>
    {
        private readonly MessagePackSerializer<MessagePackObject> mSerializer =
            MessagePackSerializer.Create<MessagePackObject>();

        public WampMessage<MessagePackObject> Parse(byte[] bytes)
        {
            MessagePackObject[] message = 
                mSerializer.UnpackSingleObject(bytes).AsList()
                           .ToArray();

            return new WampMessage<MessagePackObject>()
                       {
                           Arguments = message.Skip(1).ToArray(),
                           MessageType = (WampMessageType)message[0].AsInt32()
                       };
        }

        public byte[] Format(WampMessage<MessagePackObject> message)
        {
            MessagePackObject[] output = new MessagePackObject[message.Arguments.Length + 1];

            output[0] = (int) message.MessageType;
            message.Arguments.CopyTo(output, 1);

            return mSerializer.PackSingleObject(output);
        }
    }
}