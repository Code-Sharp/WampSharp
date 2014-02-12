using WampSharp.Core.Listener;

namespace WampSharp.V1.Auxiliary.Client
{
    public interface IWampAuxiliaryClientFactory<TMessage>
    {
        IWampClientConnectionMonitor CreateMonitor(IWampConnection<TMessage> connection);
    }
}