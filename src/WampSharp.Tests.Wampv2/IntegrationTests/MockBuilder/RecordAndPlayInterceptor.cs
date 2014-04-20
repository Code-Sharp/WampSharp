using Castle.DynamicProxy;
using WampSharp.Core.Message;
using WampSharp.Core.Proxy;

namespace WampSharp.Tests.Wampv2.MockBuilder
{
    public class RecordAndPlayInterceptor<TMessage> : IInterceptor
    {
        private readonly IWampOutgoingRequestSerializer<TMessage> mOutgoingSerializer;
        private readonly IMessagePlayer<TMessage> mPlayer;
        private readonly IMessageRecorder<TMessage> mRecorder;

        public RecordAndPlayInterceptor(IWampOutgoingRequestSerializer<TMessage> outgoingSerializer, IMessagePlayer<TMessage> player, IMessageRecorder<TMessage> recorder)
        {
            mOutgoingSerializer = outgoingSerializer;
            mPlayer = player;
            mRecorder = recorder;
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
            WampMessage<TMessage> serialized = SerializeRequest(invocation);
            mPlayer.Response(serialized);
        }

        private void Record(IInvocation invocation)
        {
            WampMessage<TMessage> serialized = SerializeRequest(invocation);
            mRecorder.Record(serialized);
        }

        private WampMessage<TMessage> SerializeRequest(IInvocation invocation)
        {
            return mOutgoingSerializer.SerializeRequest
                (invocation.Method, invocation.Arguments);
        }
    }
}