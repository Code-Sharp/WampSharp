using MsgPack;
using MsgPack.Serialization;
using WampSharp.MsgPack;
using WampSharp.V2.Binding.Contracts;

namespace WampSharp.Binding
{
    public class MessagePackObjectBinding : MsgPackBinding<MessagePackObject>
    {
        public MessagePackObjectBinding() :
            base(new MessagePackFormatter(), new MessagePackParser())
        {
        }

        public MessagePackObjectBinding(SerializationContext serializationContext) :
            base(new MessagePackFormatter(serializationContext), new MessagePackParser(serializationContext))
        {
        }
    }
}