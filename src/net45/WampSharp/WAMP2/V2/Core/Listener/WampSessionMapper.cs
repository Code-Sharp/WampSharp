using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Core.Listener
{
    public class WampSessionMapper : IWampSessionMapper
    {
        private readonly WampIdMapper<IWampClientProxy> mSessionIdMap = new WampIdMapper<IWampClientProxy>();

        public long AllocateSession(IWampClientProxy proxy)
        {
            return mSessionIdMap.Add(proxy);
        }

        public void ReleaseSession(long session)
        {
            mSessionIdMap.TryRemove(session, out var removed);
        }
    }
}