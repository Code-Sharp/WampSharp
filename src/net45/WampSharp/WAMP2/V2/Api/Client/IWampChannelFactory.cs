using WampSharp.Core.Listener;
using WampSharp.V2.Binding;

namespace WampSharp.V2
{
    public interface IWampChannelFactory
    {
        IWampChannel CreateChannel<TMessage>
            (string realm,
             IControlledWampConnection<TMessage> connection,
             IWampBinding<TMessage> binding);
    }
}