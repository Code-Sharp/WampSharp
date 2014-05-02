using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Serialization;
using WampSharp.V2.Realm;

namespace WampSharp.V2.Binding
{
    public interface IWampBinding<TMessage> : IWampBinding
    {
        WampMessage<TMessage> GetRawMessage(WampMessage<TMessage> message);
        
        IWampFormatter<TMessage> Formatter { get; }

        IWampBindingHost CreateHost(IWampRealmContainer realmContainer,
                                    IWampConnectionListener<TMessage> connectionListener);
    }

    public interface IWampBinding
    {
        string Name { get; }
    }
}