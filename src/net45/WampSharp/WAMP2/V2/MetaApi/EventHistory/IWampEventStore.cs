using WampSharp.Core.Serialization;

namespace WampSharp.V2.MetaApi
{
    public interface IWampEventStore
    {
        void StoreEvent<TMessage>(IWampFormatter<TMessage> formatter,
                                  HistoricalSubscription subscription,
                                  WampHistoricalEvent<TMessage> historicalEvent);

        WampHistoricalEvent<object>[] GetEvents(HistoricalSubscription subscription, long limit);
    }
}