using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;
using WampSharp.V2.Realm;

namespace WampSharp.V2
{
    public class WampSerializedOptionsDetailsExtensions
    {
        public static TDetails DeserializeDetails<TDetails>(IWampSerializedEvent wampSerializedEvent) where TDetails : EventDetails
        {
            return wampSerializedEvent.Details.Deserialize<TDetails>();
        }

        public static TOptions DeserializeOptions<TOptions>(WampSubscriptionAddEventArgs eventArgs) where TOptions : SubscribeOptions
        {
            return eventArgs.Options.Deserialize<TOptions>();
        }
    }
}