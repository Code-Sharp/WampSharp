using System.Collections.Generic;

namespace WampSharp.Core.Dispatch.Handler
{
    internal class DelegateCache<TKey, TMethod>
    {
        private readonly IDictionary<TKey, TMethod> mKeyToMethod = new Dictionary<TKey, TMethod>();
        private readonly IMethodBuilder<TKey, TMethod> mMethodBuilder;

        public DelegateCache(IMethodBuilder<TKey,TMethod> methodBuilder)
        {
            mMethodBuilder = methodBuilder;
        }

        public TMethod Get(TKey key)
        {
            TMethod method;

            if (mKeyToMethod.TryGetValue(key, out method))
            {
                return method;
            }
            else
            {
                method = mMethodBuilder.BuildMethod(key);
                mKeyToMethod[key] = method;
                return method;
            }
        }
    }
}