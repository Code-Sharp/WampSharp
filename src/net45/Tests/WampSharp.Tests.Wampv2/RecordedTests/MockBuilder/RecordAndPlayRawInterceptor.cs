using Castle.DynamicProxy;
using WampSharp.Core.Message;
using WampSharp.Tests.TestHelpers;
using WampSharp.V2.Binding;

namespace WampSharp.Tests.Wampv2.IntegrationTests.MockBuilder
{
    public class RecordAndPlayRawInterceptor<TMessage> : IInterceptor
    {
        private readonly IMessagePlayer<TMessage> mPlayer;
        private readonly IMessageRecorder<TMessage> mRecorder;
        private readonly IWampBinding<TMessage> mBinding;

        public RecordAndPlayRawInterceptor(IMessagePlayer<TMessage> player, IMessageRecorder<TMessage> recorder, IWampBinding<TMessage> binding)
        {
            mPlayer = player;
            mRecorder = recorder;
            mBinding = binding;
        }

        public void Intercept(IInvocation invocation)
        {
            WampMessage<object> message = invocation.Arguments[0] as WampMessage<object>;
            var typed = mBinding.Formatter.SerializeMessage(message);

            mPlayer.Response(typed);
            mRecorder.Record(typed);
        }
    }
}