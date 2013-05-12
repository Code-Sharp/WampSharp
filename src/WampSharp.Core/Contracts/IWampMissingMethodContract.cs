using WampSharp.Core.Message;

namespace WampSharp.Core.Contracts
{
    public interface IWampMissingMethodContract<TMessage>
    {
        void Missing(WampMessage<TMessage> rawMessage);
    }

    public interface IWampMissingMethodContract<TMessage, TClient>
    {
        void Missing([WampProxyParameter] TClient client, WampMessage<TMessage> rawMessage);
    }
}