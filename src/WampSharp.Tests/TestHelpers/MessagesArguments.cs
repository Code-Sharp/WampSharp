using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Message;

namespace WampSharp.Tests.TestHelpers
{
    public class MessagesArguments
    {
        private static object[] ExtractArguments(WampMessage<MockRaw> message)
        {
            return message.Arguments.Select(x => x.Value).ToArray();
        }

        private static object[] ExtractCallArguments(WampMessage<MockRaw> wampMessage)
        {
            MockRaw callId = wampMessage.Arguments[0];
            MockRaw procUri = wampMessage.Arguments[1];

            object[] arguments =
                wampMessage.Arguments.Skip(2)
                           .Select(x => x.Value).ToArray();

            return new object[] {callId.Value, procUri.Value, arguments};
        }

        public static IEnumerable<object[]> WelcomeMessages
        {
            get
            {
                return Messages.WelcomeMessages.Select(x => ExtractArguments(x));
            }
        }

        public static IEnumerable<object[]> PrefixMessages
        {
            get
            {
                return Messages.PrefixMessages.Select(x => ExtractArguments(x));
            }
        }

        public static IEnumerable<object[]> CallMessages
        {
            get
            {
                return Messages.CallMessages.Select(x => ExtractCallArguments(x));
            }
        }

        public static IEnumerable<object[]> CallErrorMessagesSimple
        {
            get
            {
                return Messages.CallErrorMessagesSimple.Select(x => ExtractArguments(x));
            }
        }

        public static IEnumerable<object[]> CallErrorMessagesDetailed
        {
            get
            {
                return Messages.CallErrorMessagesDetailed.Select(x => ExtractArguments(x));
            }
        }

        public static IEnumerable<object[]> CallResultMessages
        {
            get
            {
                return Messages.CallResultMessages.Select(x => ExtractArguments(x));
            }
        }

        public static IEnumerable<object[]> EventMessages
        {
            get
            {
                return Messages.EventMessages.Select(x => ExtractArguments(x));
            }
        }

        public static IEnumerable<object[]> PublishMessagesSimple
        {
            get
            {
                return Messages.PublishMessagesSimple.Select(x => ExtractArguments(x));
            }
        }

        public static IEnumerable<object[]> PublishMessagesExcludeMe
        {
            get
            {
                return Messages.PublishMessagesExcludeMe.Select(x => ExtractArguments(x));
            }
        }

        public static IEnumerable<object[]> PublishMessagesExclude
        {
            get
            {
                return Messages.PublishMessagesExclude.Select(x => ExtractArguments(x));
            }
        }

        public static IEnumerable<object[]> PublishMessagesEligible
        {
            get
            {
                return Messages.PublishMessagesEligible.Select(x => ExtractArguments(x));
            }
        }

        public static IEnumerable<object[]> SubscribeMessages
        {
            get
            {
                return Messages.SubscribeMessages.Select(x => ExtractArguments(x));
            }
        }

        public static IEnumerable<object[]> UnsubscribeMessages
        {
            get
            {
                return Messages.UnsubscribeMessages.Select(x => ExtractArguments(x));
            }
        }
    }
}