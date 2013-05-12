using System;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Message;

namespace WampSharp.Core.Dispatch
{
    public class WampIncomingMessageHandler<TMessage, TClient> : 
        IWampIncomingMessageHandler<TMessage, TClient>,
        IWampIncomingMessageHandler<TMessage>
    {
        private readonly IWampRequestMapper<WampMessage<TMessage>> mWampRequestMapper;
        private readonly DelegateCache<WampMethodInfo, Action<TClient, WampMessage<TMessage>>> mDelegateCache;

        public WampIncomingMessageHandler(IWampRequestMapper<WampMessage<TMessage>> wampRequestMapper,
                       IMethodBuilder<WampMethodInfo, Action<TClient, WampMessage<TMessage>>> methodBuilder)
        {
            mWampRequestMapper = wampRequestMapper;
            mDelegateCache = new DelegateCache<WampMethodInfo, Action<TClient, WampMessage<TMessage>>>(methodBuilder);
        }

        public void HandleMessage(TClient client, WampMessage<TMessage> message)
        {
            WampMethodInfo method = mWampRequestMapper.Map(message);

            if (method != null)
            {
                Action<TClient, WampMessage<TMessage>> action = mDelegateCache.Get(method);
                action(client, message);
            }
        }

        public void HandleMessage(WampMessage<TMessage> message)
        {
            HandleMessage(default(TClient), message);
        }
    }
}