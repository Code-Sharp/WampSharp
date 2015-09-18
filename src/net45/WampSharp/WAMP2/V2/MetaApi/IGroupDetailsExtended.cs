using System.Collections.Generic;

namespace WampSharp.V2.MetaApi
{
    internal interface IGroupDetailsExtended
    {
        long GroupId { get; }

        string Match { get; }

        IReadOnlyList<long> Peers { get; }

        void AddPeer(long sessionId);

        void RemovePeer(long sessionId);
    }
}