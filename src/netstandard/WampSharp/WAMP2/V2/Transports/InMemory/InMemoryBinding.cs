using WampSharp.Core.Message;
using WampSharp.Core.Serialization;
using WampSharp.V2.Binding;

namespace WampSharp.V2.Transports
{
    public class InMemoryBinding : WampBinding<object>
    {
        private readonly WampCompositeFormatter mCompositeFormatter;

        public InMemoryBinding() : this(new WampCompositeFormatter())
        {
        }

        private InMemoryBinding(WampCompositeFormatter formatter) :
            base("wampsharp-inmemory", formatter)
        {
            mCompositeFormatter = formatter;
        }

        public override WampMessage<object> GetRawMessage(WampMessage<object> message)
        {
            return message;
        }

        public void AddFormatter<TMessage>(IWampFormatter<TMessage> formatter)
        {
            mCompositeFormatter.AddFormatter(formatter);
        }
    }
}