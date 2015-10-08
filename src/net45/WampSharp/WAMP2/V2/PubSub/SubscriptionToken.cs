using System;
using WampSharp.V2.Core;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.PubSub
{
    internal class SubscriptionToken : IWampRegistrationSubscriptionToken
    {
        private readonly long mSubscriptionId;
        private readonly IDisposable mDisposable;

        public SubscriptionToken(long subscriptionId, IDisposable disposable)
        {
            mSubscriptionId = subscriptionId;
            mDisposable = disposable;
        }

        public long TokenId
        {
            get
            {
                return mSubscriptionId;
            }
        }

        public void Dispose()
        {
            mDisposable.Dispose();
        }
    }
}