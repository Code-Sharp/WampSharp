using System.Threading.Tasks;
using WampSharp.Tests.Wampv2.Integration;
using WampSharp.V2.Realm;

namespace WampSharp.Tests.Wampv2.TestHelpers.Integration
{
    internal static class WampPlaygroundRoleExtensions
    {
        public static async Task<CallerCallee> GetCallerCalleeDualChannel(this WampPlayground playground)
        {
            const string realmName = "realm1";

            playground.Host.Open();

            CallerCallee result = new CallerCallee();

            result.CalleeChannel =
                playground.CreateNewChannel(realmName);

            result.CallerChannel =
                playground.CreateNewChannel(realmName);

            long? callerSessionId = null;

            result.CallerChannel.RealmProxy.Monitor.ConnectionEstablished +=
                (x, y) => { callerSessionId = y.SessionId; };

            await result.CalleeChannel.Open();
            await result.CallerChannel.Open();

            result.CallerSessionId = callerSessionId.Value;

            return result;
        }

        public static async Task<PublisherSubscriber> GetPublisherSubscriberDualChannel(this WampPlayground playground)
        {
            const string realmName = "realm1";

            playground.Host.Open();

            PublisherSubscriber result = new PublisherSubscriber();

            result.Publisher =
                playground.CreateNewChannel(realmName);

            result.Subscriber =
                playground.CreateNewChannel(realmName);

            long? publisherSessionId = null;

            result.Publisher.RealmProxy.Monitor.ConnectionEstablished +=
                (x, y) => { publisherSessionId = y.SessionId; };

            await result.Publisher.Open();
            await result.Subscriber.Open();

            result.PublisherSessionId = publisherSessionId.Value;

            return result;
        }

        public static async Task<ChannelWithExtraData> GetChannel(this WampPlayground playground)
        {
            const string realmName = "realm1";

            var channel =
                playground.CreateNewChannel(realmName);

            WampSessionCreatedEventArgs eventArgs = null;

            channel.RealmProxy.Monitor.ConnectionEstablished +=
                (x, y) => { eventArgs = y; };

            await channel.Open();

            ChannelWithExtraData result = 
                new ChannelWithExtraData(channel, eventArgs);

            return result;
        }
    }
}