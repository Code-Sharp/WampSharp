using WampSharp.Core.Message;
using WampSharp.V2.Binding;
using WampSharp.V2.Core;

namespace WampSharp.V2.Transports
{
    public class InMemoryBinding : WampBinding<object>
    {
        public InMemoryBinding() : base("wampsharp-inmemory", WampObjectFormatter.Value)
        {
        }

        public override WampMessage<object> GetRawMessage(WampMessage<object> message)
        {
            return message;
        }
    }
}