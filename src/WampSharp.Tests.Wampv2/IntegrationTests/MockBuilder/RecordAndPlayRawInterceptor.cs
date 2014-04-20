using Castle.DynamicProxy;
using WampSharp.Core.Message;

namespace WampSharp.Tests.Wampv2.IntegrationTests.MockBuilder
{
    public class RecordAndPlayRawInterceptor<TMessage> : IInterceptor
    {
        private readonly IMessagePlayer<TMessage> mPlayer;
        private readonly IMessageRecorder<TMessage> mRecorder;

        public RecordAndPlayRawInterceptor(IMessagePlayer<TMessage> player, IMessageRecorder<TMessage> recorder)
        {
            mPlayer = player;
            mRecorder = recorder;
        }

        public void Intercept(IInvocation invocation)
        {
            WampMessage<TMessage> message = invocation.Arguments[0] as WampMessage<TMessage>;

            mPlayer.Response(message);
            mRecorder.Record(message);
        }
    }
}