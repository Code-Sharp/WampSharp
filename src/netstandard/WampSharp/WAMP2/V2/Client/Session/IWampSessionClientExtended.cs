using System;
using System.Threading.Tasks;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Client
{
    internal interface IWampSessionClientExtended :
        IWampSessionClient
    {
        // Maybe not such a good idea.
        long Session { get; }

        IWampRealmProxy Realm { get; }

        Task OpenTask { get; }

        Task<GoodbyeMessage> Close(string reason, GoodbyeDetails details);

        void OnConnectionOpen();
        void OnConnectionClosed();
        void OnConnectionError(Exception exception);
    }
}