using MsgPack;
using WampSharp.MsgPack;

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