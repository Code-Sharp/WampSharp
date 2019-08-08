using System;
using System.Threading.Tasks;
using NUnit.Framework;
using WampSharp.Tests.Wampv2.TestHelpers.Integration;
using WampSharp.V2.Management;
using WampSharp.V2.Realm;
using WampSharp.V2.MetaApi;

namespace WampSharp.Tests.Wampv2.Integration
{
    [TestFixture]
    public class ManagementTests
    {
        [Test]
        public async Task KillBySessionIdTest()
        {
            WampPlayground playground = new WampPlayground();

            playground.Host.Open();

            IWampHostedRealm realm = 
                playground.Host.RealmContainer.GetRealmByName("realm1");

            IDisposable disposable = realm.HostSessionManagementService();

            var channel = await playground.GetChannel().ConfigureAwait(false);
            var channel2 = await playground.GetChannel().ConfigureAwait(false);
            var channel3 = await playground.GetChannel().ConfigureAwait(false);

            bool channelDisconnected = false;
            channel.Channel.RealmProxy.Monitor.ConnectionBroken +=
                (sender, args) => { channelDisconnected = true; };

            bool channel2Disconnected = false;
            string channel2DisconnectReason = null;
            string channel2DisconnectMessage = null;

            channel2.Channel.RealmProxy.Monitor.ConnectionBroken +=
                (sender, args) =>
                {
                    channel2Disconnected = true;
                    channel2DisconnectReason = args.Reason;
                    channel2DisconnectMessage = args.Details.Message;
                };

            bool channel3Disconnected = false;

            channel3.Channel.RealmProxy.Monitor.ConnectionBroken +=
                (sender, args) => { channel3Disconnected = true; };

            Assert.That(!channelDisconnected && 
                        !channel2Disconnected &&
                        !channel3Disconnected,
                      Is.True, "Channels should be open before calling kill method");

            IWampSessionManagementServiceProxy proxy =
                channel.Channel.RealmProxy.Services
                       .GetCalleeProxy<IWampSessionManagementServiceProxy>();

            const string expectedReason = "wamp.myreason";
            const string expectedMessage = "Bye bye bye";

            await proxy.KillBySessionIdAsync(channel2.SessionId, expectedReason, expectedMessage)
                       .ConfigureAwait(false);

            Assert.That(channelDisconnected, Is.False);
            Assert.That(channel3Disconnected, Is.False);
            Assert.That(channel2Disconnected, Is.True);
            Assert.That(channel2DisconnectReason, Is.EqualTo(expectedReason));
            Assert.That(channel2DisconnectMessage, Is.EqualTo(expectedMessage));
        }

        [Test]
        public async Task KillAllTest()
        {
            WampPlayground playground = new WampPlayground();
        
            playground.Host.Open();
        
            IWampHostedRealm realm = 
                playground.Host.RealmContainer.GetRealmByName("realm1");
        
            IDisposable disposable = realm.HostSessionManagementService();
        
            var channel = await playground.GetChannel().ConfigureAwait(false);
            var channel2 = await playground.GetChannel().ConfigureAwait(false);
            var channel3 = await playground.GetChannel().ConfigureAwait(false);
        
            bool channelDisconnected = false;
            channel.Channel.RealmProxy.Monitor.ConnectionBroken +=
                (sender, args) => { channelDisconnected = true; };
        
            bool channel2Disconnected = false;
            string channel2DisconnectReason = null;
            string channel2DisconnectMessage = null;
        
            channel2.Channel.RealmProxy.Monitor.ConnectionBroken +=
                (sender, args) =>
                {
                    channel2Disconnected = true;
                    channel2DisconnectReason = args.Reason;
                    channel2DisconnectMessage = args.Details.Message;
                };
            
            bool channel3Disconnected = false;
            string channel3DisconnectReason = null;
            string channel3DisconnectMessage = null;
        
            channel3.Channel.RealmProxy.Monitor.ConnectionBroken +=
                (sender, args) =>
                {
                    channel3Disconnected = true;
                    channel3DisconnectReason = args.Reason;
                    channel3DisconnectMessage = args.Details.Message;
                };
        
            Assert.That(!channelDisconnected && 
                        !channel3Disconnected &&
                        !channel3Disconnected,
                      Is.True, "Channels should be open before calling kill method");
        
            IWampSessionManagementServiceProxy proxy =
                channel.Channel.RealmProxy.Services
                       .GetCalleeProxy<IWampSessionManagementServiceProxy>();
        
            const string expectedReason = "wamp.myreason";
            const string expectedMessage = "Bye bye bye";
        
            await proxy.KillAllAsync(expectedReason, expectedMessage).ConfigureAwait(false);
        
            Assert.That(channelDisconnected, Is.False);
            Assert.That(channel2Disconnected, Is.True);
            Assert.That(channel2DisconnectReason, Is.EqualTo(expectedReason));
            Assert.That(channel2DisconnectMessage, Is.EqualTo(expectedMessage));
            Assert.That(channel3Disconnected, Is.True);
            Assert.That(channel3DisconnectReason, Is.EqualTo(expectedReason));
            Assert.That(channel3DisconnectMessage, Is.EqualTo(expectedMessage));
        }
    }
}