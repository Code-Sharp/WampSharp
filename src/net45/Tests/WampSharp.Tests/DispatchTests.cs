using System.Linq;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using WampSharp.Core.Dispatch;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Serialization;
using WampSharp.Newtonsoft;
using WampSharp.V1.Core.Contracts;

namespace WampSharp.Tests
{
    [TestFixture]
    public class DispatchTests
    {
        private IWampFormatter<JToken> mFormatter;
        private JsonWampMessageFormatter mMessageFormatter;

        [SetUp]
        public void Setup()
        {
            mFormatter = new JsonFormatter();
            mMessageFormatter = new JsonWampMessageFormatter();
        }

        [TestCase(@"[1, ""calc"", ""http://example.com/simple/calc#""]", "calc", "http://example.com/simple/calc#",
            TestName = @"PREFIX_message_1")]
        [TestCase(@"[1, ""keyvalue"", ""http://example.com/simple/keyvalue#""]", "keyvalue",
            "http://example.com/simple/keyvalue#", TestName = @"PREFIX_message_2")]
        public void Prefix(string message, string prefix, string uri)
        {
            var clientMock = new Mock<IWampClient>();
            Mock<IWampServer<JToken>> mock = CallHandleOnMock(clientMock.Object, message);

            mock.Verify(x => x.Prefix(clientMock.Object,
                                      prefix,
                                      uri));
        }

        [TestCase(@"[2, ""7DK6TdN4wLiUJgNM"", ""http://example.com/api#howdy""]", "7DK6TdN4wLiUJgNM",
            "http://example.com/api#howdy", new string[] {},
            TestName = "CALL_message_for_RPC_with_no_arguments")]
        [TestCase(@"[2, ""Yp9EFZt9DFkuKndg"", ""api:add2"", 23, 99]", "Yp9EFZt9DFkuKndg", "api:add2", new[] {"23", "99"}
            ,
            TestName = "CALL_message_for_RPC_with_2_arguments_using_CURIE")]
        [TestCase(@"[2, ""J5DkZJgByutvaDWc"", ""http://example.com/api#storeMeal"",
               {
                  ""category"": ""dinner"",
                  ""calories"": 2309
               }]",
            "J5DkZJgByutvaDWc",
            "http://example.com/api#storeMeal",
            new[] {@"{
                  ""category"": ""dinner"",
                  ""calories"": 2309
               }"},
            TestName = "CALL_message_for_RPC_with_1_complex_object_argument")]
        [TestCase(@"[2, ""Dns3wuQo0ipOX1Xc"", ""http://example.com/api#woooat"", null]",
            "Dns3wuQo0ipOX1Xc",
            "http://example.com/api#woooat",
            new[] {@"null"},
            TestName = "CALL_message_for_RPC_with_1_argument_value_being_null")]
        [TestCase(@"[2, ""M0nncaH0ywCSYzRv"", ""api:sum"", [9, 1, 3, 4]]",
            "M0nncaH0ywCSYzRv",
            "api:sum",
            new[] {@"[9, 1, 3, 4]"},
            TestName = "CALL_message_for_RPC_with_1_argument_value_being_a_list_of_integers_using_CURIE")]
        [TestCase(@"[2, ""ujL7WKGXCn8bkvFV"", ""keyvalue:set"",
               ""foobar"",
               {
                  ""value1"": ""23"",
                  ""value2"": ""singsing"",
                  ""value3"": true,
                  ""modified"": ""2012-03-29T10:29:16.625Z""
               }]",
            "ujL7WKGXCn8bkvFV",
            "keyvalue:set",
            new[]
                {
                    @"""foobar""",
                    @"{
                  ""value1"": ""23"",
                  ""value2"": ""singsing"",
                  ""value3"": true,
                  ""modified"": ""2012-03-29T10:29:16.625Z""
               }"
                },
            TestName = "CALL_message_for_RPC_with_2_complex_arguments_using_CURIE")]
        public void Call(string message, string callId, string procUri, string[] arguments)
        {
            var clientMock = new Mock<IWampClient>();
            Mock<IWampServer<JToken>> mock = CallHandleOnMock(clientMock.Object, message);

            mock.Verify(x => x.Call(clientMock.Object,
                                    callId,
                                    procUri,
                                    It.Is(
                                        (JToken[] args) =>
                                        args.SequenceEqual(arguments.Select(arg => JToken.Parse(arg)),
                                                           new JTokenEqualityComparer()))));
        }

        [TestCase(@"[5, ""http://example.com/simple""]", "http://example.com/simple",
            TestName = "SUBSCRIBE_message_with_fully_qualified_URI")]
        [TestCase(@"[5, ""event:myevent1""]", "event:myevent1",
            TestName = "SUBSCRIBE_message_with_CURIE")]
        public void Subscribe(string message, string topicUri)
        {
            var clientMock = new Mock<IWampClient>();
            Mock<IWampServer<JToken>> mock = CallHandleOnMock(clientMock.Object, message);

            mock.Verify(x => x.Subscribe(clientMock.Object,
                                         topicUri));
        }

        [TestCase(@"[6, ""http://example.com/simple""]", "http://example.com/simple",
            TestName = "UNSUBSCRIBE_message_with_fully_qualified_URI")]
        [TestCase(@"[6, ""event:myevent1""]", "event:myevent1",
            TestName = "UNSUBSCRIBE_message_with_CURIE")]
        public void Unsubscribe(string message, string topicUri)
        {
            var clientMock = new Mock<IWampClient>();
            Mock<IWampServer<JToken>> mock = CallHandleOnMock(clientMock.Object, message);

            mock.Verify(x => x.Unsubscribe(clientMock.Object,
                                           topicUri));
        }

        [TestCase(@"[7, ""http://example.com/simple"", ""Hello, world!""]",
            "http://example.com/simple", @"""Hello, world!""", TestName = "PUBLISH_message_with_string_as_payload")]
        [TestCase(@"[7, ""http://example.com/simple"", null]",
            "http://example.com/simple", @"null",
            TestName = "PUBLISH_message_with_null_as_payload")]
        [TestCase(@"[7, ""http://example.com/event#myevent2"",
               {
                  ""rand"": 0.09187032734575862,
                  ""flag"": false,
                  ""num"": 23,
                  ""name"":
                  ""Kross"",
                  ""created"": ""2012-03-29T10:41:09.864Z""
               }]",
            "http://example.com/event#myevent2", @"{
                  ""rand"": 0.09187032734575862,
                  ""flag"": false,
                  ""num"": 23,
                  ""name"":
                  ""Kross"",
                  ""created"": ""2012-03-29T10:41:09.864Z""
               }",
            TestName = "PUBLISH_message_with_complex_object_as_payload")]
        public void PublishSimple(string message, string topicUri, string @event)
        {
            var clientMock = new Mock<IWampClient>();
            Mock<IWampServer<JToken>> mock = CallHandleOnMock(clientMock.Object, message);
            JTokenEqualityComparer comparer = new JTokenEqualityComparer();

            mock.Verify(x => x.Publish(clientMock.Object,
                                       topicUri,
                                       It.Is((JToken value) => comparer.Equals(value, JToken.Parse(@event)))));
        }

        [TestCase(@"[7, ""http://example.com/event#myevent2"",
               {
                  ""rand"": 0.09187032734575862,
                  ""flag"": false,
                  ""num"": 23,
                  ""name"":
                  ""Kross"",
                  ""created"": ""2012-03-29T10:41:09.864Z""
               },
               true]",
            "http://example.com/event#myevent2", @"{
                  ""rand"": 0.09187032734575862,
                  ""flag"": false,
                  ""num"": 23,
                  ""name"":
                  ""Kross"",
                  ""created"": ""2012-03-29T10:41:09.864Z""
               }",
            true,
            TestName = "PUBLISH_message_with_complex_object_as_payload_exclude_me")]
        [TestCase(@"[7, ""http://example.com/event#myevent2"",
               {
                  ""rand"": 0.09187032734575862,
                  ""flag"": false,
                  ""num"": 23,
                  ""name"":
                  ""Kross"",
                  ""created"": ""2012-03-29T10:41:09.864Z""
               },
               false]",
            "http://example.com/event#myevent2", @"{
                  ""rand"": 0.09187032734575862,
                  ""flag"": false,
                  ""num"": 23,
                  ""name"":
                  ""Kross"",
                  ""created"": ""2012-03-29T10:41:09.864Z""
               }",
            false,
            TestName = "PUBLISH_message_with_complex_object_as_payload_dont_exclude_me")]
        public void PublishExcludeMe(string message, string topicUri, string @event, bool excludeMe)
        {
            var clientMock = new Mock<IWampClient>();
            Mock<IWampServer<JToken>> mock = CallHandleOnMock(clientMock.Object, message);
            JTokenEqualityComparer comparer = new JTokenEqualityComparer();

            mock.Verify(x => x.Publish(clientMock.Object,
                                       topicUri,
                                       It.Is((JToken value) => comparer.Equals(value, JToken.Parse(@event))),
                                       excludeMe));
        }

        [TestCase(@"[7, ""event:myevent1"",
               ""hello"",
               [""NwtXQ8rdfPsy-ewS"", ""dYqgDl0FthI6_hjb""]]",
            "event:myevent1", @"""hello""", new string[] {"NwtXQ8rdfPsy-ewS", "dYqgDl0FthI6_hjb"},
            TestName = "PUBLISH_message_with_exclude_list")]
        public void PublishExludedList(string message, string topicUri, string @event, string[] excluded)
        {
            var clientMock = new Mock<IWampClient>();
            Mock<IWampServer<JToken>> mock = CallHandleOnMock(clientMock.Object, message);

            JTokenEqualityComparer comparer = new JTokenEqualityComparer();


            mock.Verify(x => x.Publish(clientMock.Object,
                                       topicUri,
                                       It.Is((JToken value) => comparer.Equals(value, JToken.Parse(@event))),
                                       It.Is((string[] excludedParameter) => excluded.SequenceEqual(excludedParameter))));
        }

        [TestCase(@"[7, ""event:myevent1"",
               ""hello"",
               [],
               [""NwtXQ8rdfPsy-ewS""]]",
            "event:myevent1", @"""hello""", new string[] {}, new[] {"NwtXQ8rdfPsy-ewS"},
            TestName = "PUBLISH_message_with_eligible_list",
            Category = "Publish")]
        [TestCase(@"[7, ""event:myevent1"",
               ""hello"",
               [""dYqgDl0FthI6_hjb""],
               [""NwtXQ8rdfPsy-ewS""]]",
            "event:myevent1", @"""hello""", new[] {"dYqgDl0FthI6_hjb"}, new[] {"NwtXQ8rdfPsy-ewS"},
            TestName = "PUBLISH_message_with_excluded_and_eligible_list",
            Category = "Publish")]
        public void PublishEligibleList(string message,
                                        string topicUri,
                                        string @event,
                                        string[] excluded,
                                        string[] eligible)
        {
            var clientMock = new Mock<IWampClient>();
            Mock<IWampServer<JToken>> mock = CallHandleOnMock(clientMock.Object, message);

            JTokenEqualityComparer comparer = new JTokenEqualityComparer();

            mock.Verify(x => x.Publish(clientMock.Object,
                                       topicUri,
                                       It.Is((JToken value) => comparer.Equals(value, JToken.Parse(@event))),
                                       It.Is((string[] excludedParameter) => excluded.SequenceEqual(excludedParameter)),
                                       It.Is((string[] eligibleParameter) => eligible.SequenceEqual(eligibleParameter))));
        }

        private Mock<IWampServer<JToken>> CallHandleOnMock(IWampClient client, string message)
        {
            Mock<IWampServer<JToken>> mock =
                new Mock<IWampServer<JToken>>();

            JToken raw = JToken.Parse(message);

            IWampIncomingMessageHandler<JToken, IWampClient> handler =
                GetHandler(mock.Object);

            handler.HandleMessage(client, mMessageFormatter.Parse(raw));

            return mock;
        }

        private IWampIncomingMessageHandler<JToken, IWampClient> GetHandler(IWampServer<JToken> wampServer)
        {
            IWampIncomingMessageHandler<JToken, IWampClient> handler =
                new WampIncomingMessageHandler<JToken, IWampClient>
                    (new WampRequestMapper<JToken>(wampServer.GetType(),
                                                   mFormatter),
                     new WampMethodBuilder<JToken, IWampClient>(wampServer, mFormatter));

            return handler;
        }
    }
}