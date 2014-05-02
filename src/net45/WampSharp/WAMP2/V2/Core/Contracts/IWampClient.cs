using WampSharp.V2.Binding;
using WampSharp.V2.Core.Listener;
using WampSharp.V2.Realm;

namespace WampSharp.V2.Core.Contracts
{
    public interface IWampClient :
        IWampSessionClient,
        IWampCallee,
        IWampCaller,
        IWampPublisher,
        IWampSubscriber
    {
        long Session { get; }

        IWampBinding Binding { get; }
    }

    public interface IWampClient<TMessage> :
        IWampCallee<TMessage>,
        IWampCaller<TMessage>,
        IWampPublisher<TMessage>,
        IWampSubscriber<TMessage>,
        IWampRawClient<TMessage>
    {
        // Maybe not such a good idea.
        long Session { get; }

        IWampRealm<TMessage> Realm { get; set; } 
        
        IWampBinding<TMessage> Binding { get; }
    }
}