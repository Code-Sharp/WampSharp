#pragma warning disable 1591

using WampSharp.Core.Message;

namespace WampSharp.Core.Contracts.V2
{
    public interface IWampSubscriber : IWampSubscriber<object>
    {
    }

    public interface IWampSubscriber<TMessage>
    {
        [WampHandler(WampMessageType.v2Event)]
        void Event(string topic);

        [WampHandler(WampMessageType.v2Event)]
        void Event(string topic, TMessage @event);

        [WampHandler(WampMessageType.v2Event)]
        void Event(string topic, TMessage @event, TMessage eventDetails);

        [WampHandler(WampMessageType.v2Metaevent)]
        void Metaevent(string topic, string metaTopic);

        [WampHandler(WampMessageType.v2Metaevent)]
        void Metaevent(string topic, string metaTopic, TMessage metaEvent);
    }
}

#pragma warning restore 1591