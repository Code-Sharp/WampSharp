using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using WampSharp.Core.Utilities;
using WampSharp.V2.Client;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;

namespace WampSharp.V2.MetaApi
{
    public abstract class MetaApiEventsBase<TSubscriber>
    {
        private readonly IWampRealmProxy mRealmProxy;

        protected MetaApiEventsBase(IWampRealmProxy realmProxy)
        {
            mRealmProxy = realmProxy;
        }

        protected Task<IAsyncDisposable> InnerSubscribe(Delegate handler, string topic)
        {
            MethodInfo method = handler.Method;

            return
                mRealmProxy.TopicContainer.GetTopicByUri(topic)
                           .Subscribe(new MethodInfoSubscriber(handler.Target, method, topic),
                                      new SubscribeOptions());
        }

        protected Task<IAsyncDisposable> InnerSubscribe(Delegate handler, Expression<Action<TSubscriber>> expression)
        {
            return InnerSubscribe(handler, GetTopicUri(expression));
        }

        protected static string GetTopicUri(Expression<Action<TSubscriber>> expression)
        {
            return Method.Get(expression).GetCustomAttribute<WampTopicAttribute>()
                         .Topic;
        }
    }
}