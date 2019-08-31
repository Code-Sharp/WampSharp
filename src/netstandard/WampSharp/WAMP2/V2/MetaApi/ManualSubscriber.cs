using System;
using System.Linq.Expressions;
using System.Reflection;
using WampSharp.Core.Utilities;
using WampSharp.V2.PubSub;

namespace WampSharp.V2.MetaApi
{
    internal class ManualSubscriber<TSubscriber>
    {
        protected static string GetTopicUri(Expression<Action<TSubscriber>> expression)
        {
            return Method.Get(expression).GetCustomAttribute<WampTopicAttribute>()
                         .Topic;
        }
    }
}