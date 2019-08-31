using System;
using WampSharp.Logging;
using WampSharp.Core.Dispatch.Handler;
using WampSharp.Core.Message;

namespace WampSharp.Core.Dispatch
{
    /// <summary>
    /// An implementation of <see cref="IWampIncomingMessageHandler{TMessage,TClient}"/>,
    /// <see cref="IWampIncomingMessageHandler{TMessage}"/> that dispatches
    /// <see cref="WampMessage{TMessage}"/>s to their corresponding methods.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    /// <typeparam name="TClient"></typeparam>
    public class WampIncomingMessageHandler<TMessage, TClient> : 
        IWampIncomingMessageHandler<TMessage, TClient>,
        IWampIncomingMessageHandler<TMessage>
    {
        private readonly ILog mLogger;
        private readonly IWampRequestMapper<TMessage> mWampRequestMapper;
        private readonly DelegateCache<WampMethodInfo, Action<TClient, WampMessage<TMessage>>> mDelegateCache;

        /// <summary>
        /// Creates a new instance of <see cref="WampIncomingMessageHandler{TMessage,TClient}"/>
        /// given <paramref name="wampRequestMapper"/> and
        /// <paramref name="methodBuilder"/>
        /// </summary>
        /// <param name="wampRequestMapper">The <see cref="IWampRequestMapper{TRequest}"/>
        /// used in order to map <see cref="WampMessage{TMessage}"/>s to their suitable methods.</param>
        /// <param name="methodBuilder">The <see cref="IMethodBuilder{TKey,TMethod}"/> used 
        /// in order to build the corresponding methods.</param>
        public WampIncomingMessageHandler
            (IWampRequestMapper<TMessage> wampRequestMapper,
             IMethodBuilder<WampMethodInfo, Action<TClient, WampMessage<TMessage>>> methodBuilder)
        {
            mWampRequestMapper = wampRequestMapper;
            mLogger = LogProvider.GetLogger(this.GetType());
            mDelegateCache = new DelegateCache<WampMethodInfo, Action<TClient, WampMessage<TMessage>>>(methodBuilder);
        }

        public void HandleMessage(TClient client, WampMessage<TMessage> message)
        {
            WampMethodInfo method = mWampRequestMapper.Map(message);

            if (method != null)
            {
                mLogger.DebugFormat("Mapped message to method: {Method}", method.Method);
                Action<TClient, WampMessage<TMessage>> action = mDelegateCache.Get(method);
                action(client, message);
            }
            else
            {
                mLogger.Warn("Failed to map message to a suitable method handler");                
            }
        }

        public void HandleMessage(WampMessage<TMessage> message)
        {
            HandleMessage(default(TClient), message);
        }
    }
}