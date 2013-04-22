using System;
using WampSharp.Core.Contracts;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Message;

namespace WampSharp.Core.Dispatch
{
    public class WampIncomingMessageHandler<TMessage> : IWampIncomingMessageHandler<TMessage>
    {
        private readonly IWampRequestMapper<WampMessage<TMessage>> mWampRequestMapper;
        private readonly DelegateCache<WampMethodInfo, Action<IWampClient, TMessage[]>> mDelegateCache;

        public WampIncomingMessageHandler(IWampRequestMapper<WampMessage<TMessage>> wampRequestMapper,
                       IMethodBuilder<WampMethodInfo, Action<IWampClient, TMessage[]>> methodBuilder)
        {
            mWampRequestMapper = wampRequestMapper;
            mDelegateCache = new DelegateCache<WampMethodInfo, Action<IWampClient, TMessage[]>>(methodBuilder);
        }

        public void HandleMessage(IWampClient client, WampMessage<TMessage> message)
        {
            WampMethodInfo method = mWampRequestMapper.Map(message);
            Action<IWampClient, TMessage[]> action = mDelegateCache.Get(method);
            action(client, message.Arguments);
        }
    }
}