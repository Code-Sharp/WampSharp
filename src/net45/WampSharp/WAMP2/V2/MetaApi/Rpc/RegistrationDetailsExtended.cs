using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.Serialization;

namespace WampSharp.V2.MetaApi
{
    internal class RegistrationDetailsExtended : RegistrationDetails, IGroupDetailsExtended
    {
        private ImmutableList<long> mCallees = ImmutableList<long>.Empty;
        private readonly object mLock = new object();

        [IgnoreDataMember]
        public IReadOnlyList<long> Callees => mCallees;

        public void AddCallee(long sessionId)
        {
            lock (mLock)
            {
                mCallees = mCallees.Add(sessionId);
            }
        }

        public void RemoveCallee(long sessionId)
        {
            lock (mLock)
            {
                mCallees = mCallees.Remove(sessionId);
            }
        }

        long IGroupDetailsExtended.GroupId => RegistrationId;

        IReadOnlyList<long> IGroupDetailsExtended.Peers => Callees;

        void IGroupDetailsExtended.AddPeer(long sessionId)
        {
            AddCallee(sessionId);
        }

        void IGroupDetailsExtended.RemovePeer(long sessionId)
        {
            RemoveCallee(sessionId);
        }
    }
}