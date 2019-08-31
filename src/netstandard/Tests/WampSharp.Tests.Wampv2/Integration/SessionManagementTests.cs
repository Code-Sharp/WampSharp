using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using WampSharp.Tests.Wampv2.TestHelpers.Integration;
using WampSharp.V2;
using WampSharp.V2.Authentication;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Management;
using WampSharp.V2.Realm;
using WampSharp.V2.MetaApi;

namespace WampSharp.Tests.Wampv2.Integration
{
    [TestFixture]
    public class SessionManagementTests
    {
        [Test]
        [TestCase(3,5)]
        [TestCase(7,7, true)]
        public async Task KillBySessionIdTest(int killer, int killed, bool expectException = false)
        {
            WampPlayground playground = SetupHost();

            List<IWampChannel> channels = new List<IWampChannel>();
            IDictionary<int, long> channelToSessionId = new Dictionary<int, long>();

            for (int i = 0; i < 30; i++)
            {
                IWampChannel channel = 
                    playground.CreateNewChannel("realm1");

                int copyOfKey = i;

                channel.RealmProxy.Monitor.ConnectionEstablished +=
                    (sender, args) => { channelToSessionId[copyOfKey] = args.SessionId; };

                channels.Add(channel);
            }

            Dictionary<int, (string reason, string message)> disconnectionDetails = 
                await OpenChannels(channels).ConfigureAwait(false);

            Assert.That(disconnectionDetails.Count, Is.EqualTo(0));

            IWampSessionManagementServiceProxy proxy =
                channels[killer].RealmProxy.Services
                                .GetCalleeProxy<IWampSessionManagementServiceProxy>();

            const string expectedReason = "wamp.myreason";
            const string expectedMessage = "Bye bye bye";

            WampException exception = null;

            try
            {
                await proxy.KillBySessionIdAsync(channelToSessionId[killed], expectedReason, expectedMessage).ConfigureAwait(false);
            }
            catch (WampException ex)
            {
                exception = ex;
            }

            if (expectException)
            {
                Assert.That(exception, Is.Not.Null);
                Assert.That(exception.ErrorUri, Is.EqualTo("wamp.error.no_such_session"));
            }

            AssertClosedChannels(new []{killed}.Except(new[] {killer}), disconnectionDetails, expectedMessage,
                                 expectedReason);
        }

        [Test]
        [TestCase(0)]
        [TestCase(5)]
        [TestCase(7)]
        public async Task KillAllTest(int killer)
        {
            WampPlayground playground = SetupHost();

            List<IWampChannel> channels = new List<IWampChannel>();

            for (int i = 0; i < 30; i++)
            {
                IWampChannel channel = 
                    playground.CreateNewChannel("realm1");

                channels.Add(channel);
            }

            Dictionary<int, (string reason, string message)> disconnectionDetails = 
                await OpenChannels(channels).ConfigureAwait(false);

            Assert.That(disconnectionDetails.Count, Is.EqualTo(0));

            IWampSessionManagementServiceProxy proxy =
                channels[killer].RealmProxy.Services
                       .GetCalleeProxy<IWampSessionManagementServiceProxy>();

            const string expectedReason = "wamp.myreason";
            const string expectedMessage = "Bye bye bye";

            await proxy.KillAllAsync(expectedReason, expectedMessage).ConfigureAwait(false);

            AssertClosedChannels(Enumerable.Range(0, 30).Except(new[] {killer}), disconnectionDetails, expectedMessage,
                                 expectedReason);
        }

        private static WampPlayground SetupHost()
        {
            WampPlayground playground = new WampPlayground();

            playground.Host.Open();

            IWampHostedRealm realm =
                playground.Host.RealmContainer.GetRealmByName("realm1");

            IDisposable disposable = realm.HostSessionManagementService();
            return playground;
        }

        [TestCase(0, 3, new []{3,13,23})]
        [TestCase(4, 4, new []{14,24})]
        public async Task KillByRoleTest(int killingChannel, int killedChannel, IEnumerable<int> expectedDeadChannels)
        {
            WampAuthenticationPlayground playground = SetupAuthenticationHost();

            List<IWampChannel> channels = new List<IWampChannel>();

            for (int i = 0; i < 30; i++)
            {
                IWampChannel channel = 
                    playground.CreateNewChannel("realm1", new WampCraClientAuthenticator("user" + i, "secret"));

                channels.Add(channel);
            }

            var disconnectionDetails = await OpenChannels(channels).ConfigureAwait(false);

            Assert.That(disconnectionDetails.Count, Is.EqualTo(0));

            IWampSessionManagementServiceProxy proxy = 
                channels[killingChannel].RealmProxy.Services.GetCalleeProxy<IWampSessionManagementServiceProxy>();

            const string expectedReason = "wamp.myreason";
            const string expectedMessage = "Bye bye bye";

            await proxy.KillByAuthRoleAsync("role" + killedChannel, expectedReason, expectedMessage).ConfigureAwait(false);

            AssertClosedChannels(expectedDeadChannels, disconnectionDetails, expectedMessage, expectedReason);
        }

        [TestCase(2, 1, new []{1,16})]
        [TestCase(3, 3, new []{18})]
        [TestCase(3, 40, new int[0])]
        public async Task KillByAuthIdTest(int killingChannel, int killedChannel, IEnumerable<int> expectedDeadChannels)
        {
            WampAuthenticationPlayground playground = SetupAuthenticationHost();

            List<IWampChannel> channels = new List<IWampChannel>();
        
            for (int i = 0; i < 30; i++)
            {
                IWampChannel channel = 
                    playground.CreateNewChannel("realm1", new WampCraClientAuthenticator("user" + i % 15, "secret"));
        
                channels.Add(channel);
            }

            Dictionary<int, (string reason, string message)> disconnectionDetails = 
                await OpenChannels(channels);

            Assert.That(disconnectionDetails.Count, Is.EqualTo(0));
        
            IWampSessionManagementServiceProxy proxy = 
                channels[killingChannel].RealmProxy.Services.GetCalleeProxy<IWampSessionManagementServiceProxy>();
        
            const string expectedReason = "wamp.myreason";
            const string expectedMessage = "Bye bye bye";
        
            await proxy.KillByAuthIdAsync("user" + killedChannel, expectedReason, expectedMessage).ConfigureAwait(false);
        
            AssertClosedChannels(expectedDeadChannels, disconnectionDetails, expectedMessage, expectedReason);
        }

        private static void AssertClosedChannels(IEnumerable<int> expectedDeadChannels, Dictionary<int, (string reason, string message)> disconnectionDetails,
                                                 string expectedMessage, string expectedReason)
        {
            Assert.That(disconnectionDetails.Keys, Is.EquivalentTo(expectedDeadChannels));

            foreach (var keyValuePair in disconnectionDetails)
            {
                (string reason, string message) details = keyValuePair.Value;
                Assert.That(details.message, Is.EqualTo(expectedMessage));
                Assert.That(details.reason, Is.EqualTo(expectedReason));
            }
        }

        private static WampAuthenticationPlayground SetupAuthenticationHost()
        {
            WampAuthenticationPlayground playground =
                GetWampCraPlayground();

            playground.Host.Open();

            IWampHostedRealm realm =
                playground.Host.RealmContainer.GetRealmByName("realm1");

            IDisposable disposable = realm.HostSessionManagementService();
            return playground;
        }

        private static async Task<Dictionary<int, (string reason, string message)>> OpenChannels(List<IWampChannel> channels)
        {
            var disconnectionDetails =
                new Dictionary<int, (string reason, string message)>();

            for (int i = 0; i < channels.Count; i++)
            {
                int keyCopy = i;

                IWampChannel channel = channels[i];

                channel.RealmProxy.Monitor.ConnectionBroken +=
                    (sender, args) =>
                    {
                        disconnectionDetails[keyCopy] =
                            (reason: args.Reason, message: args.Details.Message);
                    };

                await channel.Open();
            }

            return disconnectionDetails;
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