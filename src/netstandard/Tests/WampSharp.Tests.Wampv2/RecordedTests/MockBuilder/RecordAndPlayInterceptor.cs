using Castle.DynamicProxy;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;
using WampSharp.Tests.TestHelpers;
using WampSharp.V2.Binding;

namespace WampSharp.Tests.Wampv2.IntegrationTests.MockBuilder
{
    public class RecordAndPlayInterceptor<TMessage> : IInterceptor
    {
        private readonly IWampBinding<TMessage> mBinding; 
        private readonly IWampOutgoingRequestSerializer mOutgoingSerializer;
        private readonly IMessagePlayer<TMessage> mPlayer;
        private readonly IMessageRecorder<TMessage> mRecorder;

        public RecordAndPlayInterceptor(IWampOutgoingRequestSerializer outgoingSerializer, IMessagePlayer<TMessage> player, IMessageRecorder<TMessage> recorder, IWampBinding<TMessage> binding)
        {
            mOutgoingSerializer = outgoingSerializer;
            mPlayer = player;
            mRecorder = recorder;
            mBinding = binding;
        }

        /// <summary>
        /// <see cref="IInterceptor.Intercept"/>
        /// </summary>
        public void Intercept(IInvocation invocation)
        {
            Record(invocation);

            Play(invocation);
        }

        private void Play(IInvocation invocation)
        {
            WampMessage<object> serialized = SerializeRequest(invocation);
            mPlayer.Response(mBinding.Formatter.SerializeMessage(serialized));
        }

        private void Record(IInvocation invocation)
        {
            WampMessage<object> serialized = SerializeRequest(invocation);
            mRecorder.Record(mBinding.Formatter.SerializeMessage(serialized));
        }

        private WampMessage<object> SerializeRequest(IInvocation invocation)
        {
            return mOutgoingSerializer.SerializeRequest
                (invocation.Method, invocation.Arguments);
        }
    }
}