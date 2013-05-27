using System;
using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Message;

namespace WampSharp.Tests.TestHelpers
{
    public class Messages
    {
        private static readonly WampMessage<MockRaw> mPrefixMessage2;
        private static readonly WampMessage<MockRaw> mPrefixMessage1;
        private static readonly WampMessage<MockRaw> mCallMessageForRpcWith2ArgumentsUsingCurie;
        private static readonly WampMessage<MockRaw> mCallMessageForRpcWith1ArgumentValueBeingNull;
        private static readonly WampMessage<MockRaw> mCallMessageForRpcWith1ArgumentValueBeingAListOfIntegersUsingCurie;
        private static readonly WampMessage<MockRaw> mCallMessageForRpcWith2ComplexArgumentsUsingCurie;
        private static readonly WampMessage<MockRaw> mCallMessageForRpcWith1ComplexObjectArgument;
        private static readonly WampMessage<MockRaw> mCallMessageForRpcWithNoArguments;
        private static readonly WampMessage<MockRaw> mSubscribeMessageWithFullyQualifiedUri;
        private static readonly WampMessage<MockRaw> mSubscribeMessageWithCurie;
        private static readonly WampMessage<MockRaw> mUnsubscribeMessageWithCurie;
        private static readonly WampMessage<MockRaw> mUnsubscribeMessageWithFullyQualifiedUri;
        private static readonly WampMessage<MockRaw> mPublishMessageWithComplexObjectAsPayload;
        private static readonly WampMessage<MockRaw> mPublishMessageWithNullAsPayload;
        private static readonly WampMessage<MockRaw> mPublishMessageWithStringAsPayload;
        private static readonly WampMessage<MockRaw> mPublishMessageWithComplexObjectAsPayloadExcludeMe;
        private static readonly WampMessage<MockRaw> mPublishMessageWithComplexObjectAsPayloadDontExcludeMe;
        private static readonly WampMessage<MockRaw> mPublishMessageWithExcludeList;
        private static readonly WampMessage<MockRaw> mPublishMessageWithEligibleList;
        private static readonly WampMessage<MockRaw> mPublishMessageWithExcludedAndEligibleList;
        private static readonly WampMessage<MockRaw> mWelcomeMessage;
        private static readonly WampMessage<MockRaw> mCallResultMessageWithNullResult;
        private static readonly WampMessage<MockRaw> mCallResultMessageWithStringResult;
        private static readonly WampMessage<MockRaw> mCallResultMessageWithComplexObjectResult;
        private static readonly WampMessage<MockRaw> mCallErrorMessageWithGenericError;
        private static readonly WampMessage<MockRaw> mCallErrorMessageWithSpecificErrorAndIntegerInErrorDetails;
        private static readonly WampMessage<MockRaw> mCallErrorMessageWithListOfIntegersInErrorDetails;
        private static readonly WampMessage<MockRaw> mEventMessageWithStringAsPayload;
        private static readonly WampMessage<MockRaw> mEventMessageWithNullAsPayload;
        private static readonly WampMessage<MockRaw> mEventMessageWithComplexObjectPayload;

        static Messages()
        {
            mPrefixMessage2 = new WampMessage<MockRaw>();
            {
                mPrefixMessage2.MessageType = WampMessageType.v1Prefix;
                MockRaw[] arguments = new MockRaw[2];
                arguments[0] = new MockRaw("keyvalue");
                arguments[1] = new MockRaw("http://example.com/simple/keyvalue#");
                mPrefixMessage2.Arguments = arguments;
            }
            mPrefixMessage1 = new WampMessage<MockRaw>();
            {
                mPrefixMessage1.MessageType = WampMessageType.v1Prefix;
                MockRaw[] arguments = new MockRaw[2];
                arguments[0] = new MockRaw("calc");
                arguments[1] = new MockRaw("http://example.com/simple/calc#");
                mPrefixMessage1.Arguments = arguments;
            }
            mCallMessageForRpcWith2ArgumentsUsingCurie = new WampMessage<MockRaw>();
            {
                mCallMessageForRpcWith2ArgumentsUsingCurie.MessageType = WampMessageType.v1Call;
                MockRaw[] arguments = new MockRaw[4];
                arguments[0] = new MockRaw("Yp9EFZt9DFkuKndg");
                arguments[1] = new MockRaw("api:add2");
                arguments[2] = new MockRaw(23);
                arguments[3] = new MockRaw(99);
                mCallMessageForRpcWith2ArgumentsUsingCurie.Arguments = arguments;
            }
            mCallMessageForRpcWith1ArgumentValueBeingNull = new WampMessage<MockRaw>();
            {
                mCallMessageForRpcWith1ArgumentValueBeingNull.MessageType = WampMessageType.v1Call;
                MockRaw[] arguments = new MockRaw[3];
                arguments[0] = new MockRaw("Dns3wuQo0ipOX1Xc");
                arguments[1] = new MockRaw("http://example.com/api#woooat");
                arguments[2] = new MockRaw(null);
                mCallMessageForRpcWith1ArgumentValueBeingNull.Arguments = arguments;
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
            mCallMessageForRpcWith2ComplexArgumentsUsingCurie = new WampMessage<MockRaw>();
            {
                mCallMessageForRpcWith2ComplexArgumentsUsingCurie.MessageType = WampMessageType.v1Call;
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
                mCallMessageForRpcWith2ComplexArgumentsUsingCurie.Arguments = arguments;
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
            mCallMessageForRpcWithNoArguments = new WampMessage<MockRaw>();
            {
                mCallMessageForRpcWithNoArguments.MessageType = WampMessageType.v1Call;
                MockRaw[] arguments = new MockRaw[2];
                arguments[0] = new MockRaw("7DK6TdN4wLiUJgNM");
                arguments[1] = new MockRaw("http://example.com/api#howdy");
                mCallMessageForRpcWithNoArguments.Arguments = arguments;
            }
            mSubscribeMessageWithFullyQualifiedUri = new WampMessage<MockRaw>();
            {
                mSubscribeMessageWithFullyQualifiedUri.MessageType = WampMessageType.v1Subscribe;
                MockRaw[] arguments = new MockRaw[1];
                arguments[0] = new MockRaw("http://example.com/simple");
                mSubscribeMessageWithFullyQualifiedUri.Arguments = arguments;
            }
            mSubscribeMessageWithCurie = new WampMessage<MockRaw>();
            {
                mSubscribeMessageWithCurie.MessageType = WampMessageType.v1Subscribe;
                MockRaw[] arguments = new MockRaw[1];
                arguments[0] = new MockRaw("event:myevent1");
                mSubscribeMessageWithCurie.Arguments = arguments;
            }
            mUnsubscribeMessageWithCurie = new WampMessage<MockRaw>();
            {
                mUnsubscribeMessageWithCurie.MessageType = WampMessageType.v1Unsubscribe;
                MockRaw[] arguments = new MockRaw[1];
                arguments[0] = new MockRaw("event:myevent1");
                mUnsubscribeMessageWithCurie.Arguments = arguments;
            }
            mUnsubscribeMessageWithFullyQualifiedUri = new WampMessage<MockRaw>();
            {
                mUnsubscribeMessageWithFullyQualifiedUri.MessageType = WampMessageType.v1Unsubscribe;
                MockRaw[] arguments = new MockRaw[1];
                arguments[0] = new MockRaw("http://example.com/simple");
                mUnsubscribeMessageWithFullyQualifiedUri.Arguments = arguments;
            }
            mPublishMessageWithComplexObjectAsPayload = new WampMessage<MockRaw>();
            {
                mPublishMessageWithComplexObjectAsPayload.MessageType = WampMessageType.v1Publish;
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
                mPublishMessageWithComplexObjectAsPayload.Arguments = arguments;
            }
            mPublishMessageWithNullAsPayload = new WampMessage<MockRaw>();
            {
                mPublishMessageWithNullAsPayload.MessageType = WampMessageType.v1Publish;
                MockRaw[] arguments = new MockRaw[2];
                arguments[0] = new MockRaw("http://example.com/simple");
                arguments[1] = new MockRaw(null);
                mPublishMessageWithNullAsPayload.Arguments = arguments;
            }
            mPublishMessageWithStringAsPayload = new WampMessage<MockRaw>();
            {
                mPublishMessageWithStringAsPayload.MessageType = WampMessageType.v1Publish;
                MockRaw[] arguments = new MockRaw[2];
                arguments[0] = new MockRaw("http://example.com/simple");
                arguments[1] = new MockRaw("Hello, world!");
                mPublishMessageWithStringAsPayload.Arguments = arguments;
            }
            mPublishMessageWithComplexObjectAsPayloadExcludeMe = new WampMessage<MockRaw>();
            {
                mPublishMessageWithComplexObjectAsPayloadExcludeMe.MessageType = WampMessageType.v1Publish;
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
                mPublishMessageWithComplexObjectAsPayloadExcludeMe.Arguments = arguments;
            }
            mPublishMessageWithComplexObjectAsPayloadDontExcludeMe = new WampMessage<MockRaw>();
            {
                mPublishMessageWithComplexObjectAsPayloadDontExcludeMe.MessageType = WampMessageType.v1Publish;
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
                mPublishMessageWithComplexObjectAsPayloadDontExcludeMe.Arguments = arguments;
            }
            mPublishMessageWithExcludeList = new WampMessage<MockRaw>();
            {
                mPublishMessageWithExcludeList.MessageType = WampMessageType.v1Publish;
                MockRaw[] arguments = new MockRaw[3];
                arguments[0] = new MockRaw("event:myevent1");
                arguments[1] = new MockRaw("hello");
                arguments[2] = new MockRaw(new[]
                                               {
                                                   "NwtXQ8rdfPsy-ewS",
                                                   "dYqgDl0FthI6_hjb",
                                               });
                mPublishMessageWithExcludeList.Arguments = arguments;
            }
            mPublishMessageWithEligibleList = new WampMessage<MockRaw>();
            {
                mPublishMessageWithEligibleList.MessageType = WampMessageType.v1Publish;
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
                mPublishMessageWithEligibleList.Arguments = arguments;
            }
            mPublishMessageWithExcludedAndEligibleList = new WampMessage<MockRaw>();
            {
                mPublishMessageWithExcludedAndEligibleList.MessageType = WampMessageType.v1Publish;
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
                mPublishMessageWithExcludedAndEligibleList.Arguments = arguments;
            }

            mWelcomeMessage =
                WampV1Messages.Welcome("v59mbCGDXZ7WTyxB", 1, "Autobahn/0.5.1");

            mCallResultMessageWithNullResult =
                WampV1Messages.CallResult("CcDnuI2bl2oLGBzO", null);

            mCallResultMessageWithStringResult =
                WampV1Messages.CallResult("otZom9UsJhrnzvLa", "Awesome result ..");

            mCallResultMessageWithComplexObjectResult =
                WampV1Messages.CallResult("CcDnuI2bl2oLGBzO",
                                          new
                                              {
                                                  value1 = 23,
                                                  value2 = "singsing",
                                                  value3 = true,
                                                  modified = new DateTime(2012, 3, 29, 10, 29, 16, 625),
                                              });

            mCallErrorMessageWithGenericError =
                WampV1Messages.CallError("gwbN3EDtFv6JvNV5",
                                         "http://autobahn.tavendo.de/error#generic",
                                         "math domain error");

            mCallErrorMessageWithSpecificErrorAndIntegerInErrorDetails =
                WampV1Messages.CallError("7bVW5pv8r60ZeL6u",
                                         "http://example.com/error#number_too_big",
                                         "1001 too big for me, max is 1000",
                                         1000);

            mCallErrorMessageWithListOfIntegersInErrorDetails =
                WampV1Messages.CallError("AStPd8RS60pfYP8c",
                                         "http://example.com/error#invalid_numbers",
                                         "one or more numbers are multiples of 3",
                                         new[] {0, 3});

            mEventMessageWithStringAsPayload =
                WampV1Messages.Event("http://example.com/simple", "Hello, I am a simple event.");

            mEventMessageWithNullAsPayload =
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

        public static WampMessage<MockRaw> CallErrorMessageWithListOfIntegersInErrorDetails
        {
            get { return mCallErrorMessageWithListOfIntegersInErrorDetails; }
        }

        public static WampMessage<MockRaw> CallErrorMessageWithSpecificErrorAndIntegerInErrorDetails
        {
            get { return mCallErrorMessageWithSpecificErrorAndIntegerInErrorDetails; }
        }

        public static WampMessage<MockRaw> CallErrorMessageWithGenericError
        {
            get { return mCallErrorMessageWithGenericError; }
        }

        public static WampMessage<MockRaw> CallResultMessageWithComplexObjectResult
        {
            get { return mCallResultMessageWithComplexObjectResult; }
        }

        public static WampMessage<MockRaw> CallResultMessageWithStringResult
        {
            get { return mCallResultMessageWithStringResult; }
        }

        public static WampMessage<MockRaw> CallResultMessageWithNullResult
        {
            get { return mCallResultMessageWithNullResult; }
        }

        public static WampMessage<MockRaw> WelcomeMessage
        {
            get { return mWelcomeMessage; }
        }

        public static WampMessage<MockRaw> PrefixMessage2
        {
            get { return mPrefixMessage2; }
        }

        public static WampMessage<MockRaw> PrefixMessage1
        {
            get { return mPrefixMessage1; }
        }

        public static WampMessage<MockRaw> CallMessageForRpcWith2ArgumentsUsingCurie
        {
            get { return mCallMessageForRpcWith2ArgumentsUsingCurie; }
        }

        public static WampMessage<MockRaw> CallMessageForRpcWith1ArgumentValueBeingNull
        {
            get { return mCallMessageForRpcWith1ArgumentValueBeingNull; }
        }

        public static WampMessage<MockRaw> CallMessageForRpcWith1ArgumentValueBeingAListOfIntegersUsingCurie
        {
            get { return mCallMessageForRpcWith1ArgumentValueBeingAListOfIntegersUsingCurie; }
        }

        public static WampMessage<MockRaw> CallMessageForRpcWith2ComplexArgumentsUsingCurie
        {
            get { return mCallMessageForRpcWith2ComplexArgumentsUsingCurie; }
        }

        public static WampMessage<MockRaw> CallMessageForRpcWith1ComplexObjectArgument
        {
            get { return mCallMessageForRpcWith1ComplexObjectArgument; }
        }

        public static WampMessage<MockRaw> CallMessageForRpcWithNoArguments
        {
            get { return mCallMessageForRpcWithNoArguments; }
        }

        public static WampMessage<MockRaw> SubscribeMessageWithFullyQualifiedUri
        {
            get { return mSubscribeMessageWithFullyQualifiedUri; }
        }

        public static WampMessage<MockRaw> SubscribeMessageWithCurie
        {
            get { return mSubscribeMessageWithCurie; }
        }

        public static WampMessage<MockRaw> UnsubscribeMessageWithCurie
        {
            get { return mUnsubscribeMessageWithCurie; }
        }

        public static WampMessage<MockRaw> UnsubscribeMessageWithFullyQualifiedUri
        {
            get { return mUnsubscribeMessageWithFullyQualifiedUri; }
        }

        public static WampMessage<MockRaw> PublishMessageWithComplexObjectAsPayload
        {
            get { return mPublishMessageWithComplexObjectAsPayload; }
        }

        public static WampMessage<MockRaw> PublishMessageWithNullAsPayload
        {
            get { return mPublishMessageWithNullAsPayload; }
        }

        public static WampMessage<MockRaw> PublishMessageWithStringAsPayload
        {
            get { return mPublishMessageWithStringAsPayload; }
        }

        public static WampMessage<MockRaw> PublishMessageWithComplexObjectAsPayloadExcludeMe
        {
            get { return mPublishMessageWithComplexObjectAsPayloadExcludeMe; }
        }

        public static WampMessage<MockRaw> PublishMessageWithComplexObjectAsPayloadDontExcludeMe
        {
            get { return mPublishMessageWithComplexObjectAsPayloadDontExcludeMe; }
        }

        public static WampMessage<MockRaw> PublishMessageWithExcludeList
        {
            get { return mPublishMessageWithExcludeList; }
        }

        public static WampMessage<MockRaw> PublishMessageWithEligibleList
        {
            get { return mPublishMessageWithEligibleList; }
        }

        public static WampMessage<MockRaw> PublishMessageWithExcludedAndEligibleList
        {
            get { return mPublishMessageWithExcludedAndEligibleList; }
        }

        public static WampMessage<MockRaw> EventMessageWithStringAsPayload
        {
            get { return mEventMessageWithStringAsPayload; }
        }

        public static WampMessage<MockRaw> EventMessageWithNullAsPayload
        {
            get { return mEventMessageWithNullAsPayload; }
        }

        public static WampMessage<MockRaw> EventMessageWithComplexObjectPayload
        {
            get { return mEventMessageWithComplexObjectPayload; }
        }

        #endregion

        #region Server Packs

        public static IEnumerable<WampMessage<MockRaw>> ServerMessages
        {
            get
            {
                return ServerAuxiliaryMessages
                    .Concat(ServerRpcMessages)
                    .Concat(ServerPubSubMessages);
            }
        }

        public static IEnumerable<WampMessage<MockRaw>> ServerAuxiliaryMessages
        {
            get
            {
                return PrefixMessages;
            }
        }


        public static IEnumerable<WampMessage<MockRaw>> ServerRpcMessages
        {
            get
            {
                return CallMessages;
            }
        }

        public static IEnumerable<WampMessage<MockRaw>> ServerPubSubMessages
        {
            get
            {
                return SubscribeMessages
                    .Concat(UnsubscribeMessages)
                    .Concat(PublishMessages);
            }
        }

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

        public static IEnumerable<WampMessage<MockRaw>> ClientMessages
        {
            get
            {
                return WelcomeMessages
                    .Concat(CallResultMessages)
                    .Concat(CallErrorMessages)
                    .Concat(EventMessages);
            }
        }

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

        public static IEnumerable<WampMessage<MockRaw>> CallErrorMessages
        {
            get
            {
                return CallErrorMessagesSimple
                    .Concat(CallErrorMessagesDetailed);
            }
        }

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
