using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using WampSharp.Tests.Wampv2.TestHelpers.Integration;
using WampSharp.V2;
using WampSharp.V2.Authentication;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm;
using WampSharp.V2.Rpc;

namespace WampSharp.Tests.Wampv2.Integration
{
    [TestFixture]
    public class WampCraAuthenticationTests
    {
        [Test]
        public async Task AuthenticatedIntegrationTest()
        {
            WampAuthenticationPlayground playground =
                new WampAuthenticationPlayground
                    (new WampCraUserDbAuthenticationFactory
                         (new MyAuthenticationProvider(),
                          new MyUserDb()));

            SetupHost(playground);

            IWampClientAuthenticator authenticator = 
                new WampCraClientAuthenticator(authenticationId: "peter", secret: "secret1");

            IWampChannel channel =
                playground.CreateNewChannel("realm1", authenticator);

            IWampRealmProxy realmProxy = channel.RealmProxy;

            await channel.Open().ConfigureAwait(false);

            // call a procedure we are allowed to call (so this should succeed)
            //
            IAdd2AsyncService proxy = realmProxy.Services.GetCalleeProxy<IAdd2AsyncService>();

            int five = await proxy.Add2(2, 3).ConfigureAwait(false);

            Assert.That(five, Is.EqualTo(5));

            // (try to) register a procedure where we are not allowed to (so this should fail)
            //
            Mul2Service service = new Mul2Service();

            WampException registerException = null;

            try
            {
                await realmProxy.Services.RegisterCallee(service)
                    .ConfigureAwait(false);
            }
            catch (WampException ex)
            {
                registerException = ex;
            }

            Assert.That(registerException, Is.Not.Null);

            // (try to) publish to some topics
            //
            string[] topics =
            {
                "com.example.topic1",
                "com.example.topic2",
                "com.foobar.topic1",
                "com.foobar.topic2"
            };

            List<string> successfulTopics = new List<string>();

            foreach (string topic in topics)
            {
                IWampTopicProxy topicProxy = realmProxy.TopicContainer.GetTopicByUri(topic);

                try
                {
                    await topicProxy.Publish(new PublishOptions() { Acknowledge = true },
                        new object[] { "hello" })
                        .ConfigureAwait(false);

                    successfulTopics.Add(topic);
                }
                catch (WampException)
                {
                }
            }

            Assert.That(successfulTopics, Is.EquivalentTo(new string[]
            {
                "com.foobar.topic1",
                "com.example.topic1"
            }));
        }

        [Test]
        public async Task NotAuthenticatedIntegrationTest()
        {
            WampAuthenticationPlayground playground =
                new WampAuthenticationPlayground
                    (new WampCraUserDbAuthenticationFactory
                         (new MyAuthenticationProvider(),
                          new MyUserDb()));

            SetupHost(playground);

            IWampClientAuthenticator authenticator =
                new WampCraClientAuthenticator(authenticationId: "peter", secret: "SECRET");

            IWampChannel channel =
                playground.CreateNewChannel("realm1", authenticator);

            IWampRealmProxy realmProxy = channel.RealmProxy;

            WampConnectionBrokenException openException = null;

            try
            {
                await channel.Open().ConfigureAwait(false);
            }
            catch (WampConnectionBrokenException ex)
            {
                openException = ex;
            }

            Assert.That(openException, Is.Not.Null);
            Assert.That(openException.CloseType, Is.EqualTo(SessionCloseType.Abort));
            Assert.That(openException.Reason, Is.EqualTo(WampErrors.NotAuthorized));
        }

        private static void SetupHost(WampAuthenticationPlayground playground)
        {
            IWampHost host = playground.Host;

            IWampHostedRealm realm = host.RealmContainer.GetRealmByName("realm1");

            string[] topics = new[]
            {
                "com.example.topic1",
                "com.example.topic2",
                "com.foobar.topic1",
                "com.foobar.topic2"
            };

            foreach (string topic in topics)
            {
                realm.TopicContainer.CreateTopicByUri(topic, true);
            }

            realm.Services.RegisterCallee(new Add2Service());

            host.Open();
        }

        public class Add2Service : IAdd2Service
        {
            public int Add2(int x, int y)
            {
                return (x + y);
            }
        }

        public interface IAdd2AsyncService
        {
            [WampProcedure("com.example.add2")]
            Task<int> Add2(int a, int b);
        }

        public interface IAdd2Service
        {
            [WampProcedure("com.example.add2")]
            int Add2(int a, int b);
        }

        private class MyAuthenticationProvider : WampStaticAuthenticationProvider
        {
            public MyAuthenticationProvider() :
                base(new Dictionary<string, IDictionary<string, WampAuthenticationRole>>()
                {
                    {
                        "realm1", new Dictionary<string, WampAuthenticationRole>()
                        {
                            {
                                "frontend",
                                new WampAuthenticationRole
                                {
                                    Authorizer = new WampStaticAuthorizer(new List<WampUriPermissions>
                                    {
                                        new WampUriPermissions
                                        {
                                            Uri = "com.example.add2",
                                            CanCall = true
                                        },
                                        new WampUriPermissions
                                        {
                                            Uri = "com.example.",
                                            Prefixed = true,
                                            CanPublish = true
                                        },
                                        new WampUriPermissions
                                        {
                                            Uri = "com.example.topic2",
                                            CanPublish = false
                                        },
                                        new WampUriPermissions
                                        {
                                            Uri = "com.foobar.topic1",
                                            CanPublish = true
                                        },
                                    })
                                }
                            }
                        }
                    }
                })
            {
            }
        }

        private class MyUserDb : WampCraStaticUserDb
        {
            public MyUserDb() : base(new Dictionary<string, WampCraUser>()
            {
                {
                    "joe", new WampCraUser()
                    {
                        Secret = "secret2",
                        AuthenticationRole = "frontend"
                    }
                },
                {
                    "peter", new WampCraUser()
                    {
                        Secret = "prq7+YkJ1/KlW1X0YczMHw==",
                        AuthenticationRole = "frontend",
                        Salt = "salt123",
                        Iterations = 100,
                        KeyLength = 16
                    }
                },
            })
            {
            }
        }

        private class Mul2Service
        {
            [WampProcedure("com.example.mul2")]
            public int Multiply2(int x, int y)
            {
                return x * y;
            }
        }
    }
}