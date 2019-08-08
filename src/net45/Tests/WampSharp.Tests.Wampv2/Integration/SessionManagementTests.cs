using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using WampSharp.Tests.Wampv2.TestHelpers.Integration;
using WampSharp.V2;
using WampSharp.V2.Authentication;
using WampSharp.V2.Client;
using WampSharp.V2.Management;
using WampSharp.V2.Realm;
using WampSharp.V2.MetaApi;

namespace WampSharp.Tests.Wampv2.Integration
{
    [TestFixture]
    public class SessionManagementTests
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

        [TestCase(0, 3, new []{3,13,23})]
        [TestCase(4, 4, new []{14,24})]
        public async Task KillByRoleTest(int killingChannel, int killedChannel, IEnumerable<int> expectedDeadChannels)
        {
            WampAuthenticationPlayground playground =
                GetWampCraPlayground();

            playground.Host.Open();

            IWampHostedRealm realm =
                playground.Host.RealmContainer.GetRealmByName("realm1");

            IDisposable disposable = realm.HostSessionManagementService();

            List<IWampChannel> channels = new List<IWampChannel>();

            var disconnectionDetails =
                new Dictionary<int, (string reason, string message)>();

            for (int i = 0; i < 30; i++)
            {
                IWampChannel channel = 
                    playground.CreateNewChannel("realm1", new WampCraClientAuthenticator("user" + i, "secret"));

                channels.Add(channel);

                int keyCopy = i;

                channel.RealmProxy.Monitor.ConnectionBroken +=
                    (sender, args) =>
                    {
                        disconnectionDetails[keyCopy] =
                            (reason: args.Reason, message: args.Details.Message);
                    };

                await channel.Open();
            }

            Assert.That(disconnectionDetails.Count, Is.EqualTo(0));

            IWampSessionManagementServiceProxy proxy = 
                channels[killingChannel].RealmProxy.Services.GetCalleeProxy<IWampSessionManagementServiceProxy>();

            const string expectedReason = "wamp.myreason";
            const string expectedMessage = "Bye bye bye";

            await proxy.KillByAuthRoleAsync("role" + killedChannel, expectedReason, expectedMessage).ConfigureAwait(false);

            Assert.That(disconnectionDetails.Keys, Is.EquivalentTo(expectedDeadChannels));

            foreach (var keyValuePair in disconnectionDetails)
            {
                (string reason, string message) details = keyValuePair.Value;
                Assert.That(details.message, Is.EqualTo(expectedMessage));
                Assert.That(details.reason, Is.EqualTo(expectedReason));
            }
        }

        [TestCase(2, 1, new []{1,16})]
        [TestCase(3, 3, new []{18})]
        [TestCase(3, 40, new int[0])]
        public async Task KillByAuthIdTest(int killingChannel, int killedChannel, IEnumerable<int> expectedDeadChannels)
        {
            WampAuthenticationPlayground playground =
                GetWampCraPlayground();
        
            playground.Host.Open();
        
            IWampHostedRealm realm =
                playground.Host.RealmContainer.GetRealmByName("realm1");
        
            IDisposable disposable = realm.HostSessionManagementService();
        
            List<IWampChannel> channels = new List<IWampChannel>();
        
            var disconnectionDetails =
                new Dictionary<int, (string reason, string message)>();
        
            for (int i = 0; i < 30; i++)
            {
                IWampChannel channel = 
                    playground.CreateNewChannel("realm1", new WampCraClientAuthenticator("user" + i % 15, "secret"));
        
                channels.Add(channel);
        
                int keyCopy = i;
        
                channel.RealmProxy.Monitor.ConnectionBroken +=
                    (sender, args) =>
                    {
                        disconnectionDetails[keyCopy] =
                            (reason: args.Reason, message: args.Details.Message);
                    };
        
                await channel.Open();
            }
        
            Assert.That(disconnectionDetails.Count, Is.EqualTo(0));
        
            IWampSessionManagementServiceProxy proxy = 
                channels[killingChannel].RealmProxy.Services.GetCalleeProxy<IWampSessionManagementServiceProxy>();
        
            const string expectedReason = "wamp.myreason";
            const string expectedMessage = "Bye bye bye";
        
            await proxy.KillByAuthIdAsync("user" + killedChannel, expectedReason, expectedMessage).ConfigureAwait(false);
        
            Assert.That(disconnectionDetails.Keys, Is.EquivalentTo(expectedDeadChannels));
        
            foreach (var keyValuePair in disconnectionDetails)
            {
                (string reason, string message) details = keyValuePair.Value;
                Assert.That(details.message, Is.EqualTo(expectedMessage));
                Assert.That(details.reason, Is.EqualTo(expectedReason));
            }
        }

        private static WampAuthenticationPlayground GetWampCraPlayground()
        {
            WampStaticAuthorizer wampStaticAuthorizer = new
                WampStaticAuthorizer(new List<WampUriPermissions>
                                     {
                                         new
                                         WampUriPermissions()
                                         {
                                             Uri = "wamp.",
                                             Prefixed = true,
                                             CanCall = true
                                         }
                                     });

            IDictionary<string, WampAuthenticationRole> wampAuthenticationRoles =
                new Dictionary<string, WampAuthenticationRole>();

            for (int i = 0; i < 10; i++)
            {
                wampAuthenticationRoles["role" + i]=new WampAuthenticationRole()
                                                    {
                                                        Authorizer = wampStaticAuthorizer
                                                    };                
            }

            Dictionary<string, IDictionary<string, WampAuthenticationRole>> realmToRoleNameToRole = new Dictionary<string,
                                            IDictionary<string, WampAuthenticationRole>>()
                                        {
                                            {
                                                "realm1",
                                                wampAuthenticationRoles
                                            }
                                        };
            return new WampAuthenticationPlayground
                (new WampCraUserDbAuthenticationFactory
                     (new WampStaticAuthenticationProvider
                          (realmToRoleNameToRole),
                      new WampCraDynamicUserDb()));
        }


        private class WampCraDynamicUserDb : IWampCraUserDb
        {
            public WampCraUser GetUserById(string authenticationId)
            {
                return new WampCraUser()
                       {
                           AuthenticationId = authenticationId,
                           AuthenticationRole = "role" + authenticationId.Last(),
                           Secret = "secret"
                       };
            }
        }
    }
}