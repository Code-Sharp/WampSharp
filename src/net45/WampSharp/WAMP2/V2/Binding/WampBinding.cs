using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Serialization;
using WampSharp.V2.Realm;

namespace WampSharp.V2.Binding
{
    /// <summary>
    /// Represents a base class for <see cref="IWampBinding{TMessage}"/>.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public abstract class WampBinding<TMessage> : IWampBinding<TMessage>
    {
        private readonly string mName;
        private readonly IWampFormatter<TMessage> mFormatter;

        protected WampBinding(string name, IWampFormatter<TMessage> formatter)
        {
            mName = name;
            mFormatter = formatter;
        }

        public string Name
        {
            get
            {
                return mName;
            }
        }

        public IWampFormatter<TMessage> Formatter
        {
            get
            {
                return mFormatter;
            }
        }

        public abstract WampMessage<object> GetRawMessage(WampMessage<object> message);

#if !PCL
        public virtual IWampBindingHost CreateHost(IWampHostedRealmContainer realmContainer, IWampConnectionListener<TMessage> connectionListener)
        {
            WampBindingHost<TMessage> result =
                new WampBindingHost<TMessage>(realmContainer, connectionListener, this);

            return result;
        }
#endif
    }
}