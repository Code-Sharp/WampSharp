using System.Threading.Tasks;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Client
{
    public interface IWampSessionClientExtended<TMessage> :
        IWampSessionClient<TMessage>
    {
        // Maybe not such a good idea.
        long Session { get; }

        IWampRealmProxy Realm { get; }

        Task OpenTask { get; }

        void OnConnectionOpen();
        void OnConnectionClosed();
    }
}