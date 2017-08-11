using WampSharp.V2;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm;

namespace WampSharp.Tests.Wampv2.TestHelpers.Integration
{
    internal class ChannelWithExtraData
    {
        public long SessionId { get; }
        public WelcomeDetails Details { get; }
        public IWampChannel Channel { get; }

        public ChannelWithExtraData(IWampChannel channel, WampSessionCreatedEventArgs eventArgs)
        {
            Channel = channel;
            SessionId = eventArgs.SessionId;
            Details = eventArgs.WelcomeDetails;
        }
    }
}