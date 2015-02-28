using System;
using System.Threading.Tasks;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Client
{
    internal interface IWampSessionClientExtended<TMessage> :
        IWampSessionClient<TMessage>
    {
        // Maybe not such a good idea.
        long Session { get; }

        IWampRealmProxy Realm { get; }

        Task OpenTask { get; }

        void Close(string reason, object details);

        void OnConnectionOpen();
        void OnConnectionClosed();
        void OnConnectionError(Exception exception);
    }
}