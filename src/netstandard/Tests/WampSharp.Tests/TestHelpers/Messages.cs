using System;
using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Message;

namespace WampSharp.Tests.TestHelpers
{
    public class Messages
    {
        private static readonly WampMessage<MockRaw> mPrefixMessage1;
        private static readonly WampMessage<MockRaw> mCallMessageForRpcWith1ArgumentValueBeingAListOfIntegersUsingCurie;
        private static readonly WampMessage<MockRaw> mCallMessageForRpcWith1ComplexObjectArgument;
        private static readonly WampMessage<MockRaw> mEventMessageWithComplexObjectPayload;

        static Messages()
        {
            PrefixMessage2 = new WampMessage<MockRaw>();
            {
                PrefixMessage2.MessageType = WampMessageType.v1Prefix;
                MockRaw[] arguments = new MockRaw[2];
                arguments[0] = new MockRaw("keyvalue");
                arguments[1] = new MockRaw("http://example.com/simple/keyvalue#");
                PrefixMessage2.Arguments = arguments;
            }
            mPrefixMessage1 = new WampMessage<MockRaw>();
            {
                mPrefixMessage1.MessageType = WampMessageType.v1Prefix;
                MockRaw[] arguments = new MockRaw[2];
                arguments[0] = new MockRaw("calc");
                arguments[1] = new MockRaw("http://example.com/simple/calc#");
                mPrefixMessage1.Arguments = arguments;
            }
            CallMessageForRpcWith2ArgumentsUsingCurie = new WampMessage<MockRaw>();
            {
                CallMessageForRpcWith2ArgumentsUsingCurie.MessageType = WampMessageType.v1Call;
                MockRaw[] arguments = new MockRaw[4];
                arguments[0] = new MockRaw("Yp9EFZt9DFkuKndg");
                arguments[1] = new MockRaw("api:add2");
                arguments[2] = new MockRaw(23);
                arguments[3] = new MockRaw(99);
                CallMessageForRpcWith2ArgumentsUsingCurie.Arguments = arguments;
            }
            CallMessageForRpcWith1ArgumentValueBeingNull = new WampMessage<MockRaw>();
            {
                CallMessageForRpcWith1ArgumentValueBeingNull.MessageType = WampMessageType.v1Call;
                MockRaw[] arguments = new MockRaw[3];
                arguments[0] = new MockRaw("Dns3wuQo0ipOX1Xc");
                arguments[1] = new MockRaw("http://example.com/api#woooat");
                arguments[2] = new MockRaw(null);
                CallMessageForRpcWith1ArgumentValueBeingNull.Arguments = arguments;
            }
            mCallMessageForRpcWith1ArgumentValueBeingAListOfIntegersUsingCurie = new WampMessage<MockRaw>();
            {
                mCallMessageForRpcWith1ArgumentValueBeingAListOfIntegersUsingCurie.MessageType = WampMessageType.v1Call;
                MockRaw[] arguments = new MockRaw[3];
                arguments[0] = new MockRaw("M0nncaH0ywCSYzRv");
                arguments[1] = new MockRaw("api:sum");
                arguments[2] = new MockRaw(new[]
                                               {
                                                   9,
                                                   1,
                                                   3,
                                                   4,
                                               });
                mCallMessageForRpcWith1ArgumentValueBeingAListOfIntegersUsingCurie.Arguments = arguments;
            }
            CallMessageForRpcWith2ComplexArgumentsUsingCurie = new WampMessage<MockRaw>();
            {
                CallMessageForRpcWith2ComplexArgumentsUsingCurie.MessageType = WampMessageType.v1Call;
                MockRaw[] arguments = new MockRaw[4];
                arguments[0] = new MockRaw("ujL7WKGXCn8bkvFV");
                arguments[1] = new MockRaw("keyvalue:set");
                arguments[2] = new MockRaw("foobar");
                arguments[3] = new MockRaw(new
                                               {
                                                   value1 = 23,
                                                   value2 = "singsing",
                                                   value3 = true,
                                                   modified = new DateTime(2012, 3, 29, 10, 29, 16, 625),
                                               });
                CallMessageForRpcWith2ComplexArgumentsUsingCurie.Arguments = arguments;
            }
            mCallMessageForRpcWith1ComplexObjectArgument = new WampMessage<MockRaw>();
            {
                mCallMessageForRpcWith1ComplexObjectArgument.MessageType = WampMessageType.v1Call;
                MockRaw[] arguments = new MockRaw[3];
                arguments[0] = new MockRaw("J5DkZJgByutvaDWc");
                arguments[1] = new MockRaw("http://example.com/api#storeMeal");
                arguments[2] = new MockRaw(new
                                               {
                                                   category = "dinner",
                                                   calories = 2309,
                                               });
                mCallMessageForRpcWith1ComplexObjectArgument.Arguments = arguments;
            }
            CallMessageForRpcWithNoArguments = new WampMessage<MockRaw>();
            {
                CallMessageForRpcWithNoArguments.MessageType = WampMessageType.v1Call;
                MockRaw[] arguments = new MockRaw[2];
                arguments[0] = new MockRaw("7DK6TdN4wLiUJgNM");
                arguments[1] = new MockRaw("http://example.com/api#howdy");
                CallMessageForRpcWithNoArguments.Arguments = arguments;
            }
            SubscribeMessageWithFullyQualifiedUri = new WampMessage<MockRaw>();
            {
                SubscribeMessageWithFullyQualifiedUri.MessageType = WampMessageType.v1Subscribe;
                MockRaw[] arguments = new MockRaw[1];
                arguments[0] = new MockRaw("http://example.com/simple");
                SubscribeMessageWithFullyQualifiedUri.Arguments = arguments;
            }
            SubscribeMessageWithCurie = new WampMessage<MockRaw>();
            {
                SubscribeMessageWithCurie.MessageType = WampMessageType.v1Subscribe;
                MockRaw[] arguments = new MockRaw[1];
                arguments[0] = new MockRaw("event:myevent1");
                SubscribeMessageWithCurie.Arguments = arguments;
            }
            UnsubscribeMessageWithCurie = new WampMessage<MockRaw>();
            {
                UnsubscribeMessageWithCurie.MessageType = WampMessageType.v1Unsubscribe;
                MockRaw[] arguments = new MockRaw[1];
                arguments[0] = new MockRaw("event:myevent1");
                UnsubscribeMessageWithCurie.Arguments = arguments;
            }
            UnsubscribeMessageWithFullyQualifiedUri = new WampMessage<MockRaw>();
            {
                UnsubscribeMessageWithFullyQualifiedUri.MessageType = WampMessageType.v1Unsubscribe;
                MockRaw[] arguments = new MockRaw[1];
                arguments[0] = new MockRaw("http://example.com/simple");
                UnsubscribeMessageWithFullyQualifiedUri.Arguments = arguments;
            }
            PublishMessageWithComplexObjectAsPayload = new WampMessage<MockRaw>();
            {
                PublishMessageWithComplexObjectAsPayload.MessageType = WampMessageType.v1Publish;
                MockRaw[] arguments = new MockRaw[2];
                arguments[0] = new MockRaw("http://example.com/event#myevent2");
                arguments[1] = new MockRaw(new
                                               {
                                                   rand = 0.0918703273457586,
                                                   flag = false,
                                                   num = 23,
                                                   name = "Kross",
                                                   created = new DateTime(2012, 3, 29, 10, 41, 9, 864),
                                               });
                PublishMessageWithComplexObjectAsPayload.Arguments = arguments;
            }
            PublishMessageWithNullAsPayload = new WampMessage<MockRaw>();
            {
                PublishMessageWithNullAsPayload.MessageType = WampMessageType.v1Publish;
                MockRaw[] arguments = new MockRaw[2];
                arguments[0] = new MockRaw("http://example.com/simple");
                arguments[1] = new MockRaw(null);
                PublishMessageWithNullAsPayload.Arguments = arguments;
            }
            PublishMessageWithStringAsPayload = new WampMessage<MockRaw>();
            {
                PublishMessageWithStringAsPayload.MessageType = WampMessageType.v1Publish;
                MockRaw[] arguments = new MockRaw[2];
                arguments[0] = new MockRaw("http://example.com/simple");
                arguments[1] = new MockRaw("Hello, world!");
                PublishMessageWithStringAsPayload.Arguments = arguments;
            }
            PublishMessageWithComplexObjectAsPayloadExcludeMe = new WampMessage<MockRaw>();
            {
                PublishMessageWithComplexObjectAsPayloadExcludeMe.MessageType = WampMessageType.v1Publish;
                MockRaw[] arguments = new MockRaw[3];
                arguments[0] = new MockRaw("http://example.com/event#myevent2");
                arguments[1] = new MockRaw(new
                                               {
                                                   rand = 0.0918703273457586,
                                                   flag = false,
                                                   num = 23,
                                                   name = "Kross",
                                                   created = new DateTime(2012, 3, 29, 10, 41, 9, 864),
                                               });
                arguments[2] = new MockRaw(true);
                PublishMessageWithComplexObjectAsPayloadExcludeMe.Arguments = arguments;
            }
            PublishMessageWithComplexObjectAsPayloadDontExcludeMe = new WampMessage<MockRaw>();
            {
                PublishMessageWithComplexObjectAsPayloadDontExcludeMe.MessageType = WampMessageType.v1Publish;
                MockRaw[] arguments = new MockRaw[3];
                arguments[0] = new MockRaw("http://example.com/event#myevent2");
                arguments[1] = new MockRaw(new
                                               {
                                                   rand = 0.0918703273457586,
                                                   flag = false,
                                                   num = 23,
                                                   name = "Kross",
                                                   created = new DateTime(2012, 3, 29, 10, 41, 9, 864),
                                               });
                arguments[2] = new MockRaw(false);
                PublishMessageWithComplexObjectAsPayloadDontExcludeMe.Arguments = arguments;
            }
            PublishMessageWithExcludeList = new WampMessage<MockRaw>();
            {
                PublishMessageWithExcludeList.MessageType = WampMessageType.v1Publish;
                MockRaw[] arguments = new MockRaw[3];
                arguments[0] = new MockRaw("event:myevent1");
                arguments[1] = new MockRaw("hello");
                arguments[2] = new MockRaw(new[]
                                               {
                                                   "NwtXQ8rdfPsy-ewS",
                                                   "dYqgDl0FthI6_hjb",
                                               });
                PublishMessageWithExcludeList.Arguments = arguments;
            }
            PublishMessageWithEligibleList = new WampMessage<MockRaw>();
            {
                PublishMessageWithEligibleList.MessageType = WampMessageType.v1Publish;
                MockRaw[] arguments = new MockRaw[4];
                arguments[0] = new MockRaw("event:myevent1");
                arguments[1] = new MockRaw("hello");
                arguments[2] = new MockRaw(new string[]
                                               {
                                               });
                arguments[3] = new MockRaw(new[]
                                               {
                                                   "NwtXQ8rdfPsy-ewS",
                                               });
                PublishMessageWithEligibleList.Arguments = arguments;
            }
            PublishMessageWithExcludedAndEligibleList = new WampMessage<MockRaw>();
            {
                PublishMessageWithExcludedAndEligibleList.MessageType = WampMessageType.v1Publish;
                MockRaw[] arguments = new MockRaw[4];
                arguments[0] = new MockRaw("event:myevent1");
                arguments[1] = new MockRaw("hello");
                arguments[2] = new MockRaw(new[]
                                               {
                                                   "dYqgDl0FthI6_hjb",
                                               });
                arguments[3] = new MockRaw(new[]
                                               {
                                                   "NwtXQ8rdfPsy-ewS",
                                               });
                PublishMessageWithExcludedAndEligibleList.Arguments = arguments;
            }

            WelcomeMessage =
                WampV1Messages.Welcome("v59mbCGDXZ7WTyxB", 1, "Autobahn/0.5.1");

            CallResultMessageWithNullResult =
                WampV1Messages.CallResult("CcDnuI2bl2oLGBzO", null);

            CallResultMessageWithStringResult =
                WampV1Messages.CallResult("otZom9UsJhrnzvLa", "Awesome result ..");

            CallResultMessageWithComplexObjectResult =
                WampV1Messages.CallResult("CcDnuI2bl2oLGBzO",
                                          new
                                              {
                                                  value1 = 23,
                                                  value2 = "singsing",
                                                  value3 = true,
                                                  modified = new DateTime(2012, 3, 29, 10, 29, 16, 625),
                                              });

            CallErrorMessageWithGenericError =
                WampV1Messages.CallError("gwbN3EDtFv6JvNV5",
                                         "http://autobahn.tavendo.de/error#generic",
                                         "math domain error");

            CallErrorMessageWithSpecificErrorAndIntegerInErrorDetails =
                WampV1Messages.CallError("7bVW5pv8r60ZeL6u",
                                         "http://example.com/error#number_too_big",
                                         "1001 too big for me, max is 1000",
                                         1000);

            CallErrorMessageWithListOfIntegersInErrorDetails =
                WampV1Messages.CallError("AStPd8RS60pfYP8c",
                                         "http://example.com/error#invalid_numbers",
                                         "one or more numbers are multiples of 3",
                                         new[] {0, 3});

            EventMessageWithStringAsPayload =
                WampV1Messages.Event("http://example.com/simple", "Hello, I am a simple event.");

            EventMessageWithNullAsPayload =
                WampV1Messages.Event("http://example.com/simple", null);

            mEventMessageWithComplexObjectPayload =
                WampV1Messages.Event("http://example.com/event#myevent2",
                                     new
                                         {
                                             rand = 0.0918703273457586,
                                             flag = false,
                                             num = 23,
                                             name = "Kross",
                                             created = new DateTime(2012, 3, 29, 10, 41, 9, 864),
                                         });
        }

        #region Properties

        public static WampMessage<MockRaw> CallErrorMessageWithListOfIntegersInErrorDetails { get; }

        public static WampMessage<MockRaw> CallErrorMessageWithSpecificErrorAndIntegerInErrorDetails { get; }

        public static WampMessage<MockRaw> CallErrorMessageWithGenericError { get; }

        public static WampMessage<MockRaw> CallResultMessageWithComplexObjectResult { get; }

        public static WampMessage<MockRaw> CallResultMessageWithStringResult { get; }

        public static WampMessage<MockRaw> CallResultMessageWithNullResult { get; }

        public static WampMessage<MockRaw> WelcomeMessage { get; }

        public static WampMessage<MockRaw> PrefixMessage2 { get; }

        public static WampMessage<MockRaw> PrefixMessage1 => mPrefixMessage1;

        public static WampMessage<MockRaw> CallMessageForRpcWith2ArgumentsUsingCurie { get; }

        public static WampMessage<MockRaw> CallMessageForRpcWith1ArgumentValueBeingNull { get; }

        public static WampMessage<MockRaw> CallMessageForRpcWith1ArgumentValueBeingAListOfIntegersUsingCurie => mCallMessageForRpcWith1ArgumentValueBeingAListOfIntegersUsingCurie;

        public static WampMessage<MockRaw> CallMessageForRpcWith2ComplexArgumentsUsingCurie { get; }

        public static WampMessage<MockRaw> CallMessageForRpcWith1ComplexObjectArgument => mCallMessageForRpcWith1ComplexObjectArgument;

        public static WampMessage<MockRaw> CallMessageForRpcWithNoArguments { get; }

        public static WampMessage<MockRaw> SubscribeMessageWithFullyQualifiedUri { get; }

        public static WampMessage<MockRaw> SubscribeMessageWithCurie { get; }

        public static WampMessage<MockRaw> UnsubscribeMessageWithCurie { get; }

        public static WampMessage<MockRaw> UnsubscribeMessageWithFullyQualifiedUri { get; }

        public static WampMessage<MockRaw> PublishMessageWithComplexObjectAsPayload { get; }

        public static WampMessage<MockRaw> PublishMessageWithNullAsPayload { get; }

        public static WampMessage<MockRaw> PublishMessageWithStringAsPayload { get; }

        public static WampMessage<MockRaw> PublishMessageWithComplexObjectAsPayloadExcludeMe { get; }

        public static WampMessage<MockRaw> PublishMessageWithComplexObjectAsPayloadDontExcludeMe { get; }

        public static WampMessage<MockRaw> PublishMessageWithExcludeList { get; }

        public static WampMessage<MockRaw> PublishMessageWithEligibleList { get; }

        public static WampMessage<MockRaw> PublishMessageWithExcludedAndEligibleList { get; }

        public static WampMessage<MockRaw> EventMessageWithStringAsPayload { get; }

        public static WampMessage<MockRaw> EventMessageWithNullAsPayload { get; }

        public static WampMessage<MockRaw> EventMessageWithComplexObjectPayload => mEventMessageWithComplexObjectPayload;

        #endregion

        #region Server Packs

        public static IEnumerable<WampMessage<MockRaw>> ServerMessages => ServerAuxiliaryMessages
                                                                          .Concat(ServerRpcMessages)
                                                                          .Concat(ServerPubSubMessages);

        public static IEnumerable<WampMessage<MockRaw>> ServerAuxiliaryMessages => PrefixMessages;


        public static IEnumerable<WampMessage<MockRaw>> ServerRpcMessages => CallMessages;

        public static IEnumerable<WampMessage<MockRaw>> ServerPubSubMessages => SubscribeMessages
                                                                                .Concat(UnsubscribeMessages)
                                                                                .Concat(PublishMessages);

        public static IEnumerable<WampMessage<MockRaw>> PrefixMessages
        {
            get
            {
                yield return PrefixMessage1;
                yield return PrefixMessage2;
            }
        }

        public static IEnumerable<WampMessage<MockRaw>> CallMessages
        {
            get
            {
                yield return CallMessageForRpcWith1ArgumentValueBeingAListOfIntegersUsingCurie;
                yield return CallMessageForRpcWith1ArgumentValueBeingNull;
                yield return CallMessageForRpcWith1ComplexObjectArgument;
                yield return CallMessageForRpcWith2ArgumentsUsingCurie;
                yield return CallMessageForRpcWith2ComplexArgumentsUsingCurie;
                yield return CallMessageForRpcWithNoArguments;
            }
        }

        public static IEnumerable<WampMessage<MockRaw>> SubscribeMessages
        {
            get
            {
                yield return SubscribeMessageWithFullyQualifiedUri;
                yield return SubscribeMessageWithCurie;
            }
        }

        public static IEnumerable<WampMessage<MockRaw>> UnsubscribeMessages
        {
            get
            {
                yield return UnsubscribeMessageWithFullyQualifiedUri;
                yield return UnsubscribeMessageWithCurie;
            }
        }

        public static IEnumerable<WampMessage<MockRaw>> PublishMessages
        {
            get
            {
                yield return PublishMessageWithComplexObjectAsPayload;
                yield return PublishMessageWithComplexObjectAsPayloadDontExcludeMe;
                yield return PublishMessageWithComplexObjectAsPayloadExcludeMe;
                yield return PublishMessageWithEligibleList;
                yield return PublishMessageWithExcludeList;
                yield return PublishMessageWithExcludedAndEligibleList;
                yield return PublishMessageWithNullAsPayload;
                yield return PublishMessageWithStringAsPayload;
            }
        }

        public static IEnumerable<WampMessage<MockRaw>> PublishMessagesSimple
        {
            get
            {
                yield return PublishMessageWithComplexObjectAsPayload;
                yield return PublishMessageWithNullAsPayload;
                yield return PublishMessageWithStringAsPayload;
            }
        }

        public static IEnumerable<WampMessage<MockRaw>> PublishMessagesExcludeMe
        {
            get
            {
                yield return PublishMessageWithComplexObjectAsPayloadDontExcludeMe;
                yield return PublishMessageWithComplexObjectAsPayloadExcludeMe;
            }
        }

        public static IEnumerable<WampMessage<MockRaw>> PublishMessagesExclude
        {
            get
            {
                yield return PublishMessageWithExcludeList;
            }
        }

        public static IEnumerable<WampMessage<MockRaw>> PublishMessagesEligible
        {
            get
            {
                yield return PublishMessageWithEligibleList;
                yield return PublishMessageWithExcludedAndEligibleList;
            }
        }

        #endregion

        #region Client Packs

        public static IEnumerable<WampMessage<MockRaw>> ClientMessages => WelcomeMessages
                                                                          .Concat(CallResultMessages)
                                                                          .Concat(CallErrorMessages)
                                                                          .Concat(EventMessages);

        public static IEnumerable<WampMessage<MockRaw>> WelcomeMessages
        {
            get
            {
                yield return WelcomeMessage;
            }
        }

        public static IEnumerable<WampMessage<MockRaw>> CallResultMessages
        {
            get
            {
                yield return CallResultMessageWithNullResult;
                yield return CallResultMessageWithStringResult;
                yield return CallResultMessageWithComplexObjectResult;
            }
        }

        public static IEnumerable<WampMessage<MockRaw>> CallErrorMessagesSimple
        {
            get
            {
                yield return CallErrorMessageWithGenericError;
            }
        }

        public static IEnumerable<WampMessage<MockRaw>> CallErrorMessagesDetailed
        {
            get
            {
                yield return CallErrorMessageWithSpecificErrorAndIntegerInErrorDetails;
                yield return CallErrorMessageWithListOfIntegersInErrorDetails;
            }
        }

        public static IEnumerable<WampMessage<MockRaw>> CallErrorMessages => CallErrorMessagesSimple
            .Concat(CallErrorMessagesDetailed);

        public static IEnumerable<WampMessage<MockRaw>> EventMessages
        {
            get
            {
                yield return EventMessageWithStringAsPayload;
                yield return EventMessageWithNullAsPayload;
                yield return EventMessageWithComplexObjectPayload;
            }
        }

        #endregion
    }
}
