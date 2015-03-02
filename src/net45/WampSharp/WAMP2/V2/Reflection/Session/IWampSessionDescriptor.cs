using WampSharp.V2.Rpc;

namespace WampSharp.V2.Reflection
{
    public interface IWampSessionDescriptor
    {
        [WampProcedure("wamp.session.count")]
        long SessionCount();

        [WampProcedure("wamp.session.list")]
        long[] GetSessionIds();

        [WampProcedure("wamp.session.get")]
        WampSessionDetails GetSessionDetails(long sessionId);
    }
}