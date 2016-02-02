using System.Linq;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.MetaApi
{
    public interface IWampEventHistoryProvider
    {
        [WampProcedure("wamp.subscription.get_events")]
        WampHistoricalEvent[] GetEvents(long subscriptionId, long limit = 10);
    }
}