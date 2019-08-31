using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.Core
{
    public interface IWampSessionMapper
    {
        long AllocateSession(IWampClientProxy proxy);

        void ReleaseSession(long session);
    }
}