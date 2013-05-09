using System.Collections.Generic;

namespace WampSharp.Core.Listener
{
    public interface IWampClientContainer<TMessage, TClient>
    {
        TClient GetClient(IWampConnection<TMessage> connection);

        IEnumerable<TClient> GetAllClients(); 

        void RemoveClient(IWampConnection<TMessage> connection);
    }
}