using System;
using System.Runtime.Serialization;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using WampSharp.Core.Message;
using WampSharp.Tests.Wampv2.TestHelpers.Integration;
using WampSharp.V2.Authentication;
using WampSharp.V2.Core.Contracts;
using System.Collections.Generic;

namespace WampSharp.Tests.Wampv2.Integration
{
    [TestFixture]
    public class AuthenticationServerTests
    {
        [Test]
        public void HelloParametersArePassedToAuthenticationFactory()
        {
            MockSessionAuthenticationFactory mockSessionAuthenticationFactory = new MockSessionAuthenticationFactory();

            WampPendingClientDetails authenticatorFactoryParameters = null;

            mockSessionAuthenticationFactory.SetGetSessionAuthenticator
                ((clientDetails, transportAuthenticator) =>
                {
                    authenticatorFactoryParameters = clientDetails;
                    IWampSessionAuthenticator mockSessionAuthenticator = new MockSessionAuthenticator();
                    return mockSessionAuthenticator;
                });

            WampAuthenticationPlayground playground = 
                new WampAuthenticationPlayground(mockSessionAuthenticationFactory);

            playground.Host.Open();

            IWampServerProxy serverProxy =
                playground.CreateRawConnection(new Mock<IWampClient<JToken>>().Object);

            serverProxy.Hello("realm1", new HelloDetailsHack()
            {
                AuthenticationId = "joe",
                AuthenticationMethods = new string[] {"wampcra", "ticket"}
            });

            Assert.That(authenticatorFactoryParameters.Realm, Is.EqualTo("realm1"));
            Assert.That(authenticatorFactoryParameters.HelloDetails.AuthenticationMethods, Is.EquivalentTo(new[] { "wampcra", "ticket" }));
            Assert.That(authenticatorFactoryParameters.HelloDetails.AuthenticationId, Is.EqualTo("joe"));
        }

        [Test]
        public void ExceptionOnGetSessionAuthenticatorRaisesAbort()
        {
            MockSessionAuthenticationFactory mockSessionAuthenticationFactory =
                new MockSessionAuthenticationFactory();

            mockSessionAuthenticationFactory.SetGetSessionAuthenticator
                ((clientDetails, transportAuthenticator) =>
                {
                    throw new WampAuthenticationException(new MyAbortDetails() { Message = "aborted!", Year = 2015 }, "com.myapp.abortreason");
                });

            WampAuthenticationPlayground playground =
                new WampAuthenticationPlayground(mockSessionAuthenticationFactory);

            playground.Host.Open();

            string clientReason = null;
            AbortDetails clientAbortDetails = null;

            Mock<IWampClient<JToken>> clientMock = new Mock<IWampClient<JToken>>();

            clientMock.Setup(x => x.Abort(It.IsAny<AbortDetails>(), It.IsAny<string>()))
                      .Callback((AbortDetails details, string reason) =>
                      {
                          clientReason = reason;
                          clientAbortDetails = details;
                      });

            IWampServerProxy serverProxy =
                playground.CreateRawConnection(clientMock.Object);

            serverProxy.Hello("realm1", new HelloDetailsHack()
            {
                AuthenticationId = "joe",
                AuthenticationMethods = new string[] { "wampcra", "ticket" }
            });

            Assert.That(clientReason, Is.EqualTo("com.myapp.abortreason"));
            Assert.That(clientAbortDetails.Message, Is.EqualTo("aborted!"));

            var deserialized =
                clientAbortDetails.OriginalValue.Deserialize<MyAbortDetails>();

            Assert.That(deserialized.Year, Is.EqualTo(2015));
        }

        [Test]
        public void IsAuthenticatedTrueOnGetSessionAuthenticatorRaisesWelcome()
        {
            MockSessionAuthenticationFactory mockSessionAuthenticationFactory =
                new MockSessionAuthenticationFactory();

            WampPendingClientDetails authenticatorFactoryParameters = null;

            mockSessionAuthenticationFactory.SetGetSessionAuthenticator
                ((clientDetails, transportAuthenticator) =>
                {
                    authenticatorFactoryParameters = clientDetails;
                    MockSessionAuthenticator mockSessionAuthenticator = new MockSessionAuthenticator();
                    mockSessionAuthenticator.SetAuthenticationMethod("anonymous");
                    mockSessionAuthenticator.SetAuthenticationId(clientDetails.HelloDetails.AuthenticationId);
                    mockSessionAuthenticator.SetAuthorizer(new WampStaticAuthorizer(new List<WampUriPermissions>()));
                    mockSessionAuthenticator.SetWelcomeDetails(new MyWelcomeDetails()
                    {
                        Country = "United States of America",
                        AuthenticationProvider = "unittest",
                        AuthenticationRole = "testee"
                    });
                    mockSessionAuthenticator.SetIsAuthenticated(true);

                    return mockSessionAuthenticator;
                });

            WampAuthenticationPlayground playground =
                new WampAuthenticationPlayground(mockSessionAuthenticationFactory);

            playground.Host.Open();

            long? clientSession = null;
            WelcomeDetails clientWelcomeDetails = null;

            Mock<IWampClient<JToken>> clientMock = new Mock<IWampClient<JToken>>();

            clientMock.Setup(x => x.Welcome(It.IsAny<long>(), It.IsAny<WelcomeDetails>()))
                      .Callback((long session, WelcomeDetails details) =>
                      {
                          clientWelcomeDetails = details;
                          clientSession = session;
                      });

            IWampServerProxy serverProxy =
                playground.CreateRawConnection(clientMock.Object);

            serverProxy.Hello("realm1", new HelloDetailsHack()
            {
                AuthenticationId = "joe",
                AuthenticationMethods = new string[] { "wampcra", "ticket" }
            });

            Assert.That(clientWelcomeDetails.AuthenticationMethod, Is.EqualTo("anonymous"));
            Assert.That(clientWelcomeDetails.AuthenticationId, Is.EqualTo("joe"));
            Assert.That(clientWelcomeDetails.AuthenticationProvider, Is.EqualTo("unittest"));
            Assert.That(clientWelcomeDetails.AuthenticationRole, Is.EqualTo("testee"));
            Assert.That(clientSession, Is.EqualTo(authenticatorFactoryParameters.SessionId));

            MyWelcomeDetails deserializedWelcomeDetails =
                clientWelcomeDetails.OriginalValue.Deserialize<MyWelcomeDetails>();

            Assert.That(deserializedWelcomeDetails.Country, Is.EqualTo("United States of America"));
        }

        [Test]
        public void ChallengeParametersArePassedToClient()
        {
            MockSessionAuthenticationFactory mockSessionAuthenticationFactory = 
                new MockSessionAuthenticationFactory();

            WampPendingClientDetails authenticatorFactoryParameters = null;

            mockSessionAuthenticationFactory.SetGetSessionAuthenticator
                ((clientDetails, transportAuthenticator) =>
                {
                    authenticatorFactoryParameters = clientDetails;
                    MockSessionAuthenticator mockSessionAuthenticator = new MockSessionAuthenticator();
                    mockSessionAuthenticator.SetAuthenticationMethod("ticket");
                    mockSessionAuthenticator.SetChallengeDetails
                        (new MyChallenge {President = "Obama"});

                    return mockSessionAuthenticator;
                });

            WampAuthenticationPlayground playground =
                new WampAuthenticationPlayground(mockSessionAuthenticationFactory);

            playground.Host.Open();

            string clientAuthMethod = null;
            ChallengeDetails clientChallengeDetails = null;

            Mock<IWampClient<JToken>> clientMock = new Mock<IWampClient<JToken>>();
            clientMock.Setup(x => x.Challenge(It.IsAny<string>(), It.IsAny<ChallengeDetails>()))
                      .Callback((string authMethod, ChallengeDetails details) =>
                      {
                          clientAuthMethod = authMethod;
                          clientChallengeDetails = details;
                      });

            IWampServerProxy serverProxy =
                playground.CreateRawConnection(clientMock.Object);

            serverProxy.Hello("realm1", new HelloDetailsHack()
            {
                AuthenticationId = "joe",
                AuthenticationMethods = new string[] { "wampcra", "ticket" }
            });

            MyChallenge deserializedChallengeDetails =
                clientChallengeDetails.OriginalValue.Deserialize<MyChallenge>();

            Assert.That(deserializedChallengeDetails.President, Is.EqualTo("Obama"));
            Assert.That(clientAuthMethod, Is.EqualTo("ticket"));
        }

        [Test]
        public void AuthenticateParametersArePassedToSessionAuthenticator()
        {
            MockSessionAuthenticationFactory mockSessionAuthenticationFactory =
                new MockSessionAuthenticationFactory();

            string receivedSignature = null;
            AuthenticateExtraData receivedExtraData = null;

            mockSessionAuthenticationFactory.SetGetSessionAuthenticator
                ((clientDetails, transportAuthenticator) =>
                {
                    MockSessionAuthenticator mockSessionAuthenticator = new MockSessionAuthenticator();
                    mockSessionAuthenticator.SetAuthenticationMethod("ticket");
                    mockSessionAuthenticator.SetAuthenticate((signature, extra) =>
                    {
                        receivedSignature = signature;
                        receivedExtraData = extra;
                    });
                    return mockSessionAuthenticator;
                });


            WampAuthenticationPlayground playground =
                new WampAuthenticationPlayground(mockSessionAuthenticationFactory);

            playground.Host.Open();

            Mock<IWampClient<JToken>> clientMock = new Mock<IWampClient<JToken>>();

            IWampServerProxy serverProxy =
                playground.CreateRawConnection(clientMock.Object);

            serverProxy.Hello("realm1", new HelloDetailsHack()
            {
                AuthenticationId = "joe",
                AuthenticationMethods = new string[] { "wampcra", "ticket" }
            });

            serverProxy.Authenticate("Barack Hussein", new MyAuthenticateExtraData() {Wife = "Michelle"});

            MyAuthenticateExtraData deserializedExtraData =
                receivedExtraData.OriginalValue.Deserialize<MyAuthenticateExtraData>();

            Assert.That(receivedSignature, Is.EqualTo("Barack Hussein"));

            Assert.That(deserializedExtraData.Wife, Is.EqualTo("Michelle"));
        }

        [Test]
        public void ExceptionOnAuthenticateRaisesAbort()
        {
            MockSessionAuthenticationFactory mockSessionAuthenticationFactory =
                new MockSessionAuthenticationFactory();

            WampPendingClientDetails authenticatorFactoryParameters = null;

            mockSessionAuthenticationFactory.SetGetSessionAuthenticator
                ((clientDetails, transportAuthenticator) =>
                {
                    authenticatorFactoryParameters = clientDetails;
                    MockSessionAuthenticator mockSessionAuthenticator = new MockSessionAuthenticator();
                    mockSessionAuthenticator.SetAuthenticationMethod("ticket");

                    mockSessionAuthenticator.SetAuthenticate((signature, extraData) => throw new WampAuthenticationException(new MyAbortDetails() {Message = "aborted!", Year = 2015}, "com.myapp.abortreason"));

                    return mockSessionAuthenticator;
                });

            WampAuthenticationPlayground playground =
                new WampAuthenticationPlayground(mockSessionAuthenticationFactory);

            playground.Host.Open();

            string clientReason = null;
            AbortDetails clientAbortDetails = null;

            Mock<IWampClient<JToken>> clientMock = new Mock<IWampClient<JToken>>();

            clientMock.Setup(x => x.Abort(It.IsAny<AbortDetails>(), It.IsAny<string>()))
                      .Callback((AbortDetails details, string reason) =>
                      {
                          clientReason = reason;
                          clientAbortDetails = details;
                      });

            IWampServerProxy serverProxy =
                playground.CreateRawConnection(clientMock.Object);

            serverProxy.Hello("realm1", new HelloDetailsHack()
            {
                AuthenticationId = "joe",
                AuthenticationMethods = new string[] { "wampcra", "ticket" }
            });

            serverProxy.Authenticate("Barack Hussein", new AuthenticateExtraData());

            Assert.That(clientReason, Is.EqualTo("com.myapp.abortreason"));
            Assert.That(clientAbortDetails.Message, Is.EqualTo("aborted!"));

            var deserialized = 
                clientAbortDetails.OriginalValue.Deserialize<MyAbortDetails>();

            Assert.That(deserialized.Year, Is.EqualTo(2015));
        }

        [Test]
        public void NotAuthenticatedRaisesAbort()
        {
            MockSessionAuthenticationFactory mockSessionAuthenticationFactory =
                new MockSessionAuthenticationFactory();

            WampPendingClientDetails authenticatorFactoryParameters = null;

            mockSessionAuthenticationFactory.SetGetSessionAuthenticator
                ((clientDetails, transportAuthenticator) =>
                {
                    authenticatorFactoryParameters = clientDetails;
                    MockSessionAuthenticator mockSessionAuthenticator = new MockSessionAuthenticator();
                    mockSessionAuthenticator.SetAuthenticationMethod("ticket");

                    mockSessionAuthenticator.SetAuthenticate((signature, extraData) =>
                    {
                        mockSessionAuthenticator.SetAuthenticationId(clientDetails.HelloDetails.AuthenticationId);
                        mockSessionAuthenticator.SetIsAuthenticated(false);
                    });

                    return mockSessionAuthenticator;
                });

            WampAuthenticationPlayground playground =
                new WampAuthenticationPlayground(mockSessionAuthenticationFactory);

            playground.Host.Open();

            Mock<IWampClient<JToken>> clientMock = new Mock<IWampClient<JToken>>();

            IWampServerProxy serverProxy =
                playground.CreateRawConnection(clientMock.Object);

            serverProxy.Hello("realm1", new HelloDetailsHack()
            {
                AuthenticationId = "joe",
                AuthenticationMethods = new string[] {"wampcra", "ticket"}
            });

            serverProxy.Authenticate("Barack Hussein", new AuthenticateExtraData());

            clientMock.Verify(x => x.Abort(It.IsAny<AbortDetails>(), It.IsAny<string>()));
        }


        [Test]
        public void WelcomeParametersArePassedToClient()
        {
            MockSessionAuthenticationFactory mockSessionAuthenticationFactory =
                new MockSessionAuthenticationFactory();

            WampPendingClientDetails authenticatorFactoryParameters = null;

            mockSessionAuthenticationFactory.SetGetSessionAuthenticator
                ((clientDetails, transportAuthenticator) =>
                {
                    authenticatorFactoryParameters = clientDetails;
                    MockSessionAuthenticator mockSessionAuthenticator = new MockSessionAuthenticator();
                    mockSessionAuthenticator.SetAuthenticationMethod("ticket");

                    mockSessionAuthenticator.SetAuthenticate((signature, extraData) =>
                    {
                        mockSessionAuthenticator.SetAuthenticationId(clientDetails.HelloDetails.AuthenticationId);
                        mockSessionAuthenticator.SetIsAuthenticated(true);
                        mockSessionAuthenticator.SetWelcomeDetails(new MyWelcomeDetails()
                        {
                            AuthenticationProvider = "unittest",
                            AuthenticationRole = "testee",
                            Country = "United States of America"
                        });

                        mockSessionAuthenticator.SetAuthorizer(new WampStaticAuthorizer(new List<WampUriPermissions>()));
                    });

                    return mockSessionAuthenticator;
                });

            WampAuthenticationPlayground playground =
                new WampAuthenticationPlayground(mockSessionAuthenticationFactory);

            playground.Host.Open();

            long? clientSessionId = null;
            WelcomeDetails clientWelcomeDetails = null;

            Mock<IWampClient<JToken>> clientMock = new Mock<IWampClient<JToken>>();
            clientMock.Setup(x => x.Welcome(It.IsAny<long>(), It.IsAny<WelcomeDetails>()))
                      .Callback((long sessionId, WelcomeDetails details) =>
                      {
                          clientSessionId = sessionId;
                          clientWelcomeDetails = details;
                      });

            IWampServerProxy serverProxy =
                playground.CreateRawConnection(clientMock.Object);

            serverProxy.Hello("realm1", new HelloDetailsHack()
            {
                AuthenticationId = "joe",
                AuthenticationMethods = new string[] { "wampcra", "ticket" }
            });

            serverProxy.Authenticate("Barack Hussein", new AuthenticateExtraData());

            Assert.That(clientWelcomeDetails.AuthenticationMethod, Is.EqualTo("ticket"));
            Assert.That(clientWelcomeDetails.AuthenticationId, Is.EqualTo("joe"));
            Assert.That(clientWelcomeDetails.AuthenticationProvider, Is.EqualTo("unittest"));
            Assert.That(clientWelcomeDetails.AuthenticationRole, Is.EqualTo("testee"));
            Assert.That(clientSessionId, Is.EqualTo(authenticatorFactoryParameters.SessionId));

            MyWelcomeDetails deserializedWelcomeDetails =
                clientWelcomeDetails.OriginalValue.Deserialize<MyWelcomeDetails>();

            Assert.That(deserializedWelcomeDetails.Country, Is.EqualTo("United States of America"));
        }

        private class MockSessionAuthenticationFactory : IWampSessionAuthenticatorFactory
        {
            private Func<WampPendingClientDetails, IWampSessionAuthenticator, IWampSessionAuthenticator>
                mGetSessionAuthenticator =
                    (details, transportAuthenticator) =>
                    {
                        throw new NotImplementedException();
                    };

            public void SetGetSessionAuthenticator
                (Func<WampPendingClientDetails, IWampSessionAuthenticator, IWampSessionAuthenticator> value)
            {
                mGetSessionAuthenticator = value;
            }

            public IWampSessionAuthenticator GetSessionAuthenticator(WampPendingClientDetails details,
                                                                     IWampSessionAuthenticator transportAuthenticator)
            {
                return mGetSessionAuthenticator(details, transportAuthenticator);
            }
        }

        internal class MockSessionAuthenticator : WampSessionAuthenticator
        {
            private string mAuthenticationId;
            private string mAuthenticationMethod;

            private Action<string, AuthenticateExtraData> mAuthenticate = (signature, extra) =>
            {
                throw new NotImplementedException();
            };

            public override void Authenticate(string signature, AuthenticateExtraData extra)
            {
                mAuthenticate(signature, extra);
            }

            public override string AuthenticationId => mAuthenticationId;

            public override string AuthenticationMethod => mAuthenticationMethod;

            public void SetAuthenticate(Action<string, AuthenticateExtraData> value)
            {
                mAuthenticate = value;
            }

            public void SetAuthenticationId(string value)
            {
                mAuthenticationId = value;
            }

            public void SetAuthenticationMethod(string value)
            {
                mAuthenticationMethod = value;
            }

            public void SetAuthorizer(IWampAuthorizer authorizer)
            {
                base.Authorizer = authorizer;
            }

            public void SetChallengeDetails(ChallengeDetails value)
            {
                base.ChallengeDetails = value;
            }

            public void SetIsAuthenticated(bool value)
            {
                IsAuthenticated = value;
            }

            public void SetWelcomeDetails(WelcomeDetails welcomeDetails)
            {
                base.WelcomeDetails = welcomeDetails;
            }
        }

        [DataContract]
        [WampDetailsOptions(WampMessageType.v2Hello)]
        private class HelloDetailsHack : HelloDetails
        {
            [DataMember(Name = "authmethods")]
            public new string[] AuthenticationMethods { get; set; }

            [DataMember(Name = "authid")]
            public new string AuthenticationId { get; set; }
        }

        private class MyChallenge : ChallengeDetails
        {
            [JsonProperty("president")]
            public string President { get; set; }
        }

        private class MyAuthenticateExtraData : AuthenticateExtraData
        {
            [JsonProperty("wife")]
            public string Wife { get; set; }
        }

        private class MyWelcomeDetails : WelcomeDetails
        {
            [JsonProperty("country")]
            public string Country { get; set; }
        }

        private class MyAbortDetails : AbortDetails
        {
            [JsonProperty("year")]
            public int Year { get; set; }
        }
    }
}