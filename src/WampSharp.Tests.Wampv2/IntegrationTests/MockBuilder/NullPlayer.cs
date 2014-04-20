using WampSharp.Core.Message;

namespace WampSharp.Tests.Wampv2.IntegrationTests.MockBuilder
{
    public class NullPlayer<TMessage> : IMessagePlayer<TMessage>
    {
        public void Response(WampMessage<TMessage> message)
        {
        }
    }
}