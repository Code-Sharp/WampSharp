using WampSharp.Core.Message;
using WampSharp.Core.Serialization;

namespace WampSharp.V2.Binding
{
    /// <summary>
    /// Represents a base class for <see cref="IWampBinding{TMessage}"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public abstract class WampBinding<TMessage> : IWampBinding<TMessage>
    {
        private readonly IWampFormatter<TMessage> mFormatter;

        protected WampBinding(string name, IWampFormatter<TMessage> formatter)
        {
            Name = name;
            mFormatter = formatter;
        }

        public string Name { get; }

        public IWampFormatter<TMessage> Formatter => mFormatter;

        public abstract WampMessage<object> GetRawMessage(WampMessage<object> message);
    }
}