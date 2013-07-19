using WampSharp.Core.Listener;

namespace WampSharp.Auxiliary
{
    public interface IWampAuxiliaryClientFactory<TMessage>
    {
        IWampClientConnectionMonitor CreateMonitor(IWampConnection<TMessage> connection);
    }
}