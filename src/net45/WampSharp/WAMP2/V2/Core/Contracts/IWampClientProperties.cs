using WampSharp.V2.Binding;
using WampSharp.V2.Realm;

namespace WampSharp.V2.Core.Contracts
{
    public interface IWampClientProperties
    {
        long Session { get; }
        // TODO: Maybe get rid of the binding property, nobody needs it
        IWampBinding Binding { get; }
    }

    public interface IWampClientProperties<TMessage>
    {
        long Session { get; }
        IWampRealm<TMessage> Realm { get; set; }
        // TODO: Maybe get rid of the binding property, nobody needs it
        new IWampBinding<TMessage> Binding { get; }
    }
}