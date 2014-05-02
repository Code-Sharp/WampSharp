using MsgPack;
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
    }
}