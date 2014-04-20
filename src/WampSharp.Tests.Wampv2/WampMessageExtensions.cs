using WampSharp.Core.Message;
using WampSharp.Tests.TestHelpers;

namespace WampSharp.Tests.Wampv2
{
    internal static class WampMessageExtensions
    {
        public static long? GetRequestId(this WampMessage<MockRaw> wampMessage)
        {
            if (wampMessage.MessageType == WampMessageType.v2Error)
            {
                long? result = wampMessage.Arguments[1].Value as long?;
                return result;
            }
            else if (wampMessage.Arguments.Length >= 1)
            {
                long? result = wampMessage.Arguments[0].Value as long?;
                return result;
            }

            return null;
        }
    }
}