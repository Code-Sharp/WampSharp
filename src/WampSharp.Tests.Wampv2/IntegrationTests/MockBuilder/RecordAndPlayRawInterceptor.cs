using Castle.DynamicProxy;
using WampSharp.Core.Message;
using WampSharp.Tests.TestHelpers;

namespace WampSharp.Tests.Wampv2.MockBuilder
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