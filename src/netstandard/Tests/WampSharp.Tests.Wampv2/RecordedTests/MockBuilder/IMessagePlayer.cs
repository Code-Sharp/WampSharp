using WampSharp.Core.Message;

namespace WampSharp.Tests.Wampv2.IntegrationTests.MockBuilder
{
    public interface IMessagePlayer<TMessage>
    {
        void Response(WampMessage<TMessage> message);
    }
}