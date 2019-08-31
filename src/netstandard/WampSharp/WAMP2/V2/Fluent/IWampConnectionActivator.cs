using WampSharp.Core.Listener;
using WampSharp.V2.Binding;

namespace WampSharp.V2.Fluent
{
    public interface IWampConnectionActivator
    {
        IControlledWampConnection<TMessage> Activate<TMessage>(IWampBinding<TMessage> binding);
    }
}