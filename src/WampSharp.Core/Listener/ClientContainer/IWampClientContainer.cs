using System.Collections.Generic;
using WampSharp.Core.Contracts;

namespace WampSharp.Core.Listener
{
    public interface IWampClientContainer<TConnection>
    {
        IWampClient GetClient(TConnection connection);

        IEnumerable<IWampClient> GetAllClients(); 

        void RemoveClient(TConnection connection);
    }
}