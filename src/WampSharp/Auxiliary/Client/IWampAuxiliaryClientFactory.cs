using WampSharp.Core.Listener;

namespace WampSharp.Auxiliary.Client
{
    public interface IWampAuxiliaryClientFactory<TMessage>
    {
        IWampClientConnectionMonitor CreateMonitor(IWampConnection<TMessage> connection);
    }
}