using System.Collections.Generic;
using WampSharp.Core.Contracts;

namespace WampSharp.Core.Listener
{
    public interface IWampClientContainer<TMessage>
    {
        IWampClient GetClient(IWampConnection<TMessage> connection);

        IEnumerable<IWampClient> GetAllClients(); 

        void RemoveClient(IWampConnection<TMessage> connection);
    }
}