using System.Collections.Generic;
using WampSharp.Core.Utilities;

namespace WampSharp.Core.Dispatch.Handler
{
    internal class DelegateCache<TKey, TMethod>
    {
        private readonly IDictionary<TKey, TMethod> mKeyToMethod = new SwapDictionary<TKey, TMethod>();
        private readonly IMethodBuilder<TKey, TMethod> mMethodBuilder;

        public DelegateCache(IMethodBuilder<TKey,TMethod> methodBuilder)
        {
            mMethodBuilder = methodBuilder;
        }

        public TMethod Get(TKey key)
        {

            if (mKeyToMethod.TryGetValue(key, out TMethod method))
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