using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Serialization;
using WampSharp.V2.Realm;

namespace WampSharp.V2.Core.Listener
{
    public abstract class WampBinding<TMessage> : IWampBinding<TMessage>
    {
        private readonly string mName;
        private readonly IWampFormatter<TMessage> mFormatter;

        public WampBinding(string name, IWampFormatter<TMessage> formatter)
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

        public abstract WampMessage<TMessage> GetRawMessage(WampMessage<TMessage> message);

        public virtual IWampBindingHost CreateHost(IWampRealmContainer realmContainer, IWampConnectionListener<TMessage> connectionListener)
        {
            WampBindingHost<TMessage> result =
                new WampBindingHost<TMessage>(realmContainer, connectionListener, this);

            return result;
        }
    }
}